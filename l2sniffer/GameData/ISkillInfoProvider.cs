using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameData;

public interface ISkillInfoProvider
{
    SkillInfo GetSkillInfo(MagicSkillId skill);

    bool TryGetSkillInfo(MagicSkillId skillId, out SkillInfo skillInfo);
}