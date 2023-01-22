using System.Collections.ObjectModel;
using System.Net;

namespace L2sniffer.L2PacketHandlers;

public class L2ServerRegistry : IL2ServerRegistry
{
    private readonly Collection<IPEndPoint> _loginServers = new();
    private readonly Collection<IPEndPoint> _gameServers = new();

    public void RegisterLoginServer(IPEndPoint loginServerEndpoint)
    {
        _loginServers.Add(loginServerEndpoint);
    }

    public void RegisterGameServer(IPEndPoint gameServerEndpoint)
    {
        _gameServers.Add(gameServerEndpoint);
    }

    public bool IsGameServer(IPEndPoint endpoint)
    {
        return _gameServers.Contains(endpoint);
    }

    public bool IsLoginServer(IPEndPoint endpoint)
    {
        return _loginServers.Contains(endpoint);
    }
}