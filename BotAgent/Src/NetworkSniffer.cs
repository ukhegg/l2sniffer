using System.Net;
using System.Net.NetworkInformation;
using L2sniffer;
using SharpPcap;
using SharpPcap.LibPcap;

namespace BotAgent;

public class NetworkSniffer : INetworkSniffer
{
    private ICaptureDevice? _captureDevice;

    public NetworkSniffer()
    {
    }

    public bool IsRunning => _captureDevice != null;

    public string Start(IPAddress l2ServerAddress, ICaptureProcessor captureProcessor)
    {
        ICaptureDevice device;

        try
        {
            // Get an offline device
            //device = new CaptureFileReaderDevice(capFile);
            var interfaces = PcapInterface.GetAllPcapInterfaces();
            var targetInterface = interfaces.FirstOrDefault(@interface => ServerIsAccessibleUsingInterface(
                                                                l2ServerAddress, @interface));
            device = new LibPcapLiveDevice(targetInterface);
            // Open the device
            device.Open();
            device.Filter = $"tcp and host {l2ServerAddress}";
            captureProcessor.ProcessCaptureAsync(device);
            _captureDevice = device;
            return targetInterface.FriendlyName;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to launch network capture:{e.Message}");
            throw new Exception("Failed to run network capture", e);
        }
    }

    public void Stop()
    {
        if (!IsRunning) return;
        var deviceEofEvent = new ManualResetEvent(false);
        _captureDevice.OnCaptureStopped += (sender, status) => { deviceEofEvent.Set(); };
        _captureDevice.StopCapture();
        deviceEofEvent.WaitOne();
    }

    private bool ServerIsAccessibleUsingInterface(IPAddress l2ServerAddress, PcapInterface @interface)
    {
        return @interface.Addresses
            .Select(address => PingService.Send(address.Addr.ipAddress, l2ServerAddress, timeout: 200))
            .Any(pingReply => pingReply.Status == IPStatus.Success);
    }
}