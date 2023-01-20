using L2sniffer;

namespace l2sniffer.PacketHandlers;

public class StreamId : IComparable<StreamId>,
    IEquatable<StreamId>
{
    public StreamId(IpDirection ipDirection, TransportDirection ports)
    {
        IpDirection = ipDirection;
        Ports = ports;
    }

    public readonly IpDirection IpDirection;
    public readonly TransportDirection Ports;

    public int CompareTo(StreamId rhs)
    {
        var c1 = IpDirection.CompareTo(rhs.IpDirection);
        if (c1 != 0) return c1;
        return Ports.CompareTo(rhs.Ports);
    }

    public bool Equals(StreamId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return IpDirection.Equals(other.IpDirection) && Ports.Equals(other.Ports);
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
        return HashCode.Combine(IpDirection, Ports);
    }
}