using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class ChangeWaitTypePacket : GameServerPacketBase
{
    public ChangeWaitTypeData Data;

    public ChangeWaitTypePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out Data);
    }
}