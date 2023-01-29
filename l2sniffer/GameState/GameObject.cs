using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;
using L2sniffer.Packets.GS;

namespace L2sniffer.GameState;

public abstract class GameObject
{
    public GameObjectId ObjectId;
    public Coordinates3d Position { get; set; }
    public uint Heading { get; set; }
    public bool IsSweepable { get; private set; } = false;

    public bool IsDead { get; private set; } = false;
    protected GameObject(GameObjectId objectId)
    {
        ObjectId = objectId;
    }

    public virtual string ObjectName => $"GameObject #{ObjectId.Id}";

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

    public virtual void SetMoveType(MoveType moveType)
    {
        throw new NotImplementedException();
    }

    public virtual void ChangeWaitType(WaitTypes waitType, Coordinates3d packetDataCoordinates)
    {
        throw new NotImplementedException();
    }

    public virtual void StopRotation(int i)
    {
        throw new NotImplementedException();
    }

    public virtual void UpdatePosition(Coordinates3d position)
    {
        Position = position;
    }

    public virtual void UpdatePosition(Coordinates3d position, uint heading)
    {
        Position = position;
        Heading = heading;
    }

    public virtual void SetTarget(GameObject target, Coordinates3d targetCoordinates)
    {
        throw new NotImplementedException();
    }

    public void StopMove(Coordinates3d coordinates, uint heading)
    {
        Position = coordinates;
        Heading = heading;
    }

    public void SetDead(bool isSweepable)
    {
        IsDead = true;
        IsSweepable = isSweepable;
    }


}