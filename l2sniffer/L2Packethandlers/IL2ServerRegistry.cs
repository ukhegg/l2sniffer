using System.Net;

namespace L2sniffer.L2PacketHandlers;

public interface IL2ServerRegistry
{
    void RegisterLoginServer(IPEndPoint endpoint);

    void RegisterGameServer(IPEndPoint endpoint);

    bool IsLoginServer(IPEndPoint endpoint);

    bool IsGameServer(IPEndPoint endpoint);
}