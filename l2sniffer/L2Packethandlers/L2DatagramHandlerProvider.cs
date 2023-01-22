using System.Collections.ObjectModel;
using System.Net;
using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;

namespace L2sniffer.L2PacketHandlers;

public class L2DatagramHandlerProvider : IDatagramStreamHandlerProvider,
    IL2ServerRegistry
{
    private readonly Collection<IPEndPoint> _loginServers = new();
    private readonly Collection<IPEndPoint> _gameServers = new();

    private readonly IPacketDecryptorProvider _packetDecryptorProvider;
    private readonly ISessionCryptKeysRegistry _cryptoKeysRegistry;
    private IL2PacketLogger _packetLogger;

    public L2DatagramHandlerProvider(IPacketDecryptorProvider packetDecryptorProvider,
                                     ISessionCryptKeysRegistry cryptoKeysRegistry, 
                                     IL2PacketLogger packetLogger)
    {
        _packetDecryptorProvider = packetDecryptorProvider;
        _packetLogger = packetLogger;
        _cryptoKeysRegistry = cryptoKeysRegistry;
    }

    public IDatagramStreamHandler GetDatagramHandler(StreamId streamId)
    {
        var srcEndpoint = streamId.SrcEndpoint;
        if (_loginServers.Contains(srcEndpoint))
        {
            return new LoginServerPacketHandler(streamId, _packetDecryptorProvider, this, _packetLogger);
        }

        if (_gameServers.Contains(srcEndpoint))
        {
            return new GameServerPacketHandler(streamId, 
                                               _cryptoKeysRegistry,
                                               _packetDecryptorProvider,
                                               _packetLogger);
        }

        var dstEndpoint = streamId.DstEndpoint;
        if (_loginServers.Contains(dstEndpoint))
        {
            return new LoginClientPacketHandler(streamId, _packetDecryptorProvider, _packetLogger);
        }

        if (_gameServers.Contains(dstEndpoint))
        {
            return new GameClientPacketHandler(streamId, _packetDecryptorProvider, _packetLogger);
        }

        throw new Exception("unknown stream");
    }

    public IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports)
    {
        return GetDatagramHandler(new StreamId(ipDirection, ports));
    }

    public void RegisterLoginServer(IPEndPoint endpoint)
    {
        _loginServers.Add(endpoint);
    }

    public void RegisterGameServer(IPEndPoint endpoint)
    {
        _gameServers.Add(endpoint);
    }
}