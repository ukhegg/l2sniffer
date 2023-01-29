using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class MagicSkillLaunchedPacket : GameServerPacketBase
{
    public GameObjectId CharId;
    public uint SkillId;
    public uint SkillLevel;
    public uint FailedOrNot;
    public GameObjectId TargetId;

    public MagicSkillLaunchedPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.Read(out SkillId);
        reader.Read(out SkillLevel);
        reader.Read(out FailedOrNot);
        reader.Read(out TargetId);
    }
}