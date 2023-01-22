namespace L2sniffer.Packets.GC;

public class GameClientPacketTypedPacket : L2PacketBase
{
    public GameClientPacketTypedPacket(byte[] bytes) : base(bytes)
    {
    }

    public GameClientPacketTypes PacketType => (GameClientPacketTypes)PacketTypeRaw;
}