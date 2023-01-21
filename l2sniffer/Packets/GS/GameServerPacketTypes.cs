﻿namespace L2sniffer.Packets.GS;

public enum GameServerPacketTypes : byte
{
    CryptInit = 0x00,
    MoveToLocation = 0x01,
    UserInfo = 0x04,
    StatusUpdate = 0x0e,
    CharList = 0x13,
    AuthLoginFail = 0x14,
    CharSelected = 0x15,
    CharacterInfo = 0x16,//both for users and npc
    CharCreateOk = 0x19,
    CharCreateFail = 0x1a,
    ItemsList = 0x1b,
    CharDeleteOk = 0x23,
    CharDeleteFail = 0x24,
    ActionFailed = 0x25,
    InventoryUpdate = 0x27,
    SocialAction = 0x2d,
    ChangeWaitType = 0x2f,
    TeleportToLocation = 0x38,
    ChangeMoveType = 0x3e,
    ShortcutInit = 0x45,
    CreatureSay = 0x4a,
    PledgeShowMemberListAll = 0x53,
    SystemMessage = 0x64,
    LogoutOk = 0x7e,
    MagicEffectIcons = 0x7f,
    QuestList = 0x80,
    PledgeInfo = 0x83,
    NetPingRequest = 0xd3,
    ServerSocketClose = 0xaf,
    ChairSit = 0xe1,
    HennaInfo = 0xe4,
    SendMacroList = 0xe7,
    SignsSky = 0xf8,
    FriendList = 0xfa,
    ExSendManorList = 0xfe,
}