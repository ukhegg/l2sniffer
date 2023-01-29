using System.Net;
using L2sniffer;
using L2sniffer.L2PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.Packets.LS;

namespace BotAgent;

public class LoginServerPacketHandler : IPacketHandler<L2PacketBase>
{
    private readonly IL2ServerRegistry _serversRegistry;
    private readonly IPacketHandler<L2PacketBase> _nextHandler;

    public LoginServerPacketHandler(IL2ServerRegistry serversRegistry,
                                    IPacketHandler<L2PacketBase> nextHandler)
    {
        _serversRegistry = serversRegistry;
        _nextHandler = nextHandler;
    }

    public void HandlePacket(L2PacketBase packet, PacketMetainfo metainfo)
    {
        if (packet.PacketTypeRaw == (byte)LoginServerPacketTypes.ServerList)
        {
            var gameServers = packet.As<ServerListPacket>().Servers;
            foreach (var gameServer in gameServers)
            {
                var serverEndpoint = new IPEndPoint(gameServer.Ip, (ushort)gameServer.Port);
                _serversRegistry.RegisterGameServer(serverEndpoint);
            }
        }

        _nextHandler.HandlePacket(packet, metainfo);
    }
}