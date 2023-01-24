using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class StatusUpdatePacket : GameServerPacketBase
{
    public CharStatusUpdate Updates;

    public StatusUpdatePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Updates);
    }
}