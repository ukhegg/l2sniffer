using L2sniffer;
using PacketDotNet;

namespace l2sniffer.PacketHandlers;

public interface ITcpStreamHandlerProvider
{
    IPacketHandler<TcpPacket> GetStreamHandler(IpDirection ipDirection, TransportDirection ports);
}