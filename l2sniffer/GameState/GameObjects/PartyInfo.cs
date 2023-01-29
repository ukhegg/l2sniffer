using System.Collections.ObjectModel;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class PartyInfo
{
    public PartyInfo(HumanPlayer leader)
    {
        Leader = leader;
        Members = new ObservableCollection<HumanPlayer>();
    }

    public HumanPlayer Leader { get; }

    public ObservableCollection<HumanPlayer> Members { get; }


    public void AddMember(HumanPlayer player)
    {
        Members.Add(player);
    }

    public void Update(PartyMemberInfo info)
    {
        Members.First(player => player.ObjectId.Equals(info.PlayerId)).Update(info);
    }

    public void Dismiss(GameObjectId memberId)
    {
        var member = Members.First(player => player.ObjectId.Equals(memberId));
        Console.WriteLine($"    {member.Name} left the party");
        Members.Remove(member);
    }
}