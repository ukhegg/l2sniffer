using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class TargetSelectedPacket : GameServerPacketBase
{
    public GameObjectId Character;

    public GameObjectId Target;

    public Coordinates3d TargetCoordinates;

    public TargetSelectedPacket(byte[] bytes) : base(bytes)
    {
    }


    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Character);
        reader.Read(out Target);
        reader.Read(out TargetCoordinates);
    }
}