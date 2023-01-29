using L2sniffer.GameState;
using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public enum MoveType : uint
{
    Walk = 0x00,
    Run = 0x01,
}

public class ChangeMoveTypePacket : GameServerPacketBase
{
    public GameObjectId CharId;

    public MoveType MoveType;

    public ChangeMoveTypePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.ReadEnum(out MoveType);
    }
}