namespace L2sniffer.Packets.GC;

public class GameClientPacketBase : TypeL2PacketBase<GameClientPacketTypes>
{
    public GameClientPacketBase(byte[] bytes) : base(bytes)
    {
    }
}