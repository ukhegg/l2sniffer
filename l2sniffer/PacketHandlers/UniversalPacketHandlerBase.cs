using L2sniffer;
using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public abstract class UniversalPacketHandlerBase<TPacket, TPayloadTypeEnum> : IPacketHandler<TPacket>
    where TPayloadTypeEnum : Enum, IConvertible
    where TPacket : Packet
{
    private readonly IPacketHandlerProvider _packetHandlerProvider;
    private readonly object[] _handlers;

    public UniversalPacketHandlerBase(IPacketHandlerProvider packetHandlerProvider)
    {
        _packetHandlerProvider = packetHandlerProvider;
        var values = Enum.GetValues(typeof(TPayloadTypeEnum));
        _handlers = new object[Enum.GetValues(typeof(TPayloadTypeEnum)).Cast<TPayloadTypeEnum>().Max().ToInt32(null)];
    }

    public void HandlePacket(TPacket packet, PacketMetainfo metainfo)
    {
        UpdateMetainfo(packet, metainfo);
        ExtractPayloadPacketAndSendToHandler(packet.PayloadPacket, metainfo, GetPayloadType(packet));
    }

    protected abstract TPayloadTypeEnum GetPayloadType(TPacket packet);

    protected abstract void UpdateMetainfo(TPacket packet, PacketMetainfo packetMetainfo);

    protected abstract void ExtractPayloadPacketAndSendToHandler(Packet payloadPacket,
        PacketMetainfo metainfo,
        TPayloadTypeEnum payloadPacketType);

    protected void SendPacketToHandlerAs<TPayloadPacket>(Packet payloadPacket,
        PacketMetainfo metainfo,
        TPayloadTypeEnum payloadPacketType)
        where TPayloadPacket : Packet
    {
        ref var handler = ref _handlers[payloadPacketType.ToInt32(null)];
        handler ??= _packetHandlerProvider.GetPacketHandler<TPayloadPacket>();
        ((IPacketHandler<TPayloadPacket>)handler).HandlePacket(payloadPacket.Extract<TPayloadPacket>(), metainfo);
    }
}