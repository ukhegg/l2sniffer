using L2sniffer.Crypto;
using L2sniffer.GameState;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.GC;

namespace L2sniffer.L2PacketHandlers;

public class GameClientPacketHandler : L2PacketHandlerBase<GameClientPacketBase, GameClientPacketTypes>
{
    private readonly GameSession _gameSession;

    public GameClientPacketHandler(StreamId streamId,
                                   IPacketDecryptorProvider packetDecryptorProvider,
                                   IL2PacketLogger packetLogger,
                                   GameSession gameSession)
        : base(packetLogger, packetDecryptorProvider, streamId)
    {
        _gameSession = gameSession;
    }

    protected override void RegisterHandlers(IHandlersRegistry handlersRegistry)
    {
    }

    protected override IL2PacketDecryptor SelectDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetGameSessionDecryptor(streamId);
    }

    protected override void ProcessUnhandledPacket(GameClientPacketBase packet, PacketMetainfo metainfo)
    {
    }
}