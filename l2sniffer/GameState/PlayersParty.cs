using System.Collections.ObjectModel;
using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState;

public class PlayersParty
{
    public ObservableCollection<OtherPlayer> Members;

    public PlayersParty(List<OtherPlayer> otherPlayers)
    {
        Members = new ObservableCollection<OtherPlayer>(otherPlayers);
    }
    
    public void Update(PartyMemberInfo packetParticipant)
    {
        Members.First(player => Equals(player.PlayerInfo.ObjectId, packetParticipant.PlayerId)).Update(packetParticipant);
    }

    public void AddMember(OtherPlayer newMember)
    {
        Members.Add(newMember);
    }
}