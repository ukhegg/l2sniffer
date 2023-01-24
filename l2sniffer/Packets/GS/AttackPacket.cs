using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class AttackPacket : GameServerPacketBase
{
    public AttackInfo AttackInfo;

    public AttackPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out AttackInfo);
    }
}