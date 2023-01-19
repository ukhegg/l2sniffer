using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;

namespace L2sniffer;

public class CaptureProcessor
{
    private IPacketHandlerProvider _packetHandlerProvider;
    private IPacketHandler<EthernetPacket> _ethernetHandler = null!;
    private IPacketHandler<IPv4Packet> _ipv4Handler = null!;
    private IPacketHandler<IPv6Packet> _ipv6Handler = null!;

    public CaptureProcessor(IPacketHandlerProvider packetHandlerProvider)
    {
        _packetHandlerProvider = packetHandlerProvider;
    }

    public void ProcessCapture(ICaptureDevice device)
    {
        var deviceEofEvent = new ManualResetEvent(false);
        device.OnCaptureStopped += (sender, status) => { deviceEofEvent.Set(); };
        device.OnPacketArrival += this.GetCapturePacketHandler(device.LinkType);
        device.StartCapture();
        deviceEofEvent.WaitOne();
    }

    private PacketArrivalEventHandler GetCapturePacketHandler(LinkLayers deviceLinkType)
    {
        switch (deviceLinkType)
        {
            case LinkLayers.Ethernet:
                _ethernetHandler = _packetHandlerProvider.GetPacketHandler<EthernetPacket>();
                return this.HandleEthernetRawCapture;
            case LinkLayers.IPv4:
                _ipv4Handler = _packetHandlerProvider.GetPacketHandler<IPv4Packet>();
                return this.HandlerIpV4RawCapture;
            case LinkLayers.IPv6:
                _ipv6Handler = _packetHandlerProvider.GetPacketHandler<IPv6Packet>();
                return this.HandlerIpV6RawCapture;
            default:
                throw new NotImplementedException($"link layer {deviceLinkType} not yet implemented");
        }
    }

    private void HandlerIpV6RawCapture(object sender, PacketCapture e)
    {
        var bytes = new ByteArraySegment(e.Data.ToArray());
        this._ipv6Handler.HandlePacket(new IPv6Packet(bytes), GetBaseMetainfo(e));
    }

    private void HandlerIpV4RawCapture(object sender, PacketCapture e)
    {
        var bytes = new ByteArraySegment(e.Data.ToArray());
        _ipv4Handler.HandlePacket(new IPv4Packet(bytes), GetBaseMetainfo(e));
    }

    private void HandleEthernetRawCapture(object sender, PacketCapture e)
    {
        var bytes = new ByteArraySegment(e.Data.ToArray());
        _ethernetHandler.HandlePacket(new EthernetPacket(bytes), GetBaseMetainfo(e));
    }

    private PacketMetainfo GetBaseMetainfo(PacketCapture packetCapture)
    {
        return new PacketMetainfo
        {
            CaptureTime = packetCapture.Header.Timeval
        };
    }
}