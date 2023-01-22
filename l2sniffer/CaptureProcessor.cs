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
    private ulong _skipPackets = 0;
    private ulong _maxProcess = ulong.MaxValue;
    private ulong _packetsProcessed = 0;

    public CaptureProcessor(IPacketHandlerProvider packetHandlerProvider)
    {
        _packetHandlerProvider = packetHandlerProvider;
    }

    public void ProcessCapture(ICaptureDevice device, ulong skipPacket = 0, ulong maxProcess = ulong.MaxValue)
    {
        _skipPackets = skipPacket;
        _maxProcess = maxProcess;
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
                return (sender, capture) => HandleCapture(capture, _ethernetHandler);
            case LinkLayers.IPv4:
                _ipv4Handler = _packetHandlerProvider.GetPacketHandler<IPv4Packet>();
                return (sender, capture) => HandleCapture(capture, _ipv4Handler);
            case LinkLayers.IPv6:
                _ipv6Handler = _packetHandlerProvider.GetPacketHandler<IPv6Packet>();
                return (sender, capture) => HandleCapture(capture, _ipv6Handler);
            default:
                throw new NotImplementedException($"link layer {deviceLinkType} not yet implemented");
        }
    }

    private void HandleCapture<T>(PacketCapture e, IPacketHandler<T> handler)
    {
        try
        {
            if (_packetsProcessed < _skipPackets) return;
            if (_packetsProcessed > _skipPackets + _maxProcess) return;
            var bytes = new ByteArraySegment(e.Data.ToArray());
            var packet = (T)Activator.CreateInstance(typeof(T), bytes);
            handler.HandlePacket(packet, GetBaseMetainfo(e));
        }
        finally
        {
            _packetsProcessed++;
        }
    }

    private PacketMetainfo GetBaseMetainfo(PacketCapture packetCapture)
    {
        return new PacketMetainfo
        {
            CaptureTime = packetCapture.Header.Timeval
        };
    }
}