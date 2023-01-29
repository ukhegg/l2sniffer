using L2sniffer;
using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.StreamHandlers;

namespace BotAgent;

public class L2DecryptingDatagramHandler : IDatagramStreamHandler
{
    private readonly IDecryptorProvider _packetDecryptorProvider;
    private ISessionCryptKeysRegistry _cryptoKeysRegistry;
    private readonly StreamId _streamId;
    private readonly IPacketHandler<L2PacketBase> _l2PacketsHandler;
    private ulong _packetsCounter = 0;
    private IL2PacketDecryptor _packetDecryptor;
    private bool _streamIntegrityOk = true;

    public L2DecryptingDatagramHandler(IDecryptorProvider packetDecryptorProvider,
                                       StreamId streamId,
                                       IPacketHandler<L2PacketBase> l2PacketsHandler)
    {
        _packetDecryptorProvider = packetDecryptorProvider;
        _streamId = streamId;
        _l2PacketsHandler = l2PacketsHandler;
    }

    public void HandleDatagram(byte[] datagram, PacketMetainfo metainfo)
    {
        try
        {
            if (!_streamIntegrityOk) return;
            var decrypted = DecryptDatagram(datagram);
            _l2PacketsHandler.HandlePacket(decrypted, metainfo);
        }
        finally
        {
            _packetsCounter++;
        }
    }

    public void HandleMissingInterval(uint loweBound, uint upperBound)
    {
        _streamIntegrityOk = false;
    }

    private L2PacketBase DecryptDatagram(byte[] datagram)
    {
        if (_packetDecryptor != null)
        {
            return _packetDecryptor.DecryptPacket<L2PacketBase>(datagram);
        }

        if (_packetsCounter == 0)
        {
            return new L2PacketBase(datagram);
        }

        _packetDecryptor = _packetDecryptorProvider.GetDecryptor(_streamId);
        return _packetDecryptor.DecryptPacket<L2PacketBase>(datagram);
    }
}