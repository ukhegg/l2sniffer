using System.Collections.ObjectModel;
using System.Collections.Specialized;
using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class MagicSkill
{
    public SkillInfo Info;

    public MagicSkill(SkillInfo info)
    {
        Info = info;
    }
}

public class Player : HumanPlayer
{
    private IItemNameProvider _itemNameProvider;
    private IActionNameProvider _actionNameProvider;
    private ISkillInfoProvider _skillInfoProvider;
    private IGameObjectsRegistry _objectsRegistry;
    private ObservableCollection<GameItem> _items;
    private ReadOnlyObservableCollection<GameItem> _itemsReadonly;

    private readonly CharacterInfo _characterInfo;
    public readonly SelectedCharInfo CharInfo;
    public UserInfo UserInfo;
    private Collection<MagicSkill> _activeSkills;
    private Collection<MagicSkill> _passiveSkills;

    public Player(CharacterInfo characterInfo,
                  SelectedCharInfo charInfo,
                  IItemNameProvider itemNameProvider,
                  IActionNameProvider actionNameProvider,
                  IGameObjectsRegistry objectsRegistry,
                  ISkillInfoProvider skillInfoProvider)
        : base(new GameObjectId(0))
    {
        _characterInfo = characterInfo;
        CharInfo = charInfo;
        Position = CharInfo.Coordinates;
        _itemNameProvider = itemNameProvider;
        _actionNameProvider = actionNameProvider;
        _objectsRegistry = objectsRegistry;
        _skillInfoProvider = skillInfoProvider;
        _attributes.Update(charInfo);
        _attributes.DictionaryChanged += (sender, args) =>
        {
            Console.WriteLine($"    Updating {Name} {args.Key}: {args.OldValue} --> {args.NewValue}");
        };
        _appliedEffects.CollectionChanged += (sender, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newEffect = (PlayerMagicEffect)args.NewItems[0];
                    Console.WriteLine(
                        $"    {Name} is now filling \'{newEffect.Name} lvl.{newEffect.Level}\' until {newEffect.EndsAt}");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var removedEffect = (PlayerMagicEffect)args.OldItems[0];
                    Console.WriteLine($"    {Name} does not fill\'{removedEffect.Name}\' no more");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        };

        _items = new ObservableCollection<GameItem>();
        _itemsReadonly = new ReadOnlyObservableCollection<GameItem>(_items);
        _items.CollectionChanged += (sender, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var addedItem = (GameItem)args.NewItems[0];
                    Console.WriteLine($"    {Name} got {addedItem.Count,5} of {addedItem.Name}");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var removedItem = (GameItem)args.OldItems[0];
                    Console.WriteLine($"    {Name} lost {removedItem.Count,5} of {removedItem.Name}");
                    break;
                default:
                    break;
            }
        };
    }

    public override string ObjectName => $"Player {CharInfo.Name}";
    public override string Name => CharInfo.Name;

    public ReadOnlyObservableCollection<GameItem> ItemsReadonly => _itemsReadonly;

    public virtual void IfPlayer(Action<Player> callback)
    {
        callback(this);
    }

    public void Update(ItemsList items)
    {
        foreach (var item in items.Items)
        {
            var itemName = _itemNameProvider.GetItem(item.ItemId);
            var gameItem = new GameItem(item, itemName);
            _items.Add(gameItem);
        }
    }

    public void Update(UserInfo info)
    {
        _attributes.Update(info);
        UserInfo = info;
    }

    public override void MoveToLocation(Coordinates3d current, Coordinates3d dst)
    {
        Position = current;
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

    public override void StopRotation(int degree)
    {
        base.StopRotation(degree);
    }

    public void SetTarget(GameObject target)
    {
        CurrentTarget = target;
    }

    public GameObject CurrentTarget { get; set; }

    public void AddInventoryItem(ItemsListEntry itemItem)
    {
        var gameItem = new GameItem(itemItem, _itemNameProvider.GetItem(itemItem.ItemType));
        _items.Add(gameItem);
    }

    public void ModifyInventoryItem(ItemsListEntry itemInfo)
    {
        _items.First(gameItem => gameItem.Id == itemInfo.ItemId).Update(itemInfo);
    }

    public void RemoveInventoryItem(ItemsListEntry itemInfo)
    {
        var item = _items.First(gameItem => gameItem.Id == itemInfo.ItemId);
        _items.Remove(item);
    }

    public void UnselectTarget(GameObject target, Coordinates3d targetCoordinates)
    {
        Target = null;
    }

    public void SetAvailableSkills(MagicSkillInfo[] skills)
    {
        _activeSkills = new Collection<MagicSkill>();
        _passiveSkills = new Collection<MagicSkill>();
        foreach (var skill in skills)
        {
            var skillInfo = _skillInfoProvider.GetSkillInfo(skill.SkillId);
            var s = new MagicSkill(skillInfo);
            if (skill.IsPassive)
            {
                _passiveSkills.Add(s);
            }
            else
            {
                _activeSkills.Add(s);
            }
        }
    }

    public void LeaveParty()
    {
        Party = null;
    }
}