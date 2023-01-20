using System.Net;
using System.Net.NetworkInformation;
using l2sniffer.PacketHandlers;
using SharpPcap;

namespace L2sniffer;

public class PacketMetainfo
{
    private List<IpDirection>? _ipDirections;

    public PosixTimeval? CaptureTime { get; set; }

    public EthernetDirection? EthernetDirection { get; set; }

    public IpDirection? TopLevelIpDirection => _ipDirections?.Last();

    public TransportDirection? TransportPorts { get; set; }


    public void AddIpDirection(IPAddress src, IPAddress dst)
    {
        _ipDirections ??= new List<IpDirection>();
        _ipDirections.Add(new IpDirection(src, dst));
    }
}