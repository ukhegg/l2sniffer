using System.Net;
using L2sniffer;
using L2sniffer.Packets;

namespace BotAgent;

public enum L2SessionTypes
{
    Login,
    Game
}

public enum ConversationDirections
{
    ClientToServer,
    ServerToClient
}

public interface IL2StreamsHandlerProvider
{
    IPacketHandler<L2PacketBase> GetHandler(L2SessionTypes sessionType,
                                            ConversationDirections direction,
                                            IPEndPoint serverEndpoint,
                                            IPEndPoint clientEndpoint);
}