using L2sniffer.Packets;

namespace L2sniffer.L2PacketHandlers;

public class ConsoleWritingPacketLogger : IL2PacketLogger
{
    private ulong _packetsLogged = 0;

    public void LogPacket(L2PacketBase loginServerPacket, PacketMetainfo metainfo)
    {
        Console.WriteLine($"#{_packetsLogged++}: {loginServerPacket.Bytes.Length} bytes");
    }
}