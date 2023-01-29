namespace L2sniffer.Packets.DataStructs;

public class MagicSkillInfo : DataStruct
{
    public MagicSkillId SkillId;
    public bool IsPassive;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out uint isPassive);
        IsPassive = isPassive == 0x01;
        reader.Read(out uint level);
        reader.Read(out uint id);
        SkillId = new MagicSkillId() { Id = id, Level = (ushort)level };
    }
}