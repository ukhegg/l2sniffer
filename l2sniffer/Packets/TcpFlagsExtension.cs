using PacketDotNet;

namespace L2sniffer.Packets;

static class TcpFlagsExtension
{
    public static TcpFlags Flags2(this TcpPacket packet)
    {
        return new TcpFlags(packet.Flags);
    }
}