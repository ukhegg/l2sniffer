using PacketDotNet;

namespace L2sniffer;

public class PacketHandlerProvider : IPacketHandlerProvider,
    IPacketHandlerRegistry
{
    public string Uuid = Guid.NewGuid().ToString();

    interface IGetterBase
    {
    }

    interface IGetter<TPacket> : IGetterBase
    {
        IPacketHandler<TPacket> GetHandler();
    }

    class FuncGetter<TPacket> : IGetter<TPacket>
    {
        private Func<IPacketHandler<TPacket>> _getter;

        public FuncGetter(Func<IPacketHandler<TPacket>> getter)
        {
            _getter = getter;
        }

        public IPacketHandler<TPacket> GetHandler()
        {
            return this._getter();
        }
    }

    class InstanceGetter<TPacket> : IGetter<TPacket>
    {
        private IPacketHandler<TPacket> _instance;

        public InstanceGetter(IPacketHandler<TPacket> instance)
        {
            _instance = instance;
        }

        public IPacketHandler<TPacket> GetHandler()
        {
            return _instance;
        }
    }


    private readonly Dictionary<Type, IGetterBase> _handlerProviders;

    public PacketHandlerProvider()
    {
        _handlerProviders = new Dictionary<Type, IGetterBase>();
    }

    public void RegisterPacketHandler<TPacket>(Func<IPacketHandler<TPacket>> getter)
    {
        _handlerProviders[typeof(TPacket)] = new FuncGetter<TPacket>(getter);
    }

    public void RegisterPacketHandler<TPacket>(IPacketHandler<TPacket> instance)
    {
        _handlerProviders[typeof(TPacket)] = new InstanceGetter<TPacket>(instance);
    }

    public IPacketHandler<TPacket> GetPacketHandler<TPacket>()
    {
        return ((IGetter<TPacket>)_handlerProviders[typeof(TPacket)]).GetHandler();
    }
}