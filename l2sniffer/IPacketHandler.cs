namespace L2sniffer;

public interface IPacketHandler<TPacket>
{
    void HandlePacket(TPacket packet, PacketMetainfo metainfo);
}