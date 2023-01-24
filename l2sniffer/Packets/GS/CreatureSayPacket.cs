using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class CreatureSayPacket : GameServerPacketBase
{
    public CreatureReplica Replica;

    public CreatureSayPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out Replica);
    }
}