using L2sniffer;
using L2sniffer.Crypto;
using L2sniffer.L2PacketHandlers;
using l2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;

namespace BotAgent;

public class L2DecryptingDatagramHandlerProvider : IDatagramStreamHandlerProvider
{
    private readonly IPacketDecryptorProvider _packetDecryptorProvider;
    private readonly ISessionCryptKeysRegistry _cryptoKeysRegistry;
    private readonly IL2ServerRegistry _serverRegistry;
    private readonly IL2StreamsHandlerProvider _decryptedPacketsHandler;

    public L2DecryptingDatagramHandlerProvider(IPacketDecryptorProvider packetDecryptorProvider,
                                               ISessionCryptKeysRegistry cryptoKeysRegistry,
                                               IL2ServerRegistry serverRegistry,
                                               IL2StreamsHandlerProvider decryptedPacketsHandler)
    {
        _packetDecryptorProvider = packetDecryptorProvider;
        _cryptoKeysRegistry = cryptoKeysRegistry;
        _serverRegistry = serverRegistry;
        _decryptedPacketsHandler = decryptedPacketsHandler;
    }

    public IDatagramStreamHandler GetDatagramHandler(StreamId streamId)
    {
        var srcEndpoint = streamId.SrcEndpoint;
        var dstEndpoint = streamId.DstEndpoint;
        if (_serverRegistry.IsLoginServer(srcEndpoint))
        {
            var nextHandler = _decryptedPacketsHandler.GetHandler(L2SessionTypes.Login,
                                                                  ConversationDirections.ServerToClient,
                                                                  srcEndpoint,
                                                                  dstEndpoint);
            var proxyHandler = new LoginServerPacketHandler(_serverRegistry, nextHandler);
            return new L2DecryptingDatagramHandler(new LoginSessionDecryptorProvider(_packetDecryptorProvider),
                                                   streamId,
                                                   proxyHandler);
        }

        if (_serverRegistry.IsGameServer(srcEndpoint))
        {
            var nextHandler = _decryptedPacketsHandler.GetHandler(L2SessionTypes.Game,
                                                                  ConversationDirections.ServerToClient,
                                                                  srcEndpoint,
                                                                  dstEndpoint);
            var proxyHandler = new GameServerPacketsHandler(_cryptoKeysRegistry, nextHandler);
            return new L2DecryptingDatagramHandler(new GameSessionDecryptorProvider(_packetDecryptorProvider),
                                                   streamId,
                                                   proxyHandler);
        }

        if (_serverRegistry.IsLoginServer(dstEndpoint))
        {
            var nextHandler = _decryptedPacketsHandler.GetHandler(L2SessionTypes.Login,
                                                                  ConversationDirections.ClientToServer,
                                                                  dstEndpoint,
                                                                  srcEndpoint);
            return new L2DecryptingDatagramHandler(new LoginSessionDecryptorProvider(_packetDecryptorProvider),
                                                   streamId,
                                                   nextHandler);
        }

        if (_serverRegistry.IsGameServer(dstEndpoint))
        {
            var nextHandler = _decryptedPacketsHandler.GetHandler(L2SessionTypes.Game,
                                                                  ConversationDirections.ClientToServer,
                                                                  dstEndpoint,
                                                                  srcEndpoint);
            return new L2DecryptingDatagramHandler(new GameSessionDecryptorProvider(_packetDecryptorProvider),
                                                   streamId,
                                                   nextHandler);
        }

        throw new Exception("unknown stream");
    }

    public IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports)
    {
        return GetDatagramHandler(new StreamId(ipDirection, ports));
    }
}