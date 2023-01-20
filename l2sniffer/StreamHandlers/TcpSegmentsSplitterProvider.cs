using l2sniffer.PacketHandlers;
using PacketDotNet;

namespace L2sniffer.StreamHandlers;

public class TcpSegmentsSplitterProvider : ITcpStreamHandlerProvider
{
    private IDatagramStreamReaderProvider _streamReaderProvider;
    private IDatagramStreamHandlerProvider _datagramStreamHandlerProvider;

    public TcpSegmentsSplitterProvider(IDatagramStreamReaderProvider streamReaderProvider,
                                       IDatagramStreamHandlerProvider datagramStreamHandlerProvider)
    {
        _streamReaderProvider = streamReaderProvider;
        _datagramStreamHandlerProvider = datagramStreamHandlerProvider;
    }


    public IPacketHandler<TcpPacket> GetStreamHandler(IpDirection ipDirection, TransportDirection ports)
    {
        return new TcpSegmentsSplitter(ipDirection,
                                       ports,
                                       _streamReaderProvider.GetStreamReader(ipDirection,
                                                                             ports),
                                       _datagramStreamHandlerProvider.GetDatagramHandler(ipDirection,
                                                                                         ports));
    }
}