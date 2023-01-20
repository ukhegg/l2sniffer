using l2sniffer.PacketHandlers;

namespace L2sniffer.StreamHandlers;

public interface IDatagramStreamHandlerProvider
{
    IDatagramStreamHandler GetDatagramHandler(StreamId streamId);
    IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports);
}