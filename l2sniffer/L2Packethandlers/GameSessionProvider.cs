using System.Net;
using L2sniffer.GameData;
using L2sniffer.GameState;
using L2sniffer.GameState.GameObjects;

namespace L2sniffer.L2PacketHandlers;

public class GameSessionProvider : IGameSessionProvider
{
    private IItemNameProvider _itemNameProvider;
    private IActionNameProvider _actionNameProvider;
    private INpcNameProvider _npcNameProvider;
    private ISkillInfoProvider _skillInfoProvider;

    private IDictionary<IPEndPoint, GameSession> _clientGameSessions;

    public GameSessionProvider(IItemNameProvider itemNameProvider, IActionNameProvider actionNameProvider,
                               INpcNameProvider npcNameProvider, ISkillInfoProvider skillInfoProvider)
    {
        _itemNameProvider = itemNameProvider;
        _actionNameProvider = actionNameProvider;
        _npcNameProvider = npcNameProvider;
        _skillInfoProvider = skillInfoProvider;
        _clientGameSessions = new Dictionary<IPEndPoint, GameSession>();
    }

    public GameSession GetGameSession(IPEndPoint serverEndpoint, IPEndPoint clientEndpoint)
    {
        if (_clientGameSessions.TryGetValue(clientEndpoint, out var session))
        {
            return session;
        }

        var gameObjectsRegistry = new GameObjectRegistry();
        var gameObjectProvider = new GameObjectsProvider(_itemNameProvider,
                                                         _actionNameProvider,
                                                         _npcNameProvider,
                                                         gameObjectsRegistry,
                                                         _skillInfoProvider);
        session = new GameSession(serverEndpoint,
                                  clientEndpoint,
                                  gameObjectProvider,
                                  gameObjectsRegistry,
                                  _skillInfoProvider);
        _clientGameSessions[clientEndpoint] = session;
        return session;
    }
}