using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class PartySmallWindowAllPacket : GameServerPacketBase
{
    public PartyInfo Info;

    public PartySmallWindowAllPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Info);
    }
}