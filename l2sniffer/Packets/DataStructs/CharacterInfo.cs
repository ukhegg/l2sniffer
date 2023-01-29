namespace L2sniffer.Packets.DataStructs;

public class CharacterInfo : DataStruct
{
    public string Name;
    public uint Id;
    public string Login;
    public uint Id2;
    public uint SessionId;
    public uint ClanId;
    public uint Unknown1;
    public uint Gendre;
    public uint Race;
    public CharacterClassIds Klass;
    public uint IsActive;
    public Coordinates3d CoordinatedUnused;
    public double Hp;
    public double Mp;
    public uint Sp;
    public uint Exp;
    public uint Level;
    public uint Carma; //std::array<uint8_t, 36> zeroes_unknown;
    public IdsSet ItemObjectIds;
    public IdsSet ItemIds;
    public uint Pri4Eska;
    public uint HairColor;
    public uint FaceType;
    public double MaxHp;
    public double MaxMp;
    public uint DeleteTimeout;
    public uint BaseKlassId;
    public uint LastLogin;
    public byte AugLevel;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out this.Name);
        reader.Read(out this.Id);
        reader.Read(out this.Login);
        //reader.Read(out this.Id2);
        reader.Read(out this.SessionId);
        reader.Read(out this.ClanId);
        reader.Read(out this.Unknown1);
        reader.Read(out this.Gendre);
        reader.Read(out this.Race);
        reader.ReadEnum(out this.Klass);
        reader.Read(out this.IsActive);
        reader.Read(out this.CoordinatedUnused);
        reader.Read(out this.Hp);
        reader.Read(out this.Mp);
        reader.Read(out this.Sp);
        reader.Read(out this.Exp);
        reader.Read(out this.Level);
        reader.Read(out this.Carma);
        reader.Skip(36);
        reader.Read(out this.ItemObjectIds);
        reader.Read(out this.ItemIds);
        reader.Read(out this.Pri4Eska);
        reader.Read(out this.HairColor);
        reader.Read(out this.FaceType);
        reader.Read(out this.MaxHp);
        reader.Read(out this.MaxMp);
        reader.Read(out this.DeleteTimeout);
        reader.Read(out this.BaseKlassId);
        reader.Read(out this.LastLogin);
        reader.Read(out this.AugLevel);
    }
};