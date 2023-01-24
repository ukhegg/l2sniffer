using L2sniffer.Crypto;
using L2sniffer.GameState;
using L2sniffer.GameState.GameObjects;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.GS;

namespace L2sniffer.L2PacketHandlers;

public class GameServerPacketHandler : L2PacketHandlerBase<GameServerPacketBase, GameServerPacketTypes>
{
    private ISessionCryptKeysRegistry _cryptoKeysRegistry;
    private GameSession _gameSession;

    public GameServerPacketHandler(StreamId streamId,
                                   ISessionCryptKeysRegistry cryptoKeysRegistry,
                                   IPacketDecryptorProvider packetDecryptorProvider,
                                   IL2PacketLogger packetLogger,
                                   IGameObjectsProvider gameObjectsProvider,
                                   IGameObjectsRegistry gameObjectRegistry)
        : base(packetLogger, packetDecryptorProvider, streamId)
    {
        _cryptoKeysRegistry = cryptoKeysRegistry;
        _gameSession = new GameSession(gameObjectsProvider, gameObjectRegistry);
    }


    protected override void RegisterHandlers(IHandlersRegistry handlerRegistry)
    {
        handlerRegistry.RegisterHandler<CryptInitPacket>(GameServerPacketTypes.CryptInit, Handle);
        handlerRegistry.RegisterHandler<CharSelectedPacket>(GameServerPacketTypes.CharSelected, Handle);
        handlerRegistry.RegisterHandler<OtherUserInfoPacket>(GameServerPacketTypes.OtherUserInfo, Handle);
        handlerRegistry.RegisterHandler<CharacterInfoPacket>(GameServerPacketTypes.NpcOrCharacterInfo, Handle);
        handlerRegistry.RegisterHandler<ItemsListPacket>(GameServerPacketTypes.ItemsList, Handle);
        handlerRegistry.RegisterHandler<SystemMessagePacket>(GameServerPacketTypes.SystemMessage, Handle);
        handlerRegistry.RegisterHandler<CreatureSayPacket>(GameServerPacketTypes.CreatureSay, Handle);
        handlerRegistry.RegisterHandler<UserInfoPacket>(GameServerPacketTypes.UserInfo, Handle);
        handlerRegistry.RegisterHandler<MoveToLocationPacket>(GameServerPacketTypes.MoveToLocation, Handle);
        handlerRegistry.RegisterHandler<StatusUpdatePacket>(GameServerPacketTypes.StatusUpdate, Handle);
        handlerRegistry.RegisterHandler<SocialActionPacket>(GameServerPacketTypes.SocialAction, Handle);
        handlerRegistry.RegisterHandler<AttackPacket>(GameServerPacketTypes.Attack, Handle);
        handlerRegistry.RegisterHandler<DeleteObjectPacket>(GameServerPacketTypes.DeleteObject, Handle);
    }

    private void Handle(DeleteObjectPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.DeleteObject(packet.ObjectId);
    }

    protected override IL2PacketDecryptor SelectDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetGameSessionDecryptor(streamId);
    }

    private void Handle(CryptInitPacket cryptInitPacket, PacketMetainfo metainfo)
    {
        _cryptoKeysRegistry.RegisterGameSessionKey(this._streamId, cryptInitPacket.XorKey);
    }

    private void Handle(CharSelectedPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.OnCharSelected(packet.SelectedChar);
    }

    private void Handle(CharacterInfoPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.UpdateCharacter(packet.CharacterInfo);
    }

    private void Handle(OtherUserInfoPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.UpdateCharacter(packet.OtherUserInfo);
    }

    private void Handle(ItemsListPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.Player.Update(packet.Items);
    }

    private void Handle(CreatureSayPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.HandleReplica(packet.Replica);
    }

    private void Handle(SystemMessagePacket packet, PacketMetainfo metainfo)
    {
        Console.WriteLine($"    System message:{packet}");
    }

    private void Handle(UserInfoPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.UpdateUserInfo(packet.Info);
    }

    private void Handle(MoveToLocationPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.MoveToLocation(packet.ObjectId, packet.Current, packet.Dst);
    }

    private void Handle(StatusUpdatePacket packet, PacketMetainfo metainfo)
    {
        _gameSession.UpdateStatus(packet.Updates);
    }

    private void Handle(SocialActionPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.HandleAction(packet.PlayerId, packet.ActionId);
    }

    private void Handle(AttackPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.HandleAttack(packet.AttackInfo);
    }
}