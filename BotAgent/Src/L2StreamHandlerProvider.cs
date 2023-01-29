using System.Net;
using L2sniffer;
using L2sniffer.Packets;

namespace BotAgent;

public class DummyHandler : IPacketHandler<L2PacketBase>
{
    public void HandlePacket(L2PacketBase packet, PacketMetainfo metainfo)
    {
    }
}

public class L2StreamHandlerProvider : IL2StreamsHandlerProvider,
    INotifyL2SessionFound
{
    public event L2SessionFoundEventHandler? OnL2SessionFound;

    public IPacketHandler<L2PacketBase> GetHandler(L2SessionTypes sessionType,
                                                   ConversationDirections direction,
                                                   IPEndPoint serverEndpoint,
                                                   IPEndPoint clientEndpoint)
    {
        
        var handler = OnL2SessionFound;
        handler?.Invoke(this, new L2SessionFoundEventArgs()
        {
            Direction = direction,
            ServerEndpoint = serverEndpoint,
            ClientEndpoint = clientEndpoint,
            SessionType = sessionType
        });
        return new DummyHandler();
    }
}