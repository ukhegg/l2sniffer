using System.Net;
using System.Net.Sockets;

namespace L2sniffer;

public class IpDirection : Direction<IPAddress, IpDirection>
{
    public IpDirection(IPAddress source, IPAddress destination) : base(source, destination)
    {
    }

    public IpDirection(string source, string destination)
        : base(IPAddress.Parse(source), IPAddress.Parse(destination))
    {
    }


    protected override int Compare(IPAddress lhs, IPAddress rhs)
    {
        if (lhs.AddressFamily != rhs.AddressFamily)
        {
            return lhs.AddressFamily.CompareTo(rhs.AddressFamily);
        }

        return ByteArrayCompare.StaticCompare(lhs.GetAddressBytes(), rhs.GetAddressBytes());
    }
}