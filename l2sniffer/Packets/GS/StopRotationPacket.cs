using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class StopRotationPacket : GameServerPacketBase
{
    public GameObjectId CharId;
    public int Degree;

    public StopRotationPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.Read(out Degree);
    }
}