namespace L2sniffer.GameData;

public class SkillInfo
{
    public uint Id;
    public ushort Level;
    public string Name;
    public string Description;
    public string DescriptionAdd1;
    public string DescriptionAdd2;

    public override string ToString()
    {
        return $"{Name} lvl.{Level}";
    }
}