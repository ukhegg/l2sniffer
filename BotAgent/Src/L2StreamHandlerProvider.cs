using System.Collections.Concurrent;
using System.Net;
using L2sniffer;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;

namespace BotAgent;

public class L2StreamHandlerProvider : IL2StreamsHandlerProvider,
    INotifyL2SessionFound,
    IL2SessionPacketAsyncProvider
{
    private class PacketAccumulatingHandler : IPacketHandler<L2PacketBase>
    {
        public PacketAccumulatingHandler()
        {
            _accumulatedPackets = new ConcurrentQueue<Tuple<L2PacketBase, PacketMetainfo>>();
        }

        private ConcurrentQueue<Tuple<L2PacketBase, PacketMetainfo>> _accumulatedPackets;

        public void HandlePacket(L2PacketBase packet, PacketMetainfo metainfo)
        {
            _accumulatedPackets.Enqueue(new Tuple<L2PacketBase, PacketMetainfo>(packet, metainfo));
        }

        public async Task RedirectPacketsTo(IL2SessionPacketAsyncProvider.IPacketConsumer consumer,
                                            CancellationToken cst)
        {
            while (!cst.IsCancellationRequested)
            {
                if (!_accumulatedPackets.TryDequeue(out var next))
                {
                    await Task.Delay(100, cst);
                    continue;
                }

                await consumer.ConsumeAsync(next.Item1, next.Item2.CaptureTime, cst);
            }
        }
    }

    private Dictionary<StreamId, PacketAccumulatingHandler> _handlers;

    public L2StreamHandlerProvider()
    {
        _handlers = new Dictionary<StreamId, PacketAccumulatingHandler>();
    }

    public event L2SessionFoundEventHandler? OnL2SessionFound;

    public IPacketHandler<L2PacketBase> GetHandler(L2SessionTypes sessionType,
                                                   ConversationDirections direction,
                                                   IPEndPoint serverEndpoint,
                                                   IPEndPoint clientEndpoint)
    {
        var packetHandler = new PacketAccumulatingHandler();
        var streamId = direction == ConversationDirections.ServerToClient
            ? new StreamId(serverEndpoint, clientEndpoint)
            : new StreamId(clientEndpoint, serverEndpoint);
        
        _handlers[streamId] = packetHandler;

        var eventhandler = OnL2SessionFound;
        eventhandler?.Invoke(this, new L2SessionFoundEventArgs()
        {
            Direction = direction,
            ServerEndpoint = serverEndpoint,
            ClientEndpoint = clientEndpoint,
            SessionType = sessionType
        });
        return packetHandler;
    }

    public async Task GetSessionPacketsAsync(IPEndPoint serverEndpoint,
                                             IPEndPoint clientEndpoint,
                                             IL2SessionPacketAsyncProvider.IPacketConsumer consumer,
                                             CancellationToken cst)
    {
        var streamId = new StreamId(serverEndpoint, clientEndpoint);
        await _handlers[streamId].RedirectPacketsTo(consumer, cst);
    }
}