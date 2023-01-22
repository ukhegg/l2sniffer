using System.Data;

namespace L2sniffer.Packets.GS;

public class GameServerPacketBase : TypeL2PacketBase<GameServerPacketTypes>
{
    public GameServerPacketBase(byte[] bytes) : base(bytes)
    {
    }
}