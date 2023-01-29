using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class DropItemPacket : GameServerPacketBase
{
    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out PlayerId);
        reader.Read(out ObjectId);
        reader.Read(out ItemId);
        reader.Read(out Coordinates);
        reader.Read(out IsStackable);
        reader.Read(out Count);
    }

    public GameObjectId PlayerId;
    public GameObjectId ObjectId;
    public uint ItemId;
    public Coordinates3d Coordinates;
    public uint IsStackable;
    public uint Count;

    public DropItemPacket(byte[] bytes) : base(bytes)
    {
    }
}