using L2sniffer.Packets;

namespace L2sniffer.L2PacketHandlers;

public interface IL2PacketLogger
{
    void LogHandledPacket(L2PacketBase packet, PacketMetainfo metainfo);
    void LogUnhandledPacket(L2PacketBase packet, PacketMetainfo metainfo);
}