using System.Net;
using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.LS;

namespace L2sniffer.L2PacketHandlers;

public class LoginServerPacketHandler : L2PacketHandlerBase<LoginServerPacketBase>
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

    protected override void ProcessPacket(LoginServerPacketBase packet)
    {
        switch (packet.PacketType)
        {
            case LoginServerPacketTypes.Init:
                HandlePacket(packet.As<InitPacket>());
                break;
            case LoginServerPacketTypes.LogicFail:
                break;
            case LoginServerPacketTypes.AccountKicked:
                break;
            case LoginServerPacketTypes.LoginOk:
                HandlePacket(packet.As<LoginOkPacket>());
                break;
            case LoginServerPacketTypes.ServerList:
                HandlePacket(packet.As<ServerListPacket>());
                break;
            case LoginServerPacketTypes.PlayFail:
                break;
            case LoginServerPacketTypes.PlayOk:
                HandlePacket(packet.As<PlayOkPacket>());
                break;
            case LoginServerPacketTypes.GgAuth:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override IL2PacketDecryptor GetDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetLoginSessionDecryptor(streamId);
    }

    private void HandlePacket(InitPacket? initPacket)
    {
    }

    private void HandlePacket(LoginOkPacket? initPacket)
    {
    }

    private void HandlePacket(ServerListPacket? initPacket)
    {
        if (initPacket?.Servers == null) return;
        foreach (var serverInfo in initPacket.Servers)
        {
            _serverRegistry.RegisterGameServer(new IPEndPoint(serverInfo.Ip, (int)serverInfo.Port));
        }
    }

    private void HandlePacket(PlayOkPacket? initPacket)
    {
    }
}