namespace L2sniffer.Packets.DataStructs;

public class IdsSet : DataStruct
{
    public uint Under;
    public uint RAr;
    public uint LAr;
    public uint Neck;
    public uint RFinger;
    public uint LFinger;
    public uint Head;
    public uint RHand;
    public uint LHand;
    public uint Gloves;
    public uint Chest;
    public uint Legs;
    public uint Feet;
    public uint Back;
    public uint LrHand;
    public uint Hair;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Under);
        reader.Read(out RAr);
        reader.Read(out LAr);
        reader.Read(out Neck);
        reader.Read(out RFinger);
        reader.Read(out LFinger);
        reader.Read(out Head);
        reader.Read(out RHand);
        reader.Read(out LHand);
        reader.Read(out Gloves);
        reader.Read(out Chest);
        reader.Read(out Legs);
        reader.Read(out Feet);
        reader.Read(out Back);
        reader.Read(out LrHand);
        reader.Read(out Hair);
    }
}