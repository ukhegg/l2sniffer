using L2sniffer;

namespace l2sniffer.PacketHandlers;

class StreamId : IComparable<StreamId>,
    IEquatable<StreamId>
{
    public StreamId(IpDirection ipDirection, TransportDirection transportDirection)
    {
        IpDirection = ipDirection;
        TransportDirection = transportDirection;
    }

    public readonly IpDirection IpDirection;
    public readonly TransportDirection TransportDirection;

    public int CompareTo(StreamId rhs)
    {
        var c1 = IpDirection.CompareTo(rhs.IpDirection);
        if (c1 != 0) return c1;
        return TransportDirection.CompareTo(rhs.TransportDirection);
    }

    public bool Equals(StreamId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return IpDirection.Equals(other.IpDirection) && TransportDirection.Equals(other.TransportDirection);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((StreamId)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IpDirection, TransportDirection);
    }
}