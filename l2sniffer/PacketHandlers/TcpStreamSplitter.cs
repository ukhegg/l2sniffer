using L2sniffer;
using L2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;
using PacketDotNet;

namespace l2sniffer.PacketHandlers;

public class TcpStreamSplitter : IPacketHandler<TcpPacket>
{
    class TcpHandlerWrapper : IAssembledTcpHandler
    {
        private IDatagramStreamHandler _datagramHandler;

        public TcpHandlerWrapper(IDatagramStreamHandler datagramHandler)
        {
            _datagramHandler = datagramHandler;
        }

        public void HandleReordered(TcpPacket packet, PacketMetainfo packetMetainfo)
        {
            _datagramHandler.HandleDatagram(packet.PayloadData, packetMetainfo);
        }

        public void HandleIntervalMissing(uint loweBound, uint upperBound)
        {
            _datagramHandler.HandleMissingInterval(loweBound, upperBound);
            throw new Exception($"unrecoverable error-interval missing [{loweBound},{upperBound}]");
        }

        public void HandlePartialOverlap(TcpPacket packet, PacketMetainfo packetMetainfo, uint overlapSize)
        {
            _datagramHandler.HandleDatagram(packet.PayloadData[(int)overlapSize..], packetMetainfo);
        }

        public void HandleOutOfIndexPacket(TcpPacket packet, PacketMetainfo packetMetainfo)
        {
        }
    }

    class StreamData
    {
        public IPacketHandler<TcpPacket> TcpAssembler;

        public StreamData(IPacketHandler<TcpPacket> tcpAssembler)
        {
            TcpAssembler = tcpAssembler;
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
    private IDatagramStreamHandlerProvider _handlerProvider;
    private readonly ITcpAssemblerProvider _tcpAssemblerProvider;

    public TcpStreamSplitter(IDatagramStreamHandlerProvider handlerProvider,
                                 ITcpAssemblerProvider tcpAssemblerProvider)
    {
        _handlerProvider = handlerProvider;
        _tcpAssemblerProvider = tcpAssemblerProvider;
        _ipConversations = new SortedDictionary<IpDirection, IpConversationData>();
    }

    public void HandlePacket(TcpPacket packet, PacketMetainfo metainfo)
    {
        metainfo.TransportPorts = new TransportDirection(packet.SourcePort,
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
        if (!ipConversationData.Streams.TryGetValue(metainfo.TransportPorts, out streamData))
        {
            var streamId = new StreamId(metainfo.TopLevelIpDirection, metainfo.TransportPorts);
            var handler = _handlerProvider.GetDatagramHandler(metainfo.TopLevelIpDirection,
                                                              metainfo.TransportPorts);
            var wrapper = new TcpHandlerWrapper(handler);
            var tcpAssembler = _tcpAssemblerProvider.GetTcpAssembler(streamId, wrapper);
            streamData = new StreamData(tcpAssembler);
            ipConversationData.Streams[metainfo.TransportPorts] = streamData;
        }

        streamData.TcpAssembler.HandlePacket(packet, metainfo);
    }
}