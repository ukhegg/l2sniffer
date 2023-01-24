namespace L2sniffer.Packets.DataStructs;

public class VisibleEquipment : DataStruct
{
    public uint Head;
    public uint RHand;
    public uint LHand;
    public uint Gloves;
    public uint Chest;
    public uint Legs;
    public uint Feet;
    public uint Back;
    public uint LRHand;
    public uint Hair;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Head);
        reader.Read(out RHand);
        reader.Read(out LHand);
        reader.Read(out Gloves);
        reader.Read(out Chest);
        reader.Read(out Legs);
        reader.Read(out Feet);
        reader.Read(out Back);
        reader.Read(out LRHand);
        reader.Read(out Hair);
    }
}