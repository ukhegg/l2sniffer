using L2sniffer.Crypto;
using L2sniffer.GameData;
using L2sniffer.GameState;
using L2sniffer.GameState.GameObjects;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.DataStructs;
using L2sniffer.Packets.GS;

namespace L2sniffer.L2PacketHandlers;

public class GameServerPacketHandler : L2PacketHandlerBase<GameServerPacketBase, GameServerPacketTypes>
{
    private readonly ISessionCryptKeysRegistry _cryptoKeysRegistry;
    private readonly GameSession _gameSession;

    public GameServerPacketHandler(StreamId streamId,
                                   ISessionCryptKeysRegistry cryptoKeysRegistry,
                                   IPacketDecryptorProvider packetDecryptorProvider,
                                   IL2PacketLogger packetLogger,
                                   IGameObjectsProvider gameObjectsProvider,
                                   IGameObjectsRegistry gameObjectRegistry,
                                   ISkillInfoProvider skillRegistry,
                                   GameSession gameSession)
        : base(packetLogger, packetDecryptorProvider, streamId)
    {
        _cryptoKeysRegistry = cryptoKeysRegistry;
        _gameSession = gameSession;
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
        handlerRegistry.RegisterHandler<ChangeMoveTypePacket>(GameServerPacketTypes.ChangeMoveType, Handle);
        handlerRegistry.RegisterHandler<MagicEffectIconsPacket>(GameServerPacketTypes.MagicEffectIcons, Handle);
        handlerRegistry.RegisterHandler<CharListPacket>(GameServerPacketTypes.CharList, Handle);
        handlerRegistry.RegisterHandler<ExtendedCodePacket>(GameServerPacketTypes.ExtendedCodes,
                                                            Handle);
        handlerRegistry.RegisterHandler<ShortcutInitPacket>(GameServerPacketTypes.ShortcutInit, Handle);
        handlerRegistry.RegisterHandler<ChangeWaitTypePacket>(GameServerPacketTypes.ChangeWaitType, Handle);
        handlerRegistry.RegisterHandler<StopRotationPacket>(GameServerPacketTypes.StopRotation, Handle);
        handlerRegistry.RegisterHandler<MyTargetSelectedPacket>(GameServerPacketTypes.MyTargetSelected, Handle);
        handlerRegistry.RegisterHandler<PartySmallWindowAllPacket>(GameServerPacketTypes.PartySmallWindowAll, Handle);
        handlerRegistry.RegisterHandler<PartySpelledPacket>(GameServerPacketTypes.PartySpelled, Handle);
        handlerRegistry.RegisterHandler<PartySmallWindowUpdatePacket>(GameServerPacketTypes.PartySmallWindowUpdate,
                                                                      Handle);
        handlerRegistry.RegisterHandler<PartyMemberPositionPacket>(GameServerPacketTypes.PartyMemberPosition, Handle);
        handlerRegistry.RegisterHandler<MagicSkillUsePacket>(GameServerPacketTypes.MagicSkillUse, Handle);
        handlerRegistry.RegisterHandler<ValidateLocationPacket>(GameServerPacketTypes.ValidateLocation, Handle);
        handlerRegistry.RegisterHandler<InventoryUpdatePacket>(GameServerPacketTypes.InventoryUpdate, Handle);
        handlerRegistry.RegisterHandler<MoveToPawlPacket>(GameServerPacketTypes.MoveToPawl, Handle);
        handlerRegistry.RegisterHandler<ActionFailedPacket>(GameServerPacketTypes.ActionFailed, Handle);
        handlerRegistry.RegisterHandler<PartyJoinPacket>(GameServerPacketTypes.JoinParty, Handle);
        handlerRegistry.RegisterHandler<TargetSelectedPacket>(GameServerPacketTypes.TargetSelected, Handle);
        handlerRegistry.RegisterHandler<TargetUnselectedPacket>(GameServerPacketTypes.TargetUnselected, Handle);
        handlerRegistry.RegisterHandler<MagicSkillLaunchedPacket>(GameServerPacketTypes.MagicSkillLaunched,
                                                                  Handle);
        handlerRegistry.RegisterHandler<PartySmallWindowAddPacket>(GameServerPacketTypes.PartySmallWindowAdd,
                                                                   Handle);
        handlerRegistry.RegisterHandler<AutoAttackStartPacket>(GameServerPacketTypes.AutoAttackStart, Handle);
        handlerRegistry.RegisterHandler<AutoAttackStopPacket>(GameServerPacketTypes.AutoAttackStop, Handle);
        handlerRegistry.RegisterHandler<SetupGaugePacket>(GameServerPacketTypes.SetupGauge, Handle);
        handlerRegistry.RegisterHandler<StopMovePacket>(GameServerPacketTypes.StopMove, Handle);
        handlerRegistry.RegisterHandler<DiePacket>(GameServerPacketTypes.Die, Handle);
        handlerRegistry.RegisterHandler<DropItemPacket>(GameServerPacketTypes.DropItem, Handle);
        handlerRegistry.RegisterHandler<GetItemPacket>(GameServerPacketTypes.GetItem, Handle);
        handlerRegistry.RegisterHandler<PartySmallWindowsDeletePacket>(GameServerPacketTypes.PartySmallWindowsDelete,
                                                                       Handle);
        handlerRegistry.RegisterHandler<SkillListPacket>(GameServerPacketTypes.SkillList, Handle);
        handlerRegistry.RegisterHandler<PartySmallWindowDeleteAll>(GameServerPacketTypes.PartySmallWindowsDeleteAll,
                                                                   Handle);
        handlerRegistry.RegisterHandler<LogoutPacket>(GameServerPacketTypes.LogoutOk, Handle);


        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.SignsSky, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.QuestList, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.SendMacroList, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.HennaInfo, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.PledgeShowMemberListAll,
                                                              DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.PledgeShowMemberListAdd,
                                                              DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.MSNDialogs, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.EtcStatusUpdate, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>((GameServerPacketTypes)0x3f, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>((GameServerPacketTypes)0x40, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>((GameServerPacketTypes)0X54, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>((GameServerPacketTypes)0xc1, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.FriendList, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.PledgeInfo, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.RecipeShopSellList, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.RecipeShopItemInfo, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.NetPingRequest, DropPacket);
        handlerRegistry.RegisterHandler<GameServerPacketBase>(GameServerPacketTypes.PlaySound, DropPacket);
    }

    protected override void ProcessUnhandledPacket(GameServerPacketBase packet, PacketMetainfo metainfo)
    {
        int a = 0;
    }

    private void DropPacket(GameServerPacketBase packet, PacketMetainfo metainfo)
    {
    }

    private void Handle(LogoutPacket packet, PacketMetainfo packetMetainfo)
    {
        _gameSession.HandleLogout();
    }

    private void Handle(PartySmallWindowDeleteAll packet, PacketMetainfo packetMetainfo)
    {
        _gameSession.Player.LeaveParty();
    }

    private void Handle(SkillListPacket packet, PacketMetainfo packetMetainfo)
    {
        _gameSession.Player.SetAvailableSkills(packet.Skills);
    }

    private void Handle(PartySmallWindowsDeletePacket packet, PacketMetainfo packetMetainfo)
    {
        _gameSession.Player.Party.Dismiss(packet.MemberId);
    }

    private void Handle(GetItemPacket packet, PacketMetainfo packetMetainfo)
    {
        _gameSession.PickupItem(packet.PlayerId, packet.ObjectId, packet.AtCoordinates);
    }

    private void Handle(DropItemPacket packet, PacketMetainfo packetMetainfo)
    {
        _gameSession.ItemDropped(packet.PlayerId,
                                 packet.ObjectId,
                                 packet.ItemId,
                                 packet.Coordinates, packet.Count,
                                 packet.IsStackable);
    }

    private void Handle(DiePacket packet, PacketMetainfo packetMetainfo)
    {
        _gameSession.Die(packet.CharId, packet.Sweepable == 0x01 ? true : false);
    }

    private void Handle(ExtendedCodePacket packet, PacketMetainfo metainfo)
    {
        switch (packet.ExtendedCode)
        {
            case 0x0d:
                break;
            case 0x12:
                break;
            case 0x1b:
                //unknown
                break;
            case 46:
                break;
            default:
                break;
        }
    }

    private void Handle(StopMovePacket packet, PacketMetainfo metainfo)
    {
        _gameSession.StopMove(packet.Object, packet.Coordinates, packet.Heading);
    }

    private void Handle(SetupGaugePacket packet, PacketMetainfo metainfo)
    {
        int a = 0;
    }

    private void Handle(AutoAttackStartPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.PlayerStartAutoAttack(packet.TargetId);
    }

    private void Handle(AutoAttackStopPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.PlayerStopAutoAttack(packet.TargetId);
    }

    private void Handle(MagicSkillUsePacket packet, PacketMetainfo metainfo)
    {
        var castTime = metainfo.CaptureTime;
        var t = DateTime.UnixEpoch.AddSeconds((uint)castTime.Value);
        _gameSession.HandleMagicSkillUse(packet.SkillUseInfo, t);
    }

    private void Handle(SetToLocationPacket packet, PacketMetainfo metainfo)
    {
        int a = 0;
    }

    private void Handle(PartySmallWindowAddPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.PlayerPartyAdd(packet.NewMember);
    }

    private void Handle(MagicSkillLaunchedPacket packet, PacketMetainfo metainfo)
    {
        var skillId = new MagicSkillId() { Id = packet.SkillId, Level = (ushort)packet.SkillLevel };
        _gameSession.MagicSkillLaunched(packet.CharId,
                                        packet.TargetId,
                                        skillId,
                                        packet.FailedOrNot);
    }

    private void Handle(TargetSelectedPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.TargetSelected(packet.Character, packet.Target, packet.TargetCoordinates);
    }

    private void Handle(TargetUnselectedPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.PlayerTargetUnselected(packet.Target, packet.TargetCoordinates);
    }

    private void Handle(PartyJoinPacket packet, PacketMetainfo metainfo)
    {
        Console.WriteLine($"    Party join response:{packet.Response}");
    }

    private void Handle(ActionFailedPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.CurrentActionFailed();
    }

    private void Handle(MoveToPawlPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.MoveToPawl(packet.CharId, packet.TargetId, packet.Distance, packet.TargetCoordinates);
    }

    private void Handle(InventoryUpdatePacket packet, PacketMetainfo metainfo)
    {
        foreach (var item in packet.InventoryItems)
        {
            switch (item.ChangeType)
            {
                case ItemListEntryChange.ChangeTypes.Add:
                    _gameSession.Player.AddInventoryItem(item.Item);
                    break;
                case ItemListEntryChange.ChangeTypes.Modify:
                    _gameSession.Player.ModifyInventoryItem(item.Item);
                    break;
                case ItemListEntryChange.ChangeTypes.Remove:
                    _gameSession.Player.RemoveInventoryItem(item.Item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Handle(ValidateLocationPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.ValidatePosition(packet.CharId, packet.Position, packet.Heading);
    }


    private void Handle(PartyMemberPositionPacket packet, PacketMetainfo metainfo)
    {
        foreach (var memberPosition in packet.MemberPositions)
        {
            _gameSession.UpdatePosition(memberPosition.Key, memberPosition.Value);
        }
    }

    private void Handle(PartySmallWindowAllPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.SetPlayerParty(packet.Info);
    }

    private void Handle(PartySmallWindowUpdatePacket packet, PacketMetainfo metainfo)
    {
        _gameSession.UpdateParty(packet.Participant);
    }

    private void Handle(PartySpelledPacket packet, PacketMetainfo metainfo)
    {
        var castTime = metainfo.CaptureTime;
        var t = DateTime.UnixEpoch.AddSeconds((uint)castTime.Value);
        _gameSession.OnPartySpell(packet.Char, packet.Effects, t);
    }

    private void Handle(MyTargetSelectedPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.SetPlayerTarget(packet.ObjectId);
    }

    private void Handle(StopRotationPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.StopRotation(packet.CharId, packet.Degree);
    }

    private void Handle(ChangeWaitTypePacket packet, PacketMetainfo metainfo)
    {
        _gameSession.ChangeWaitType(packet.Data);
    }

    private void Handle(ShortcutInitPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.RegisterShortcuts(packet.Shortcuts);
    }

    private void Handle(CharListPacket packet, PacketMetainfo metainfo)
    {
        _gameSession.RegisterChars(packet.Characters);
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

    private void Handle(ChangeMoveTypePacket packet, PacketMetainfo metainfo)
    {
        _gameSession.ChangeMoveType(packet.CharId, packet.MoveType);
    }

    private void Handle(MagicEffectIconsPacket packet, PacketMetainfo metainfo)
    {
        var castTime = metainfo.CaptureTime;
        var t = DateTime.UnixEpoch.AddSeconds((uint)castTime.Value);
        _gameSession.ApplyMagicEffectsOnPlayer(packet.MagicEffects, t);
    }
}