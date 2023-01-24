using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class ItemsListPacket : GameServerPacketBase
{
    public ItemsList Items;
    public ItemsListPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out Items);
    }
}