using System.Net;
using L2sniffer.Crypto;
using L2sniffer.GameData;
using L2sniffer.GameState;
using L2sniffer.GameState.GameObjects;
using l2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;

namespace L2sniffer.L2PacketHandlers;

public class L2DatagramHandlerProvider : IDatagramStreamHandlerProvider
{
    private readonly IPacketDecryptorProvider _packetDecryptorProvider;
    private readonly ISessionCryptKeysRegistry _cryptoKeysRegistry;
    private readonly IL2ServerRegistry _serverRegistry;
    private IL2PacketLogger _packetLogger;
    private IGameObjectsProvider _gameObjectsProvider;
    private IGameObjectsRegistry _gameObjectRegistry;
    private ISkillInfoProvider _skillRegistry;
    private IGameSessionProvider _gameSessionsProvider;

    public L2DatagramHandlerProvider(IPacketDecryptorProvider packetDecryptorProvider,
                                     ISessionCryptKeysRegistry cryptoKeysRegistry,
                                     IL2ServerRegistry serverRegistry,
                                     IL2PacketLogger packetLogger,
                                     IGameObjectsProvider gameObjectsProvider,
                                     IGameObjectsRegistry gameObjectRegistry,
                                     ISkillInfoProvider skillRegistry,
                                     IGameSessionProvider gameSessionsProvider)
    {
        _packetDecryptorProvider = packetDecryptorProvider;
        _packetLogger = packetLogger;
        _gameObjectsProvider = gameObjectsProvider;
        _gameObjectRegistry = gameObjectRegistry;
        _skillRegistry = skillRegistry;
        _gameSessionsProvider = gameSessionsProvider;
        _serverRegistry = serverRegistry;
        _cryptoKeysRegistry = cryptoKeysRegistry;
    }

    public IDatagramStreamHandler GetDatagramHandler(StreamId streamId)
    {
        var srcEndpoint = streamId.SrcEndpoint;
        var dstEndpoint = streamId.DstEndpoint;
        if (_serverRegistry.IsLoginServer(srcEndpoint))
        {
            return new LoginServerPacketHandler(streamId, _packetDecryptorProvider, _serverRegistry, _packetLogger);
        }

        if (_serverRegistry.IsGameServer(srcEndpoint))
        {
            var session = _gameSessionsProvider.GetGameSession(srcEndpoint, dstEndpoint);
            return new GameServerPacketHandler(streamId,
                                               _cryptoKeysRegistry,
                                               _packetDecryptorProvider,
                                               _packetLogger,
                                               _gameObjectsProvider,
                                               _gameObjectRegistry,
                                               _skillRegistry,
                                               session);
        }


        if (_serverRegistry.IsLoginServer(dstEndpoint))
        {
            return new LoginClientPacketHandler(streamId, _packetDecryptorProvider, _packetLogger);
        }

        if (_serverRegistry.IsGameServer(dstEndpoint))
        {
            var session = _gameSessionsProvider.GetGameSession(dstEndpoint, srcEndpoint);
            return new GameClientPacketHandler(streamId, _packetDecryptorProvider, _packetLogger, session);
        }

        throw new Exception("unknown stream");
    }

    public IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports)
    {
        return GetDatagramHandler(new StreamId(ipDirection, ports));
    }
}