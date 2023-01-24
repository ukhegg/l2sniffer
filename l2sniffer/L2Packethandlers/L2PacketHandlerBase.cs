using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.StreamHandlers;

namespace L2sniffer.L2PacketHandlers;

public abstract class L2PacketHandlerBase<T, TTypeEnum> : IDatagramStreamHandler
    where T : TypeL2PacketBase<TTypeEnum> where TTypeEnum : Enum
{
    public interface IHandlersRegistry
    {
        void RegisterHandler<TPacketSubtype>(TTypeEnum packetType, Action<TPacketSubtype, PacketMetainfo> handler)
            where TPacketSubtype : T;
    }

    class HandlersRegistry : IHandlersRegistry
    {
        public readonly IDictionary<TTypeEnum, Action<T, PacketMetainfo>> PacketHandlers;

        public HandlersRegistry()
        {
            PacketHandlers = new Dictionary<TTypeEnum, Action<T, PacketMetainfo>>();
        }

        public void RegisterHandler<TPacketSubtype>(TTypeEnum packetType,
                                                    Action<TPacketSubtype, PacketMetainfo> handler)
            where TPacketSubtype : T
        {
            PacketHandlers[packetType] = (T packet, PacketMetainfo metainfo) =>
            {
                handler.Invoke(packet.As<TPacketSubtype>(), metainfo);
            };
        }
    }


    protected StreamId _streamId;
    protected IL2PacketLogger _packetLogger;
    private ulong _packetsCounter = 0;
    private IL2PacketDecryptor _packetDecryptor;
    private IPacketDecryptorProvider _packetDecryptorProvider;
    private IDictionary<TTypeEnum, Action<T, PacketMetainfo>> _packetHandlers;

    protected L2PacketHandlerBase(IL2PacketLogger packetLogger,
                                  IPacketDecryptorProvider packetDecryptorProvider,
                                  StreamId streamId)
    {
        _packetLogger = packetLogger;
        _packetDecryptorProvider = packetDecryptorProvider;
        _streamId = streamId;

        var handlerRegistry = new HandlersRegistry();
        RegisterHandlers(handlerRegistry);
        _packetHandlers = handlerRegistry.PacketHandlers;
    }

    public void HandleDatagram(byte[] datagram, PacketMetainfo metainfo)
    {
        try
        {
            ProcessPacket(DecryptDatagram(datagram, metainfo), metainfo);
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
        _packetDecryptor = SelectDecryptor(_packetDecryptorProvider, streamId);
        return _packetDecryptor.DecryptPacket<T>(datagram);
    }

    protected void ProcessPacket(T packet, PacketMetainfo metainfo)
    {
        
        if (_packetHandlers.TryGetValue(packet.PacketType, out var handler))
        {
            _packetLogger.LogHandledPacket(packet, metainfo);
            handler.Invoke(packet, metainfo);
            
        }
        else
        {
            _packetLogger.LogUnhandledPacket(packet, metainfo);
        }
    }

    protected abstract IL2PacketDecryptor SelectDecryptor(IPacketDecryptorProvider decryptorProvider,
                                                       StreamId streamId);

    protected void RegisterPacketHandler(TTypeEnum packetType, Action<T, PacketMetainfo> handler)
    {
        _packetHandlers[packetType] = handler;
    }

    protected virtual void RegisterHandlers(IHandlersRegistry handlersRegistry)
    {
    }
}