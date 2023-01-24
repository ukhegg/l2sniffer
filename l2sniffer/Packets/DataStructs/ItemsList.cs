namespace L2sniffer.Packets.DataStructs;

public enum BodyPartTypes : uint
{
    LrEarring = 0x0006,
    Neck = 0x0008,
    LrFinger = 0x0030,
    Head = 0x0040,
    Unknown = 0x0080,
    LeftHand = 0x0100,
    Gloves = 0x0200,
    Chest = 0x0400,
    Pants = 0x0800,
    Feet = 0x1000,
    Unknown2 = 0x2000,
    RightHand = 0x4000,
    RightHand2 = 0x8000
}

public class ItemsListEntry : DataStruct
{
    public override string ToString()
    {
        return $"objId={ObjectId},typeId={ItemType},count={ItemCount}";
    }

    public ushort ItemType;
    public uint ObjectId;
    public uint ItemId;
    public uint ItemCount;
    public ushort ItemType2;
    public ushort CustomType1;
    public ushort IsEquipped;
    public BodyPartTypes BodyPart;
    public ushort EnchantLevel;
    public ushort CustomType2;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out ItemType);
        reader.Read(out ObjectId);
        reader.Read(out ItemId);
        reader.Read(out ItemCount);
        reader.Read(out ItemType2);
        reader.Read(out CustomType1);
        reader.Read(out IsEquipped);
        reader.ReadEnum(out BodyPart);
        reader.Read(out EnchantLevel);
        reader.Read(out CustomType2);
    }
    
    
}

public class ItemsList : DataStruct
{
    public ushort ShowWindow;
    public ItemsListEntry[] Items;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out ShowWindow);
        reader.Read(out ushort itemsCount);
        Items = new ItemsListEntry[itemsCount];
        for (var i = 0; i < itemsCount; ++i)
        {
            reader.Read(out Items[i]);
        }
    }
}