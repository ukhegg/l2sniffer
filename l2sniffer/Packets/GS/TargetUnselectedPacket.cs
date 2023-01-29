using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class TargetUnselectedPacket : GameServerPacketBase
{
    public GameObjectId Target;
    
    public Coordinates3d TargetCoordinates;
    
    public TargetUnselectedPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Target);
        reader.Read(out TargetCoordinates);
    }
}