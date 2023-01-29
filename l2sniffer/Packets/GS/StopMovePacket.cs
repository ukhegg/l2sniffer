using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class StopMovePacket : GameServerPacketBase
{
    public GameObjectId Object;
    public Coordinates3d Coordinates;
    public uint Heading;

    public StopMovePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Object);
        reader.Read(out Coordinates);
        reader.Read(out Heading);
    }
}