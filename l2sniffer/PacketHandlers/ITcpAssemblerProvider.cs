using l2sniffer.PacketHandlers;
using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public interface ITcpAssemblerProvider
{
    IPacketHandler<TcpPacket> GetTcpAssembler(StreamId stream, IAssembledTcpHandler tcpHandler);
}