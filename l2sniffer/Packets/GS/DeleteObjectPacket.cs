using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class DeleteObjectPacket : GameServerPacketBase
{
    public GameObjectId ObjectId;
    
    public DeleteObjectPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out ObjectId);
    }
}