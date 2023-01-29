using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class PlayerMagicEffect
{
    private SkillInfo _skill;
    private readonly DateTime _castTime;
    private readonly TimeSpan _duration;

    public PlayerMagicEffect(SkillInfo skillInfo,
                             DateTime castTime,
                             TimeSpan duration)
    {
        _skill = skillInfo;
        _castTime = castTime;
        _duration = duration;
    }

    public string Name => _skill.Name;

    public ushort Level => _skill.Level;

    public DateTime EndsAt => _castTime + _duration;
}