using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class DiePacket : GameServerPacketBase
{
    public GameObjectId CharId;
    public uint ReturnOption1;
    public uint ReturnOption2;
    public uint ReturnOption3;
    public uint Sweepable;
    public uint Success;

    public DiePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out CharId);
        reader.Read(out ReturnOption1);
        reader.Read(out ReturnOption2);
        reader.Read(out ReturnOption3);
        reader.Read(out Sweepable);
        reader.Read(out Success);
    }
}