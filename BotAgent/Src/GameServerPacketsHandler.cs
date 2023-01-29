using L2sniffer;
using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.Packets.GS;

namespace BotAgent;

public class GameServerPacketsHandler : IPacketHandler<L2PacketBase>
{
    private ISessionCryptKeysRegistry _cryptKeysRegistry;
    private IPacketHandler<L2PacketBase> _nextHandler;

    public GameServerPacketsHandler(ISessionCryptKeysRegistry cryptKeysRegistry,
                                    IPacketHandler<L2PacketBase> nextHandler)
    {
        _cryptKeysRegistry = cryptKeysRegistry;
        _nextHandler = nextHandler;
    }

    public void HandlePacket(L2PacketBase packet, PacketMetainfo metainfo)
    {
        if (packet.PacketTypeRaw == (byte)GameServerPacketTypes.CryptInit)
        {
            var cryptInitPacket = packet.As<CryptInitPacket>();

            var streamId = new StreamId(metainfo.TopLevelIpDirection, metainfo.TransportPorts);
            _cryptKeysRegistry.RegisterGameSessionKey(streamId, cryptInitPacket.XorKey);
        }

        _nextHandler.HandlePacket(packet, metainfo);
    }
}