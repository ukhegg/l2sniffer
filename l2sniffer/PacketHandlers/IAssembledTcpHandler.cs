using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public interface IAssembledTcpHandler
{
    void HandleReordered(TcpPacket packet,
                         PacketMetainfo packetMetainfo);

    void HandlePartialOverlap(TcpPacket packet,
                              PacketMetainfo packetMetainfo,
                              uint overlapSize);

    void HandleIntervalMissing(uint loweBound, uint upperBound);

    void HandleOutOfIndexPacket(TcpPacket packet, PacketMetainfo packetMetainfo);
}