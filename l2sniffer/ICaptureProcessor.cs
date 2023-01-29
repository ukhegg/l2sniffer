using SharpPcap;

namespace L2sniffer;

public interface ICaptureProcessor
{
    void ProcessCaptureSync(ICaptureDevice device, ulong skipPacket = 0, ulong maxProcess = ulong.MaxValue);
    
    void ProcessCaptureAsync(ICaptureDevice device, ulong skipPacket = 0, ulong maxProcess = ulong.MaxValue);
}