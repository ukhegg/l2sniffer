using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public class IpV6PacketHandler : IPacketHandler<IPv6Packet>
{
    private IpPacketHandler _ipPacketHandler;

    public IpV6PacketHandler(IPacketHandlerProvider packetHandlerProvider)
    {
        _ipPacketHandler = new IpPacketHandler(packetHandlerProvider);
    }

    public void HandlePacket(IPv6Packet packet, PacketMetainfo metainfo)
    {
        _ipPacketHandler.HandlePacket(packet, metainfo);
    }
}