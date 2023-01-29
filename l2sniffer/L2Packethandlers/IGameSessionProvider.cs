using System.Net;
using L2sniffer.GameState;

namespace L2sniffer.L2PacketHandlers;

public interface IGameSessionProvider
{
    GameSession GetGameSession(IPEndPoint serverEndpoint, IPEndPoint clientEndpoint);
}