namespace L2sniffer.Packets.GS;

public class GameServerPacketTypedPacket : L2PacketBase
{
    public GameServerPacketTypedPacket(byte[] bytes) : base(bytes)
    {
    }

    public GameServerPacketTypes PacketType => (GameServerPacketTypes)PacketTypeRaw;
}