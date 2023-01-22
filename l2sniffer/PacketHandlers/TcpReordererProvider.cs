using l2sniffer.PacketHandlers;
using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public class TcpReordererProvider : ITcpAssemblerProvider
{
    public IPacketHandler<TcpPacket> GetTcpAssembler(StreamId stream, IAssembledTcpHandler tcpHandler)
    {
        return new TcpReorderer(tcpHandler);
    }
}