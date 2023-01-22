using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.GS;

namespace L2sniffer.L2PacketHandlers;

public class GameServerPacketHandler : L2PacketHandlerBase<GameServerPacketBase, GameServerPacketTypes>
{
    private ISessionCryptKeysRegistry _cryptoKeysRegistry;

    public GameServerPacketHandler(StreamId streamId,
                                   ISessionCryptKeysRegistry cryptoKeysRegistry,
                                   IPacketDecryptorProvider packetDecryptorProvider,
                                   IL2PacketLogger packetLogger)
        : base(packetLogger, packetDecryptorProvider, streamId)
    {
        _cryptoKeysRegistry = cryptoKeysRegistry;
    }


    protected override void RegisterHandlers(IHandlersRegistry handlerRegistry)
    {
        handlerRegistry.RegisterHandler<CryptInitPacket>(GameServerPacketTypes.CryptInit, Handle);
    }

    protected override IL2PacketDecryptor SelectDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetGameSessionDecryptor(streamId);
    }

    private void Handle(CryptInitPacket cryptInitPacket, PacketMetainfo metainfo)
    {
        _cryptoKeysRegistry.RegisterGameSessionKey(this._streamId, cryptInitPacket.XorKey);
    }
}