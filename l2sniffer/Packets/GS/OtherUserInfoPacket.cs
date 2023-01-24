using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class OtherUserInfoPacket : GameServerPacketBase
{
    public OtherUserInfoPacket(byte[] bytes) : base(bytes)
    {
    }

    public OtherUserInfo OtherUserInfo;

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out OtherUserInfo);
    }
}