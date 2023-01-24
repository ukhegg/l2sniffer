using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class MoveToLocationPacket : GameServerPacketBase
{
    public MoveToLocationPacket(byte[] bytes) : base(bytes)
    {
    }

    public GameObjectId ObjectId;
    public Coordinates3d Dst;
    public Coordinates3d Current;

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out ObjectId);
        reader.Read(out Dst);
        reader.Read(out Current);
    }
}