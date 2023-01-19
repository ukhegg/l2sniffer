using L2sniffer;
using PacketDotNet;

namespace l2sniffer.PacketHandlers;

public class TcpPacketHandler : IPacketHandler<TcpPacket>
{
    public void HandlePacket(TcpPacket packet, PacketMetainfo metainfo)
    {
        throw new NotImplementedException();
    }
}