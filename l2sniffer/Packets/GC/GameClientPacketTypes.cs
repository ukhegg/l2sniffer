﻿namespace L2sniffer.Packets.GC;

public enum GameClientPacketTypes : byte
{
    ProtocolVersion = 0x00,
    MoveBackwardToLocation = 0x01,
    Say = 0x02,
    EnterWorld = 0x03,
    Action = 0x04,
    AuthRequest = 0x08,
    Logout = 0x09,
    AttackRequest = 0x0A,
    CharacterCreate = 0x0B,
    CharacterDelete = 0x0C,
    CharacterSelected = 0x0D,
    RequestItemList = 0x0F,
    RequestUnEquipItem = 0x11,
    RequestDropItem = 0x12,
    UseItem = 0x14,
    TradeRequest = 0x15,
    AddTradeItem = 0x16,
    TradeDone = 0x17,
    RequestSocialAction = 0x1B,
    ChangeMoveType = 0x1C,

    // устарел. Теперь юзается 'RequestActionUse'
    ChangeWaitType = 0x1D,

    // устарел. Теперь юзается 'RequestActionUse'
    RequestSellItem = 0x1E,
    RequestBuyItem = 0x1F,
    RequestBypassToServer = 0x21,
    RequestJoinPledge = 0x24,
    RequestAnswerJoinPledge = 0x25,
    RequestWithdrawalPledge = 0x26,
    RequestOustPledgeMember = 0x27,
    RequestJoinParty = 0x29,
    RequestAnswerJoinParty = 0x2A,
    RequestWithDrawalParty = 0x2B,
    RequestOustPartyMember = 0x2C,
    RequestMagicSkillUse = 0x2F,
    Appearing = 0x30,
    RequestShortCutReg = 0x33,
    RequestShortCutDel = 0x35,
    RequestTargetCanceld = 0x37,
    Say2 = 0x38,
    RequestPledgeMemberList = 0x3C,
    RequestSkillList = 0x3F,
    AnswerTradeRequest = 0x40,
    RequestActionUse = 0x45,
    RequestRestart = 0x46,
    ValidatePosition = 0x48,
    StartRotating = 0x4A,
    FinishRotating = 0x4B,
    RequestStartPledgeWar = 0x4D,
    RequestStopPledgeWar = 0x4F,
    RequestGiveNickName = 0x55,
    RequestEnchantItem = 0x58,
    RequestDestroyItem = 0x59,
    RequestFriendInvite = 0x5E,
    RequestAnswerFriendInvite = 0x5F,
    RequestFriendList = 0x60,
    RequestFriendDel = 0x61,
    CharacterRestore = 0x62,
    RequestQuestList = 0x63,
    RequestQuestAbort = 0x64,
    RequestPledgeInfo = 0x66,
    RequestPledgeCrest = 0x68,
    RequestRide = 0x6A,
    RequestAquireSkillInfo = 0x6B,
    RequestAquireSkill = 0x6C,
    RequestRestartPoint = 0x6D,
    RequestGmCommand = 0x6E,
    RequestPartyMatchConfig = 0x6F,
    RequestPartyMatchList = 0x70,
    RequestPartyMatchDetail = 0x71,
    RequestCrystallizeItem = 0x72,
    SetPrivateStoreMsgSell = 0x77,
    RequestGmList = 0x81,
    RequestJoinAlly = 0x82,
    RequestAnswerJoinAlly = 0x83,
    AllyLeave = 0x84,
    AllyDismiss = 0x85,
    RequestAllyCrest = 0x88,
    RequestChangePetName = 0x89,
    RequestPetUseItem = 0x8A,
    RequestGiveItemToPet = 0x8B,
    RequestGetItemFromPet = 0x8C,
    RequestAllyInfo = 0x8E,
    RequestPetGetItem = 0x8F,
    SetPrivateStoreMsgBuy = 0x94,
    RequestStartAllianceWar = 0x98,
    RequestStopAllianceWar = 0x9A,
    RequestBlock = 0xA0,
    RequestSiegeAttackerList = 0xA2,
    RequestJoinSiege = 0xA4,
    NetPing = 0xA8,
    RequestRecipeBookOpen = 0xAC,
    RequestEvaluate = 0xB9,
    RequestHennaList = 0xBA,
    RequestHennaItemInfo = 0xBB,
    RequestHennaEquip = 0xBC,
    RequestMakeMacro = 0xC1,
    RequestDeleteMacro = 0xC2,
    RequestAutoSoulShot = 0xCF,
    RequestExEnchantSkillInfo = 0xD0,

    //:06,
    RequestExEnchantSkill = 0xD0,

    //: 07,
    RequestExManorList = 0xD0,

    //: 08,
    RequestExPledgeCrestLarge = 0xD0,

    //:10,
    RequestExSetPledgeCrestLarge = 0xD0,

    // : 11,
    RequestChangePartyLeader = 0xEE,
}