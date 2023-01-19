using L2sniffer;

namespace l2sniffer.PacketHandlers;

public class TransportDirection : Direction<UInt16, TransportDirection>
{
    public TransportDirection(ushort source, ushort destination) : base(source, destination)
    {
    }

    protected override int Compare(ushort lhs, ushort rhs)
    {
        return lhs.CompareTo(rhs);
    }
}