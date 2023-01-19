using l2sniffer.PacketHandlers;
using PacketDotNet;

namespace L2sniffer.StreamHandlers;

public class TcpSegmentsSplitterProvider : ITcpStreamHandlerProvider
{
    private IDatagramStreamReaderProvider _streamReaderProvider;

    public TcpSegmentsSplitterProvider(IDatagramStreamReaderProvider streamReaderProvider)
    {
        _streamReaderProvider = streamReaderProvider;
    }

    class DummyDatagramHandler : IDatagramStreamHandler
    {
        public void HandleDatagram(byte[] datagram)
        {
            Console.WriteLine($"handling datagram {datagram.Length} bytes");
        }
    }


    public IPacketHandler<TcpPacket> GetStreamHandler(IpDirection ipDirection, TransportDirection ports)
    {
        return new TcpSegmentsSplitter(ipDirection,
                                       ports,
                                       _streamReaderProvider.GetStreamReader(ipDirection,
                                                                             ports),
                                       new DummyDatagramHandler());
    }
}