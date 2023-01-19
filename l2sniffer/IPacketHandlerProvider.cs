namespace L2sniffer;

public interface IPacketHandlerProvider
{
    IPacketHandler<TPacket> GetPacketHandler<TPacket>();
}