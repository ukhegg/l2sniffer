using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class SetToLocationPacket : GameServerPacketBase
{
    public GameObjectId CharId;
    public Coordinates3d Location;
    public uint Heading;

    public SetToLocationPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.Read(out Location);
        reader.Read(out Heading);
    }
}