using L2sniffer.GameState.GameObjects;
using PacketDotNet.Ieee80211;

namespace L2sniffer.Packets.GS;

public class MyTargetSelectedPacket : GameServerPacketBase
{
    public GameObjectId ObjectId;

    public ushort Color;
    
    public MyTargetSelectedPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out ObjectId);
        reader.Read(out Color);
    }
}