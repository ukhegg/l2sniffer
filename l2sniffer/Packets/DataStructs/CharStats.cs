namespace L2sniffer.Packets.DataStructs;

public class CharStats : DataStruct
{
    public uint Int;
    public uint Str;
    public uint Con;
    public uint Men;
    public uint Dex;
    public uint Wit;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Int);
        reader.Read(out Str);
        reader.Read(out Con);
        reader.Read(out Men);
        reader.Read(out Dex);
        reader.Read(out Wit);
    }
}