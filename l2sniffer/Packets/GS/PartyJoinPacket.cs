namespace L2sniffer.Packets.GS;

public class PartyJoinPacket : GameServerPacketBase
{
    public uint Response;

    public PartyJoinPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Response);
    }
}