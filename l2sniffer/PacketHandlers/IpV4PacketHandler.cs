using L2sniffer.PacketHandlers;
using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public class IpV4PacketHandler : IPacketHandler<IPv4Packet>
{
    private IpPacketHandler _ipPacketHandler;

    public IpV4PacketHandler(IPacketHandlerProvider packetHandlerProvider)
    {
        _ipPacketHandler = new IpPacketHandler(packetHandlerProvider);
    }

    public void HandlePacket(IPv4Packet packet, PacketMetainfo metainfo)
    {
        _ipPacketHandler.HandlePacket(packet, metainfo);
    }
}