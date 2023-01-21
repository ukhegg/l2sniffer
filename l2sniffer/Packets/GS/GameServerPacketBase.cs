namespace L2sniffer.Packets.GS;

public class GameServerPacketBase : L2PacketBase
{
    public GameServerPacketBase(byte[] bytes) : base(bytes)
    {
    }

    public GameServerPacketTypes PacketType => (GameServerPacketTypes)PacketTypeRaw;
}