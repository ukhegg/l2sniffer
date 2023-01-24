namespace L2sniffer.Packets.DataStructs;

public class CubicInfo : DataStruct
{
    public ushort[] Cubics;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out ushort cubicsSize);
        Cubics = new ushort[cubicsSize];
        reader.Read(ref Cubics);
    }
}