using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class SkillListPacket : GameServerPacketBase
{
    public MagicSkillInfo[] Skills;

    public SkillListPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out uint skillCount);
        Skills = new MagicSkillInfo[skillCount];
        for (var i = 0; i < skillCount; ++i)
        {
            reader.Read(out MagicSkillInfo skill);
            Skills[i] = skill;
        }
    }
}