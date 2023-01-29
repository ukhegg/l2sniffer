using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

public class PartyInfo : DataStruct
{
    public GameObjectId Leader;
    public uint LootDistribution;
    public PartyMemberInfo[] Participants;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Leader);
        reader.Read(out LootDistribution);
        reader.Read(out uint otherPlayersCount);
        Participants = new PartyMemberInfo[otherPlayersCount];
        for (var i = 0; i < otherPlayersCount; ++i)
        {
            reader.Read(out PartyMemberInfo pm);
            Participants[i] = pm;
        }
    }
}