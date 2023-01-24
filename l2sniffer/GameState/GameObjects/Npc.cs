using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;
using L2sniffer.Packets.GS;

namespace L2sniffer.GameState.GameObjects;

public class Npc : GameObject
{
    private MorphedCharacterInfo _info;
    private readonly NpcName _name;
    private readonly IActionNameProvider _actionNameProvider;
    private readonly PlayerAttributes _attributes;
    private readonly IGameObjectsRegistry _objectsRegistry;

    public Npc(MorphedCharacterInfo info,
               NpcName name,
               IActionNameProvider actionNameProvider,
               IGameObjectsRegistry objectsRegistry)
        : base(info.ObjectId)
    {
        _info = info;
        _name = name;
        _actionNameProvider = actionNameProvider;
        _objectsRegistry = objectsRegistry;
        _attributes = new PlayerAttributes();
        _attributes.DictionaryChanged += (sender, args) =>
        {
            Console.WriteLine($"    Updating Npc {Name} {args.Key}: {args.OldValue} --> {args.NewValue}");
        };
    }

    public string Name => _name.Name;

    public override string ObjectName => $"NPC {_name.Name}";

    public override void IfNpc(Action<Npc> callback)
    {
        callback(this);
    }

    public override void Update(MorphedCharacterInfo monsterInfo)
    {
        Console.WriteLine($"    Updating \'{_name.Name}\' info, id {monsterInfo.ObjectId}");
        _info = monsterInfo;
    }

    public override void Update(CharStatusUpdate charStatus)
    {
        _attributes.Update(charStatus);
    }

    public override void MoveToLocation(Coordinates3d current, Coordinates3d dst)
    {
        Console.WriteLine($"    \'{_name.Name}\' is moving from {current} to {dst}");
        var distance = current.DistanceTo(dst);

        var speed = _info.IsRunning == 0
            ? _info.MovementSpeeds.WalkSpeed
            : _info.MovementSpeeds.RunSpeed;

        var timetomove = distance / speed;
        Console.WriteLine($"        distance {distance}, time to move {timetomove}");
    }

    public override void HandleAction(uint actionId)
    {
        var actionName = _actionNameProvider.GetActionName(actionId);
        Console.WriteLine($"    Npc {_name.Name} performing action {actionName.Name}");
    }

    public override void HandleAttack(AttackInfo packetAttackInfo)
    {
        var hitTarget = _objectsRegistry.GetObject(packetAttackInfo.FirstHit.TargetId);
        Console.WriteLine($"    Npc {_name.Name} attacked {hitTarget.ObjectName}");
        foreach (var hit in packetAttackInfo.Hits)
        {
            var nextTarget = _objectsRegistry.GetObject(hit.TargetId);
            Console.WriteLine($"        additional hit for {nextTarget.ObjectName},{hit.Damage} damage done");
        }
    }
}