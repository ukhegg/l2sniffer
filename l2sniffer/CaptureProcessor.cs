using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;

namespace L2sniffer;

public class CaptureProcessor : ICaptureProcessor
{
    interface IRawCaptureHandler
    {
        public void ProcessCapture(PacketCapture capture);
    }

    class RawCaptureHandler<T> : IRawCaptureHandler
        where T : Packet
    {
        private readonly IPacketHandler<T> _handler;
        private ulong _packetsProcessed = 0;
        private readonly ulong _startFrom;
        private readonly ulong _stopAfter;
        private ICaptureDevice _captureDevice;

        public RawCaptureHandler(ICaptureDevice captureDevice,
                                 IPacketHandler<T> handler,
                                 ulong startFrom, ulong maxProcess)
        {
            _handler = handler;
            _startFrom = startFrom;
            _captureDevice = captureDevice;
            _stopAfter = maxProcess == ulong.MaxValue ? ulong.MaxValue : startFrom + maxProcess;
        }

        public void ProcessCapture(PacketCapture capture)
        {
            try
            {
                if (_packetsProcessed < _startFrom) return;
                if (_packetsProcessed > _stopAfter) return;

                var realPacketsProcess = _packetsProcessed - _startFrom;
                if (realPacketsProcess > 0 && realPacketsProcess % 200000 == 0)
                {
                    Console.WriteLine($"Processed {realPacketsProcess} packets,press any key to continue or q to stop");
                    var k = Console.ReadKey();
                    if (k.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine("Stopping capture");
                        _captureDevice.StopCapture();
                        return;
                    }

                    Console.WriteLine("Continuing capture...");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }

                var bytes = new ByteArraySegment(capture.Data.ToArray());
                var packet = (T)Activator.CreateInstance(typeof(T), bytes);
                _handler.HandlePacket(packet, GetBaseMetainfo(capture));
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

    private IPacketHandlerProvider _packetHandlerProvider;

    public CaptureProcessor(IPacketHandlerProvider packetHandlerProvider)
    {
        _packetHandlerProvider = packetHandlerProvider;
    }

    public void ProcessCaptureSync(ICaptureDevice device, ulong skipPacket = 0, ulong maxProcess = ulong.MaxValue)
    {
        var deviceEofEvent = new ManualResetEvent(false);
        device.OnCaptureStopped += (sender, status) => { deviceEofEvent.Set(); };
        ProcessCaptureAsync(device, skipPacket, maxProcess);
        deviceEofEvent.WaitOne();
    }

    public void ProcessCaptureAsync(ICaptureDevice device, ulong skipPacket = 0, ulong maxProcess = ulong.MaxValue)
    {
        var handler = GetCaptureHandler(device, skipPacket, maxProcess);
        device.OnPacketArrival += (sender, capture) => { handler.ProcessCapture(capture); };
        device.StartCapture();
    }

    private IRawCaptureHandler GetCaptureHandler(ICaptureDevice captureDevice,
                                                 ulong skipPacket,
                                                 ulong maxProcess)
    {
        switch (captureDevice.LinkType)
        {
            case LinkLayers.Ethernet:
                var ethernetHandler = _packetHandlerProvider.GetPacketHandler<EthernetPacket>();
                return new RawCaptureHandler<EthernetPacket>(captureDevice, ethernetHandler, skipPacket, maxProcess);
            case LinkLayers.IPv4:
                var ipv4Handler = _packetHandlerProvider.GetPacketHandler<IPv4Packet>();
                return new RawCaptureHandler<IPv4Packet>(captureDevice, ipv4Handler, skipPacket, maxProcess);
            case LinkLayers.IPv6:
                var ipv6Handler = _packetHandlerProvider.GetPacketHandler<IPv6Packet>();
                return new RawCaptureHandler<IPv6Packet>(captureDevice, ipv6Handler, skipPacket, maxProcess);
            default:
                throw new NotImplementedException($"link layer {captureDevice.LinkType} not yet implemented");
        }
    }
}