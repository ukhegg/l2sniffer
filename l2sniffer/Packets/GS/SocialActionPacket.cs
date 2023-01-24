using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.GS;

public class SocialActionPacket : GameServerPacketBase
{
    public GameObjectId PlayerId;
    public uint ActionId;

    public SocialActionPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out PlayerId);
        reader.Read(out ActionId);
    }
}