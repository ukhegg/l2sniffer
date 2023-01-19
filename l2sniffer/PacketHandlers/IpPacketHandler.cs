using PacketDotNet;

namespace L2sniffer.PacketHandlers;

public class IpPacketHandler : UniversalPacketHandlerBase<IPPacket, ProtocolType>
{
    public IpPacketHandler(IPacketHandlerProvider packetHandlerProvider) : base(packetHandlerProvider)
    {
    }

    protected override void UpdateMetainfo(IPPacket packet, PacketMetainfo packetMetainfo)
    {
        packetMetainfo.AddIpDirection(packet.SourceAddress, packet.DestinationAddress);
    }

    protected override ProtocolType GetPayloadType(IPPacket packet)
    {
        return packet.Protocol;
    }

    protected override void ExtractPayloadPacketAndSendToHandler(Packet payloadPacket,
        PacketMetainfo metainfo,
        ProtocolType payloadPacketType)
    {
        switch (payloadPacketType)
        {
            case ProtocolType.IPv4:
                SendPacketToHandlerAs<IPv4Packet>(payloadPacket, metainfo, payloadPacketType);
                break;
            case ProtocolType.IPv6:
                SendPacketToHandlerAs<IPv6Packet>(payloadPacket, metainfo, payloadPacketType);
                break;
            case ProtocolType.Tcp:
                SendPacketToHandlerAs<TcpPacket>(payloadPacket, metainfo, payloadPacketType);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}