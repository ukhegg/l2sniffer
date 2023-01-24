using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class UserInfoPacket : GameServerPacketBase
{
    public UserInfo Info;

    public UserInfoPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out Info);
    }
}