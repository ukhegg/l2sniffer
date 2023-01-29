namespace L2sniffer.Packets.DataStructs;

public class MagicSkillId : DataStruct, IComparable<MagicSkillId>
{
    public uint Id;
    public ushort Level;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Id);
        reader.Read(out Level);
    }

    protected bool Equals(MagicSkillId other)
    {
        return Id == other.Id && Level == other.Level;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MagicSkillId)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Level);
    }

    public int CompareTo(MagicSkillId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var idComparison = Id.CompareTo(other.Id);
        if (idComparison != 0) return idComparison;
        return Level.CompareTo(other.Level);
    }
}