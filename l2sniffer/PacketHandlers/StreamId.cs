using System.Net;
using L2sniffer;

namespace l2sniffer.PacketHandlers;

public class StreamId : IComparable<StreamId>,
    IEquatable<StreamId>
{
    public ref struct SourceHolder
    {
        public SourceHolder(IPAddress srcIp, ushort srcPort)
        {
            _srcIp = srcIp;
            _srcPort = srcPort;
        }

        private IPAddress _srcIp;
        private ushort _srcPort;

        public StreamId To(IPAddress dstIp, ushort dstPort)
        {
            return new StreamId(new IpDirection(_srcIp, dstIp),
                                new TransportDirection(_srcPort, dstPort));
        }

        public StreamId To(string dstIp, ushort dstPort)
        {
            return To(IPAddress.Parse(dstIp), dstPort);
        }
    }

    public static SourceHolder From(string srcIp, ushort srcPort)
    {
        return From(IPAddress.Parse(srcIp), srcPort);
    }

    public static SourceHolder From(IPAddress srcIp, ushort srcPort)
    {
        return new SourceHolder(srcIp, srcPort);
    }

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