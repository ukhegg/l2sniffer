using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.GC;

namespace L2sniffer.L2PacketHandlers;

public class GameClientPacketHandler : L2PacketHandlerBase<GameClientPacketBase, GameClientPacketTypes>
{
    public GameClientPacketHandler(StreamId streamId,
                                   IPacketDecryptorProvider packetDecryptorProvider,
                                   IL2PacketLogger packetLogger)
        : base(packetLogger, packetDecryptorProvider, streamId)
    {
    }

    protected override void RegisterHandlers(IHandlersRegistry handlersRegistry)
    {
    }

    protected override IL2PacketDecryptor SelectDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetGameSessionDecryptor(streamId);
    }
}