namespace L2sniffer.Packets.DataStructs;

public class ClanAndAllyInfo : DataStruct
{
    public string Title;
    public uint ClanId;
    public uint ClanCrestId;
    public uint AllyId;
    public uint AllyCrestId;
    
    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Title);
        reader.Read(out ClanId);
        reader.Read(out ClanCrestId);
        reader.Read(out AllyId);
        reader.Read(out AllyCrestId);
    }
}