namespace L2sniffer.Packets.DataStructs;

public class CollisionInfo : DataStruct
{
    public double CollisionRadius;
    public double CollisionHeight;
    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out CollisionRadius);
        reader.Read(out CollisionHeight);
    }
}