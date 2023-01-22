using System.Net;

namespace L2sniffer.L2PacketHandlers;

class L2ServersRegistry : IL2ServerRegistry
{
    private readonly ICollection<IPEndPoint> _loginServers;
    private readonly ICollection<IPEndPoint> _gameServers;

    public void RegisterLoginServer(IPEndPoint endpoint)
    {
        _loginServers.Add(endpoint);
    }

    public void RegisterGameServer(IPEndPoint endpoint)
    {
        _gameServers.Add(endpoint);
    }

    public bool IsLoginServer(IPEndPoint endpoint)
    {
        return _loginServers.Contains(endpoint);
    }

    public bool IsGameServer(IPEndPoint endpoint)
    {
        return _gameServers.Contains(endpoint);
    }
}