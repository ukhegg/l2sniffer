using l2sniffer.PacketHandlers;

namespace L2sniffer.StreamHandlers;

public interface IDatagramStreamReaderProvider
{
    IDatagramStreamReader GetStreamReader(IpDirection ipDirection, TransportDirection ports);
}