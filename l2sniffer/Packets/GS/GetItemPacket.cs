using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class GetItemPacket : GameServerPacketBase
{
    public GameObjectId PlayerId;
    public GameObjectId ObjectId;
    public Coordinates3d AtCoordinates;
    
    public GetItemPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out PlayerId);
        reader.Read(out ObjectId);
        reader.Read(out AtCoordinates);
    }
}