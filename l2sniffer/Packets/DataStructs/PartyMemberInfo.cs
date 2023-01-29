using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

public class PartyMemberInfo : DataStruct
{
    public GameObjectId PlayerId;
    public string Name;
    public uint CurrentCp;
    public uint MaxCp;
    public uint CurrentHp;
    public uint MaxHp;
    public uint CurrentMp;
    public uint MaxMp;
    public uint Level;
    public CharacterClassIds ClassId;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out PlayerId);
        reader.Read(out Name);
        reader.Read(out CurrentCp);
        reader.Read(out MaxCp);
        reader.Read(out CurrentHp);
        reader.Read(out MaxHp);
        reader.Read(out CurrentMp);
        reader.Read(out MaxMp);
        reader.Read(out Level);
        reader.ReadEnum(out ClassId);
    }
}