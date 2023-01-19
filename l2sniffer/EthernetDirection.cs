using System.Net.NetworkInformation;

namespace L2sniffer;

public class EthernetDirection : Direction<PhysicalAddress, EthernetDirection>
{
    public EthernetDirection(PhysicalAddress source, PhysicalAddress destination) : base(source, destination)
    {
    }

    protected override int Compare(PhysicalAddress lhs, PhysicalAddress rhs)
    {
        return ByteArrayCompare.StaticCompare(lhs.GetAddressBytes(), rhs.GetAddressBytes());
    }
}