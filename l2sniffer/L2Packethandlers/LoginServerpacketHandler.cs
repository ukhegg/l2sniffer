using System.Net;
using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.Packets.LS;

namespace L2sniffer.L2PacketHandlers;

public class LoginServerPacketHandler : L2PacketHandlerBase<LoginServerPacketBase, LoginServerPacketTypes>
{
    private IL2ServerRegistry _serverRegistry;

    public LoginServerPacketHandler(StreamId streamId,
                                    IPacketDecryptorProvider packetDecryptorProvider,
                                    IL2ServerRegistry serverRegistry,
                                    IL2PacketLogger packetLogger)
        : base(packetLogger, packetDecryptorProvider, streamId)
    {
        _serverRegistry = serverRegistry;
    }

    protected override void RegisterHandlers(IHandlersRegistry handlersRegistry)
    {
        handlersRegistry.RegisterHandler<InitPacket>(LoginServerPacketTypes.Init, HandlePacket);
        handlersRegistry.RegisterHandler<LoginOkPacket>(LoginServerPacketTypes.LoginOk, HandlePacket);
        handlersRegistry.RegisterHandler<ServerListPacket>(LoginServerPacketTypes.ServerList, HandlePacket);
        handlersRegistry.RegisterHandler<PlayOkPacket>(LoginServerPacketTypes.PlayOk, HandlePacket);
    }

    protected override IL2PacketDecryptor SelectDecryptor(IPacketDecryptorProvider decryptorProvider,
                                                          StreamId streamId)
    {
        return decryptorProvider.GetLoginSessionDecryptor(streamId);
    }

    private void HandlePacket(InitPacket initPacket, PacketMetainfo metainfo)
    {
    }

    private void HandlePacket(LoginOkPacket initPacket, PacketMetainfo metainfo)
    {
    }

    private void HandlePacket(ServerListPacket initPacket, PacketMetainfo metainfo)
    {
        if (initPacket?.Servers == null) return;
        foreach (var serverInfo in initPacket.Servers)
        {
            _serverRegistry.RegisterGameServer(new IPEndPoint(serverInfo.Ip, (int)serverInfo.Port));
        }
    }

    private void HandlePacket(PlayOkPacket initPacket, PacketMetainfo metainfo)
    {
    }

    protected override void ProcessUnhandledPacket(LoginServerPacketBase packet, PacketMetainfo metainfo)
    {
        
    }
}