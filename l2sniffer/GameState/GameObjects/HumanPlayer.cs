using System.Collections.ObjectModel;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public abstract class HumanPlayer : GameObject
{
    protected readonly PlayerAttributes _attributes;
    public WaitTypes WaitType { get; private set; }
    public GameObject Target { get; set; }

    public bool IsAutoAttacking { get; private set; }

    public abstract string Name { get; }

    protected ObservableCollection<PlayerMagicEffect> _appliedEffects;

    public PartyInfo Party { get; set; }

    public ReadOnlyObservableCollection<PlayerMagicEffect> MagicEffectsReadonly { get; private set; }

    public HumanPlayer(GameObjectId objectId) : base(objectId)
    {
        _attributes = new PlayerAttributes();
        _appliedEffects = new ObservableCollection<PlayerMagicEffect>();
        MagicEffectsReadonly = new ReadOnlyObservableCollection<PlayerMagicEffect>(_appliedEffects);
        IsAutoAttacking = false;
    }

    public override void ChangeWaitType(WaitTypes waitType, Coordinates3d packetDataCoordinates)
    {
        WaitType = waitType;
    }

    public override void SetTarget(GameObject target, Coordinates3d targetCoordinates)
    {
        Target = target;
    }


    public void Update(PartyMemberInfo partyMemberInfo)
    {
        _attributes.Update(partyMemberInfo);
    }

    public void AddMagicEffect(PlayerMagicEffect effect)
    {
        _appliedEffects.Add(effect);
    }

    public void StartAutoAttack(GameObject target)
    {
        Target = target;
        IsAutoAttacking = true;
    }

    public void StopAutoAttack(GameObject target)
    {
        IsAutoAttacking = false;
    }
}