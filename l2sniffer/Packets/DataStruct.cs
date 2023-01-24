namespace L2sniffer.Packets;

public abstract class DataStruct
{
    protected uint GetUintField()
    {
        return 0;
    }

    public abstract void ReadFields(ref FieldsReader reader);
}