using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class PartySmallWindowUpdatePacket : GameServerPacketBase
{
    public PartyMemberInfo Participant;

    public PartySmallWindowUpdatePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Participant);
    }
}