namespace L2sniffer.Packets.DataStructs;

public class MagicEffect : DataStruct
{
    public MagicSkillId Skill;
    public uint Duration;
    public TimeSpan TimeDuration => new TimeSpan(0, 0, (int)(Duration));

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Skill);
        reader.Read(out Duration);
    }
}