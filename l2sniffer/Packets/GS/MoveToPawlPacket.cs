using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class MoveToPawlPacket : GameServerPacketBase
{
    public GameObjectId CharId;

    public GameObjectId TargetId;

    public uint Distance;

    public Coordinates3d TargetCoordinates;

    public MoveToPawlPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.Read(out TargetId);
        reader.Read(out Distance);
        reader.Read(out TargetCoordinates);
    }
}