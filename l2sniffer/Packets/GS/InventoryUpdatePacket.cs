using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class InventoryUpdatePacket : GameServerPacketBase
{
    public ItemListEntryChange[] InventoryItems;

    public InventoryUpdatePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out ushort itemsCount);
        InventoryItems = new ItemListEntryChange[itemsCount];
        for (var i = 0; i < itemsCount; ++i)
        {
            reader.Read(out ItemListEntryChange change);
            InventoryItems[i] = change;
        }
    }
}