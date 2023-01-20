namespace L2sniffer;

public interface IPacketHandlerRegistry
{
    void RegisterPacketHandler<TPacket>(Func<IPacketHandler<TPacket>> getter);
    void RegisterPacketHandler<TPacket>(IPacketHandler<TPacket> instance);
}