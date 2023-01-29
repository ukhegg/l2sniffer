namespace L2sniffer.Packets.GS;

public enum ExtendedCodes : byte
{
    ExAutoSoulShot = 0x12,
    ExFishingStart = 0x13,
    ExFishingEnd = 0x14,
    ExFishingStartCombat = 0x15,
    ExFishingHpRegen = 0x16,
    ExEnchantSkillList = 0x17,
    ExEnchantSkillInfo = 0x18,
    QuestInfo = 0x19,
    ExSendManorList = 0x1b,
    HeroList = 0x23,
    PledgeCrestLarge = 0x28,
    OlympiadUserInfo = 0x29,
    OlimpiadSpelledMode = 0x2a,
    OlimpiadMode = 0x2b,
    MailArrived = 0x2d,
    StorageMaxCount = 0x2e
}

public class ExtendedCodePacket : GameServerPacketBase
{
    public byte ExtendedCode;

    public ExtendedCodePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out ExtendedCode);
    }
}