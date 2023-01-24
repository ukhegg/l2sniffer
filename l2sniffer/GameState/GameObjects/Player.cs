using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class Player : GameObject
{
    private IItemNameProvider _itemNameProvider;
    private IActionNameProvider _actionNameProvider;
    private IGameObjectsRegistry _objectsRegistry;
    private PlayerAttributes _attributes;
    public readonly SelectedCharInfo CharInfo;
    public ItemsList Items;
    public UserInfo UserInfo;

    public Player(SelectedCharInfo charInfo,
                  IItemNameProvider itemNameProvider,
                  IActionNameProvider actionNameProvider,
                  IGameObjectsRegistry objectsRegistry)
        : base(new GameObjectId(0))
    {
        CharInfo = charInfo;
        _itemNameProvider = itemNameProvider;
        _actionNameProvider = actionNameProvider;
        _objectsRegistry = objectsRegistry;
        _attributes = new PlayerAttributes();
        _attributes.Update(charInfo);
        _attributes.DictionaryChanged += (sender, args) =>
        {
            Console.WriteLine($"    Updating {Name} {args.Key}: {args.OldValue} --> {args.NewValue}");
        };
    }

    public override string ObjectName => $"Player {CharInfo.Name}";
    public string Name => CharInfo.Name;

    public override void IfPlayer(Action<Player> callback)
    {
        callback(this);
    }

    public void Update(ItemsList items)
    {
        Items = items;
        Console.WriteLine($"    Updating main char inventory,got {items.Items.Length} items");
        foreach (var item in items.Items)
        {
            var itemName = _itemNameProvider.GetItem(item.ItemId);
            Console.WriteLine($"        got {item.ItemCount,5} of {itemName.Name}");
        }
    }

    public void Update(UserInfo info)
    {
        _attributes.Update(info);
        UserInfo = info;
    }

    public override void MoveToLocation(Coordinates3d current, Coordinates3d dst)
    {
        Console.WriteLine($"    Player {Name} is moving from {current} to {dst}");
    }

    public override void HandleAttack(AttackInfo packetAttackInfo)
    {
        var firstTarget = _objectsRegistry.GetObject(packetAttackInfo.FirstHit.TargetId);
        Console.WriteLine(
            $"    {ObjectName} is attacking {firstTarget.ObjectName},{packetAttackInfo.FirstHit.Damage} damage done");
        foreach (var hit in packetAttackInfo.Hits)
        {
            var hitTarget = _objectsRegistry.GetObject(hit.TargetId);
            Console.WriteLine($"        additional hit for {hitTarget.ObjectName},{hit.Damage} damage done");
        }
    }

    public override void HandleAction(uint actionId)
    {
        var actionName = _actionNameProvider.GetActionName(actionId);
        Console.WriteLine($"    {this.Name} is performing action {actionName.Name}");
    }

    public override void Update(CharStatusUpdate charStatus)
    {
        _attributes.Update(charStatus);
    }
}