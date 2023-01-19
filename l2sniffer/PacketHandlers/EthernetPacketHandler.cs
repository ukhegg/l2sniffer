using L2sniffer.PacketHandlers;
using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public class EthernetPacketHandler : UniversalPacketHandlerBase<EthernetPacket, EthernetType>
{
    public EthernetPacketHandler(IPacketHandlerProvider packetHandlerProvider)
        : base(packetHandlerProvider)
    {
    }

    protected override void UpdateMetainfo(EthernetPacket packet, PacketMetainfo packetMetainfo)
    {
        packetMetainfo.EthernetDirection = new EthernetDirection(packet.SourceHardwareAddress,
                                                                 packet.DestinationHardwareAddress);
    }

    protected override EthernetType GetPayloadType(EthernetPacket packet)
    {
        return packet.Type;
    }

    protected override void ExtractPayloadPacketAndSendToHandler(Packet payloadPacket,
        PacketMetainfo metainfo,
        EthernetType payloadPacketType)
    {
        switch (payloadPacketType)
        {
            case EthernetType.IPv4:
                SendPacketToHandlerAs<IPv4Packet>(payloadPacket, metainfo, payloadPacketType);
                break;
            case EthernetType.IPv6:
                SendPacketToHandlerAs<IPv6Packet>(payloadPacket, metainfo, payloadPacketType);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}