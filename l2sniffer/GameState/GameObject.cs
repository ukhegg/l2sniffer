using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;
using L2sniffer.Packets.GS;

namespace L2sniffer.GameState;

public abstract class GameObject
{
    public GameObjectId ObjectId;

    protected GameObject(GameObjectId objectId)
    {
        ObjectId = objectId;
    }

    public virtual string ObjectName => $"GameObject #{ObjectId.Id}";

    public virtual void IfNpc(Action<Npc> callback)
    {
    }

    public virtual void IfPlayer(Action<Player> callback)
    {
    }

    public virtual void IfOtherPlayer(Action<OtherPlayer> callback)
    {
    }

    public virtual void IfMorphedChar(Action<MorphedCharacter> callback)
    {
    }

    public virtual void Update(MorphedCharacterInfo info)
    {
        throw new NotImplementedException();
    }

    public virtual void Update(OtherCharacterInfo info)
    {
        throw new NotImplementedException();
    }

    public virtual void Update(UserInfo info)
    {
        throw new NotImplementedException();
    }

    public virtual void Update(CharStatusUpdate charStatus)
    {
        throw new NotImplementedException();
    }

    public virtual void MoveToLocation(Coordinates3d current, Coordinates3d dst)
    {
        throw new NotImplementedException();
    }

    public virtual void HandleAction(uint actionId)
    {
        throw new NotImplementedException();
    }

    public virtual void HandleAttack(AttackInfo packetAttackInfo)
    {
        throw new NotImplementedException();
    }
}