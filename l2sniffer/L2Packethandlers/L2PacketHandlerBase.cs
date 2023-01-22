using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.StreamHandlers;

namespace L2sniffer.L2PacketHandlers;

public abstract class L2PacketHandlerBase<T> : IDatagramStreamHandler
    where T : L2PacketBase
{
    protected StreamId _streamId;
    protected IL2PacketLogger _packetLogger;
    private ulong _packetsCounter = 0;
    private IL2PacketDecryptor _packetDecryptor;
    private IPacketDecryptorProvider _packetDecryptorProvider;

    protected L2PacketHandlerBase(IL2PacketLogger packetLogger,
                                  IPacketDecryptorProvider packetDecryptorProvider,
                                  StreamId streamId)
    {
        _packetLogger = packetLogger;
        _packetDecryptorProvider = packetDecryptorProvider;
        _streamId = streamId;
    }

    public void HandleDatagram(byte[] datagram, PacketMetainfo metainfo)
    {
        try
        {
            var packet = DecryptDatagram(datagram, metainfo);
            _packetLogger.LogPacket(packet, metainfo);
            ProcessPacket(packet);
        }
        finally
        {
            _packetsCounter++;
        }
    }

    public void HandleMissingInterval(uint loweBound, uint upperBound)
    {
        throw new Exception("critical error-data segment missing,unable to recover");
    }

    private T DecryptDatagram(byte[] datagram, PacketMetainfo metainfo)
    {
        if (_packetDecryptor != null)
        {
            return _packetDecryptor.DecryptPacket<T>(datagram);
        }

        if (_packetsCounter == 0)
        {
            return (T)Activator.CreateInstance(typeof(T), datagram);
        }

        var streamId = new StreamId(metainfo.TopLevelIpDirection, metainfo.TransportPorts);
        _packetDecryptor = GetDecryptor(_packetDecryptorProvider, streamId);
        return _packetDecryptor.DecryptPacket<T>(datagram);
    }

    protected abstract void ProcessPacket(T packet);

    protected abstract IL2PacketDecryptor GetDecryptor(IPacketDecryptorProvider decryptorProvider,
                                                       StreamId streamId);
}