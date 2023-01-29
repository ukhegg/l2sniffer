using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class AutoAttackStopPacket : GameServerPacketBase
{
    public GameObjectId TargetId;
    
    public AutoAttackStopPacket(byte[] bytes) : base(bytes)
    {
    }
    
    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out TargetId);
    }
}