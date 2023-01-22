namespace L2sniffer.Packets.GC;

public class GameClientPacketBase : L2PacketBase
{
    public GameClientPacketBase(byte[] bytes) : base(bytes)
    {
    }

    public GameClientPacketTypes PacketType => (GameClientPacketTypes)PacketTypeRaw;
}