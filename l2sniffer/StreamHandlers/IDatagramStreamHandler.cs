namespace L2sniffer.StreamHandlers;

public interface IDatagramStreamHandler
{
    void HandleDatagram(byte[] datagram, PacketMetainfo metainfo);

    void HandleMissingInterval(uint loweBound, uint upperBound);
}