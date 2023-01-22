using l2sniffer.PacketHandlers;

namespace L2sniffer.StreamHandlers;

public class TcpSegmentsSplitterProvider : IDatagramStreamHandlerProvider
{
    private IDatagramStreamReaderProvider _streamReaderProvider;
    private IDatagramStreamHandlerProvider _datagramStreamHandlerProvider;

    public TcpSegmentsSplitterProvider(IDatagramStreamReaderProvider streamReaderProvider,
                                       IDatagramStreamHandlerProvider datagramStreamHandlerProvider)
    {
        _streamReaderProvider = streamReaderProvider;
        _datagramStreamHandlerProvider = datagramStreamHandlerProvider;
    }


    public IDatagramStreamHandler GetDatagramHandler(StreamId streamId)
    {
        return GetDatagramHandler(streamId.IpDirection, streamId.Ports);
    }

    public IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports)
    {
        return new TcpSegmentsSplitter(ipDirection,
                                       ports,
                                       _streamReaderProvider.GetStreamReader(ipDirection,
                                                                             ports),
                                       _datagramStreamHandlerProvider.GetDatagramHandler(ipDirection,
                                                                                         ports));
    }
}