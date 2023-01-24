using L2sniffer.Packets;

namespace L2sniffer.GameState.GameObjects;

public class GameObjectId : DataStruct, IComparable<GameObjectId>
{
    private uint _id;
    public uint Id => _id;

    public GameObjectId()
    {
    }

    public GameObjectId(uint id)
    {
        _id = id;
    }

    public override string ToString()
    {
        return $"#{_id:x8}";
    }

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out _id);
        if (_id == 1251014542)
        {
            int a = 0;
        }
    }

    protected bool Equals(GameObjectId other)
    {
        return _id == other._id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GameObjectId)obj);
    }
    
    
    public override int GetHashCode()
    {
        return (int)_id;
    }

    public int CompareTo(GameObjectId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return _id.CompareTo(other._id);
    }
    
    
}