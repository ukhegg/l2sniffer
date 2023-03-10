using System.Net;
using Google.Protobuf;
using Grpc.Core;
using L2sniffer;
using L2sniffer.L2PacketHandlers;
using L2sniffer.Packets;
using SharpPcap;

namespace BotAgent.GrpcServices;

public class SniffingService : BotAgent.SniffingService.SniffingServiceBase
{
    private readonly INetworkSniffer _sniffer;
    private readonly ICaptureProcessor _captureProcessor;
    private readonly IL2ServerRegistry _serversRegistry;
    private readonly INotifyL2SessionFound _l2SessionsNotifier;
    private string? _sniffingServer = null;
    private IL2SessionPacketAsyncProvider _l2PacketAsyncProvider;

    public SniffingService(INetworkSniffer sniffer,
                           ICaptureProcessor captureProcessor,
                           IL2ServerRegistry serversRegistry,
                           INotifyL2SessionFound l2SessionsNotifier,
                           IL2SessionPacketAsyncProvider l2PacketAsyncProvider)
    {
        _sniffer = sniffer;
        _captureProcessor = captureProcessor;
        _serversRegistry = serversRegistry;
        _l2SessionsNotifier = l2SessionsNotifier;
        _l2PacketAsyncProvider = l2PacketAsyncProvider;
    }


    public override Task<StartSniffingReply> StartSniffing(StartSniffingRequest request, ServerCallContext context)
    {
        try
        {
            var serverAddress = IPAddress.Parse(request.ServerIp);
            var loginServer = new IPEndPoint(serverAddress, (ushort)request.LoginServerPort);
            _serversRegistry.RegisterLoginServer(loginServer);
            if (request.GameServerPort != null)
            {
                var gameServer = new IPEndPoint(serverAddress, (ushort)request.GameServerPort.Value);
                _serversRegistry.RegisterGameServer(gameServer);
            }

            var captureInterface = _sniffer.Start(serverAddress, _captureProcessor);
            _sniffingServer = request.ServerIp;
            return Task.FromResult(new StartSniffingReply()
            {
                Success = true,
                CapturingOnInterface = captureInterface
            });
        }
        catch (Exception e)
        {
            return Task.FromResult(new StartSniffingReply()
            {
                Success = false,
                ErrorMessage = e.Message
            });
        }
    }

    public override Task<StopSniffingReply> StopSniffing(StopSniffingRequest request, ServerCallContext context)
    {
        try
        {
            _sniffer.Stop();
            return Task.FromResult(new StopSniffingReply() { Success = true });
        }
        catch (Exception e)
        {
            return Task.FromResult(new StopSniffingReply()
            {
                Success = false,
                ErrorMessage = e.Message
            });
        }
    }

    public override Task<GetStatusReply> GetStatus(GetStatusRequest request, ServerCallContext context)
    {
        var result = new GetStatusReply
        {
            Running = _sniffer.IsRunning,
            SniffingServer = _sniffingServer
        };
        return Task.FromResult(result);
    }

    public override async Task ReceiveL2SessionsNotifications(GetL2SessionsNotificationsRequest request,
                                                              IServerStreamWriter<GetL2SessionsNotificationsReply>
                                                                  responseStream,
                                                              ServerCallContext context)
    {
        var gotNewStreamEvent = new ManualResetEvent(false);
        GetL2SessionsNotificationsReply pendingReply = null;
        L2SessionFoundEventHandler handler = (sender, args) =>
        {
            pendingReply = new GetL2SessionsNotificationsReply()
            {
                SessionType = args.SessionType == L2SessionTypes.Game ? L2_SESSION_TYPES.Game : L2_SESSION_TYPES.Login,
                SessionId = new L2SessionId()
                {
                    ServerIp = args.ServerEndpoint.Address.ToString(),
                    ServerPort = (uint)args.ServerEndpoint.Port,
                    ClientIp = args.ClientEndpoint.Address.ToString(),
                    ClientPort = (uint)args.ClientEndpoint.Port,
                },
                Direction = args.Direction == ConversationDirections.ClientToServer
                    ? SessionDirections.ClientToServer
                    : SessionDirections.ServerToClient
            };
            gotNewStreamEvent.Set();
        };
        _l2SessionsNotifier.OnL2SessionFound += handler;

        var waitForEvent = () =>
        {
            gotNewStreamEvent.WaitOne();
            gotNewStreamEvent.Reset();
            GetL2SessionsNotificationsReply result = null;
            (result, pendingReply) = (pendingReply, result);
            return result;
        };

        while (!context.CancellationToken.IsCancellationRequested)
        {
            var reply = waitForEvent();
            await responseStream.WriteAsync(reply);
        }
    }

    public override async Task GetL2SessionPackets(GetL2SessionPacketsRequest request,
                                                   IServerStreamWriter<GetL2SessionPacketsReply> responseStream,
                                                   ServerCallContext context)
    {
        var consumer = new L2PacketConsumer(responseStream);
        var serverEndpoint = new IPEndPoint(IPAddress.Parse(request.SessionId.ServerIp),
                                            (ushort)request.SessionId.ServerPort);
        var clientEndpoint = new IPEndPoint(IPAddress.Parse(request.SessionId.ClientIp),
                                            (ushort)request.SessionId.ClientPort);
        await _l2PacketAsyncProvider.GetSessionPacketsAsync(serverEndpoint, clientEndpoint,
                                                            consumer,
                                                            context.CancellationToken);
    }

    private class L2PacketConsumer : IL2SessionPacketAsyncProvider.IPacketConsumer
    {
        private readonly IServerStreamWriter<GetL2SessionPacketsReply> _responseStream;

        public L2PacketConsumer(IServerStreamWriter<GetL2SessionPacketsReply> responseStream)
        {
            _responseStream = responseStream;
        }

        public async Task ConsumeAsync(L2PacketBase packet, PosixTimeval captureTime,
                                       CancellationToken cst)
        {
            var reply = new GetL2SessionPacketsReply()
            {
                PacketBytes = ByteString.CopyFrom(packet.Bytes),
                CaptureTime = (ulong)captureTime.Value
            };
            await _responseStream.WriteAsync(reply, cst);
        }
    }
}