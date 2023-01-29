using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

public class MagicSkillUseInfo : DataStruct
{
    public GameObjectId CharId;
    public GameObjectId TargetId;
    public MagicSkillId Skill;
    public uint HitTime;
    public uint ReuseDelay;
    public Coordinates3d Position;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.Read(out TargetId);
        reader.Read(out Skill);
        reader.Read(out HitTime);
        reader.Read(out ReuseDelay);
        reader.Read(out Position);
    }
}