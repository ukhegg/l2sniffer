using System.Net;
using L2sniffer.Packets;
using SharpPcap;

namespace BotAgent;

public interface IL2SessionPacketAsyncProvider
{
    public interface IPacketConsumer
    {
        Task ConsumeAsync(L2PacketBase packet, PosixTimeval captureTime,
                          CancellationToken cst);
    }

    Task GetSessionPacketsAsync(IPEndPoint serverEndpoint,
                                IPEndPoint clientEndpoint,
                                IPacketConsumer consumer,
                                CancellationToken cst);
}