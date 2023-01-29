using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class OtherPlayer : HumanPlayer
{
    private readonly IActionNameProvider _actionNameProvider;
    
    public OtherPlayer(OtherUserInfo otherUserPlayerInfo,
                       IActionNameProvider actionNameProvider)
        : base(otherUserPlayerInfo.ObjectId)
    {
        _actionNameProvider = actionNameProvider;
        _attributes.Update(otherUserPlayerInfo);
        _attributes.DictionaryChanged += (sender, args) =>
        {
            Console.WriteLine($"    Updating {Name} {args.Key}: {args.OldValue} --> {args.NewValue}");
        };
        PlayerInfo = otherUserPlayerInfo;
    }

    public OtherUserInfo PlayerInfo { get; private set; }

    public override string Name => PlayerInfo.Name;

    public override string ObjectName => $"Other player \'{Name}\'";

    public virtual void IfOtherPlayer(Action<OtherPlayer> callback)
    {
        callback(this);
    }

    public void Update(OtherUserInfo otherUserInfo)
    {
        if (!Equals(otherUserInfo.ObjectId, PlayerInfo.ObjectId))
        {
            throw new ArgumentException("mismatching object id");
        }

        PlayerInfo = otherUserInfo;
        _attributes.Update(otherUserInfo);
    }

    public override void Update(CharStatusUpdate charStatus)
    {
        _attributes.Update(charStatus);
    }



    public override void MoveToLocation(Coordinates3d current, Coordinates3d dst)
    {
        Console.WriteLine($"    \'{PlayerInfo.Name}\' is moving from {current} to {dst}");
        var distance = current.DistanceTo(dst);

        var speed = PlayerInfo.IsRunning == 0
            ? PlayerInfo.MovementSpeedsEx.WalkSpeed
            : PlayerInfo.MovementSpeedsEx.RunSpeed;

        var timetomove = distance / speed;
        Console.WriteLine($"        distance {distance}, time to move {timetomove}");
    }

    public override void HandleAction(uint actionId)
    {
        var actionName = _actionNameProvider.GetActionName(actionId);
        Console.WriteLine($"    player {this.Name} is performing action {actionId}");
    }

    public override void StopRotation(int degree)
    {
        PlayerInfo.Heading += degree;
    }
    

}