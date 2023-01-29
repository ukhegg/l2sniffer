using System.Net;
using L2sniffer;

namespace BotAgent;

public interface INetworkSniffer
{
    bool IsRunning { get; }
    
    string Start(IPAddress l2ServerAddress, ICaptureProcessor captureProcessor);

    void Stop();
}