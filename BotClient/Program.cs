using System.Net;
using BotAgent;
using Grpc.Core;
using Grpc.Net.Client;

namespace BotClient;

class L2SessionHandler
{
    public void HandleNewSession(IPEndPoint serverIp,
                                 IPEndPoint clientIp,
                                 SessionDirections sessionDataDirection,
                                 IAsyncStreamReader<GetL2SessionPacketsReply> packetsStream)
    {
        Console.WriteLine("Handling new session in L2SessionHandler");
        var task = new Task(async () =>
        {
            var src = sessionDataDirection == SessionDirections.ClientToServer ? clientIp : serverIp;
            var dst = sessionDataDirection == SessionDirections.ClientToServer ? serverIp : clientIp;

            while (await packetsStream.MoveNext())
            {
                var nextPacket = packetsStream.Current;
                Console.WriteLine(
                    $"Got {nextPacket.PacketBytes.Length}-bytes packet from {src} to {dst}");
            }
        });

        ThreadPool.QueueUserWorkItem(state =>
        {
            Console.WriteLine("Starting packet processing");
            task.RunSynchronously();
        });
    }
}

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var channel = GrpcChannel.ForAddress("http://localhost:13642");
        var botClient = new SniffingService.SniffingServiceClient(channel);

        var reply = botClient.StartSniffing(new StartSniffingRequest()
        {
            ServerIp = "83.166.99.220",
            LoginServerPort = 2106
        });
        Console.WriteLine($"Sniffing server 83.166.99.220 through interface {reply.CapturingOnInterface}");


        L2SessionHandler sessionsHandler = new L2SessionHandler();

        Console.WriteLine("Waiting for Lineage II sessions...");
        var notifications = botClient.ReceiveL2SessionsNotifications(new GetL2SessionsNotificationsRequest());
        var cts = new CancellationTokenSource();
        try
        {
            await foreach (var sessionData in notifications.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
            {
                Console.WriteLine($"Found new {sessionData.SessionType} session: " +
                                  $"    Server={sessionData.SessionId.ServerIp}:{sessionData.SessionId.ServerPort}" +
                                  $"    Client={sessionData.SessionId.ClientIp}:{sessionData.SessionId.ClientPort}");
                var packetReply = botClient.GetL2SessionPackets(
                    new GetL2SessionPacketsRequest()
                    {
                        SessionId = sessionData.SessionId,
                        Direction = sessionData.Direction
                    });
                Console.WriteLine("got session packet stream");
                var serverIp = new IPEndPoint(IPAddress.Parse(sessionData.SessionId.ServerIp),
                                              (ushort)sessionData.SessionId.ServerPort);
                var clientIp = new IPEndPoint(IPAddress.Parse(sessionData.SessionId.ClientIp),
                                              (ushort)sessionData.SessionId.ClientPort);
                sessionsHandler.HandleNewSession(serverIp,
                                                 clientIp,
                                                 sessionData.Direction,
                                                 packetReply.ResponseStream);
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            Console.WriteLine("Stream cancelled.");
        }

        Console.ReadKey();
        return 0;
    }
}