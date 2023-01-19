using System.Net;
using System.Net.NetworkInformation;
using SharpPcap;

namespace L2sniffer;

using IpDirection = KeyValuePair<IPAddress, IPAddress>;
using EthernetDirection = KeyValuePair<PhysicalAddress, PhysicalAddress>;

public class PacketMetainfo
{
    private List<IpDirection> _ipDirections;
    private EthernetDirection _ethernetDirection;
    public PosixTimeval CaptureTime { get; set; }

    public void AddIpDirection(IPAddress src, IPAddress dst)
    {
        _ipDirections ??= new List<IpDirection>();
        _ipDirections.Add(new IpDirection(src, dst));
    }

    public void SetEthernetDirection(PhysicalAddress src, PhysicalAddress dst)
    {
        _ethernetDirection = new EthernetDirection(src, dst);
    }
}