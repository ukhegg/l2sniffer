using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class ValidateLocationPacket : GameServerPacketBase
{
    public GameObjectId CharId;

    public Coordinates3d Position;

    public uint Heading;

    public ValidateLocationPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.Read(out Position);
        reader.Read(out Heading);
    }
}