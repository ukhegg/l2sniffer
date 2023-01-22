using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.GS;

namespace L2sniffer.L2PacketHandlers;

public class GameServerPacketHandler : L2PacketHandlerBase<GameServerPacketBase>
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

    protected override void ProcessPacket(GameServerPacketBase packet)
    {
        switch (packet.PacketType)
        {
            case GameServerPacketTypes.CryptInit:
                Handle(packet.As<CryptInitPacket>());
                break;
            default:
                break;
        }
    }

    protected override IL2PacketDecryptor GetDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetGameSessionDecryptor(streamId);
    }

    private void Handle(CryptInitPacket cryptInitPacket)
    {
        _cryptoKeysRegistry.RegisterGameSessionKey(this._streamId, cryptInitPacket.XorKey);
    }
}