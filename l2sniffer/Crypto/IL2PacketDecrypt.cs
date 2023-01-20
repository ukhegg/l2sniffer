using L2sniffer.Packets;

namespace L2sniffer.Crypto;

public interface IL2PacketDecrypt
{
    L2PacketBase DecryptPacket(L2PacketBase packet);
}