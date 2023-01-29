using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class PartySmallWindowAddPacket : GameServerPacketBase
{
    public GameObjectId Player;

    public PartyMemberInfo NewMember;
    
    public PartySmallWindowAddPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Player);
        reader.Read(out uint zero);
        reader.Read(out NewMember);
    }
}