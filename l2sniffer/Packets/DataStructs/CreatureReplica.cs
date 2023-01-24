namespace L2sniffer.Packets.DataStructs;

public class CreatureReplica : DataStruct
{
    public uint ObjectId;
    public uint TextType;
    public string CharName;
    public string Text;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out ObjectId);
        reader.Read(out TextType);
        reader.Read(out CharName);
        reader.Read(out Text);
    }
}