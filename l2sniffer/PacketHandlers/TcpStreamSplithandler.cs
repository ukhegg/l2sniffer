using L2sniffer;
using PacketDotNet;

namespace l2sniffer.PacketHandlers;

public class TcpStreamSplitHandler : IPacketHandler<TcpPacket>
{
    class StreamData
    {
        public IPacketHandler<TcpPacket> PacketHandler;

        public StreamData(IPacketHandler<TcpPacket> packetHandler)
        {
            PacketHandler = packetHandler;
        }
    }

    class IpConversationData
    {
        public IpConversationData()
        {
            Streams = new SortedDictionary<TransportDirection, StreamData>();
        }

        public SortedDictionary<TransportDirection, StreamData> Streams;
    }

    private SortedDictionary<IpDirection, IpConversationData> _ipConversations;
    private ITcpStreamHandlerProvider _handlerProvider;

    public TcpStreamSplitHandler(ITcpStreamHandlerProvider handlerProvider)
    {
        _handlerProvider = handlerProvider;
        _ipConversations = new SortedDictionary<IpDirection, IpConversationData>();
    }

    public void HandlePacket(TcpPacket packet, PacketMetainfo metainfo)
    {
        metainfo.TransportDirection = new TransportDirection(packet.SourcePort,
                                                             packet.DestinationPort);
        if (metainfo.TopLevelIpDirection == null)
        {
            throw new ArgumentException("no ip direction in metainfo");
        }

        if (!_ipConversations.TryGetValue(metainfo.TopLevelIpDirection, out var ipConversationData))
        {
            ipConversationData = new IpConversationData();
            _ipConversations[metainfo.TopLevelIpDirection] = ipConversationData;
        }

        StreamData streamData;
        if (!ipConversationData.Streams.TryGetValue(metainfo.TransportDirection, out streamData))
        {
            var handler = _handlerProvider.GetStreamHandler(metainfo.TopLevelIpDirection,
                                                            metainfo.TransportDirection);
            streamData = new StreamData(handler);
            ipConversationData.Streams[metainfo.TransportDirection] = streamData;
        }

        streamData.PacketHandler.HandlePacket(packet, metainfo);
    }
}