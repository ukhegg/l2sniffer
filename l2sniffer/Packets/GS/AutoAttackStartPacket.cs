using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class AutoAttackStartPacket : GameServerPacketBase
{
    public GameObjectId TargetId;
    
    public AutoAttackStartPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out TargetId);
    }
}