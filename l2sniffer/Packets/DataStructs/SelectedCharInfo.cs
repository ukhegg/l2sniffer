namespace L2sniffer.Packets.DataStructs;

public class SelectedCharInfo : DataStruct
{
    public string Name;
    public uint CharId;
    public string Title;
    public uint SessionId;
    public uint ClanId;
    public uint Sex;
    public uint Race;
    public uint Class;
    public uint Active;
    public Coordinates3d Coordinates;
    public double Hp;
    public double Mp;
    public uint Sp;
    public uint Exp;
    public uint Level;
    public CharStats Stats;
    public uint GameTime;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Name);
        reader.Read(out CharId);
        reader.Read(out Title);
        reader.Read(out SessionId);
        reader.Read(out ClanId);
        reader.Read(out uint _);
        reader.Read(out Sex);
        reader.Read(out Race);
        reader.Read(out Class);
        reader.Read(out Active);
        reader.Read(out Coordinates);
        reader.Read(out Hp);
        reader.Read(out Mp);
        reader.Read(out Sp);
        reader.Read(out Exp);
        reader.Read(out Level);
        reader.Read(out uint _);
        reader.Read(out uint _);
        reader.Read(out Stats);
        reader.Skip(30);
        reader.Skip(2 * sizeof(uint));
        reader.Skip(5 * sizeof(uint));
        reader.Read(out GameTime);
    }
}