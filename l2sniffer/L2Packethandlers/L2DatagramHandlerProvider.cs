using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;

namespace L2sniffer.L2PacketHandlers;

public class L2DatagramHandlerProvider : IDatagramStreamHandlerProvider
{
    private readonly IPacketDecryptorProvider _packetDecryptorProvider;
    private readonly ISessionCryptKeysRegistry _cryptoKeysRegistry;
    private readonly IL2ServerRegistry _serverRegistry;
    private IL2PacketLogger _packetLogger;

    public L2DatagramHandlerProvider(IPacketDecryptorProvider packetDecryptorProvider,
                                     ISessionCryptKeysRegistry cryptoKeysRegistry,
                                     IL2ServerRegistry serverRegistry,
                                     IL2PacketLogger packetLogger)
    {
        _packetDecryptorProvider = packetDecryptorProvider;
        _packetLogger = packetLogger;
        _serverRegistry = serverRegistry;
        _cryptoKeysRegistry = cryptoKeysRegistry;
    }

    public IDatagramStreamHandler GetDatagramHandler(StreamId streamId)
    {
        var srcEndpoint = streamId.SrcEndpoint;
        if (_serverRegistry.IsLoginServer(srcEndpoint))
        {
            return new LoginServerPacketHandler(streamId, _packetDecryptorProvider, _serverRegistry, _packetLogger);
        }

        if (_serverRegistry.IsGameServer(srcEndpoint))
        {
            return new GameServerPacketHandler(streamId,
                                               _cryptoKeysRegistry,
                                               _packetDecryptorProvider,
                                               _packetLogger);
        }

        var dstEndpoint = streamId.DstEndpoint;
        if (_serverRegistry.IsLoginServer(dstEndpoint))
        {
            return new LoginClientPacketHandler(streamId, _packetDecryptorProvider, _packetLogger);
        }

        if (_serverRegistry.IsGameServer(dstEndpoint))
        {
            return new GameClientPacketHandler(streamId, _packetDecryptorProvider, _packetLogger);
        }

        throw new Exception("unknown stream");
    }

    public IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports)
    {
        return GetDatagramHandler(new StreamId(ipDirection, ports));
    }
}