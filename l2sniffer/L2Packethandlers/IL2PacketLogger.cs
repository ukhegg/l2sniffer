using L2sniffer.Packets;

namespace L2sniffer.L2PacketHandlers;

public interface IL2PacketLogger
{
    void LogPacket(L2PacketBase packet, PacketMetainfo metainfo);
}