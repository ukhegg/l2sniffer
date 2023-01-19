namespace L2sniffer;

public abstract class Direction<T, TDerived> : IComparable<Direction<T, TDerived>>,
    IEquatable<Direction<T, TDerived>>
    where TDerived : Direction<T, TDerived>
{
    public Direction(T source, T destination)
    {
        Source = source;
        Destination = destination;
    }

    public readonly T Source;
    public readonly T Destination;

    public TDerived Reverse()
    {
        return (TDerived)Activator.CreateInstance(typeof(TDerived), Destination, Source);
    }

    public int CompareTo(Direction<T, TDerived>? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var sourceComparison = Compare(Source, other.Source);
        if (sourceComparison != 0) return sourceComparison;
        return Compare(Destination, other.Destination);
    }

    public bool Equals(Direction<T, TDerived> other)
    {
        return Compare(Source, other.Source) == 0
               && Compare(Destination, other.Destination) == 0;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Direction<T, TDerived>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Source, Destination);
    }

    protected abstract int Compare(T lhs, T rhs);
}