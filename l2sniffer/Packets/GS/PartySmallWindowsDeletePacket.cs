using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class PartySmallWindowsDeletePacket : GameServerPacketBase
{
    public GameObjectId MemberId;
    public string MemberName;

    public PartySmallWindowsDeletePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out MemberId);
        reader.Read(out MemberName);
    }
}