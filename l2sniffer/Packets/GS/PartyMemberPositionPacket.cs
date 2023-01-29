using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class PartyMemberPositionPacket : GameServerPacketBase
{
    public Dictionary<GameObjectId, Coordinates3d> MemberPositions;

    public PartyMemberPositionPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        MemberPositions = new Dictionary<GameObjectId, Coordinates3d>();
        reader.Read(out uint membersCount);
        for (var i = 0; i < membersCount; ++i)
        {
            reader.Read(out GameObjectId id);
            reader.Read(out Coordinates3d coordinates);
            MemberPositions[id] = coordinates;
        }
    }
}