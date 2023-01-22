using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class CharacterInfoPacket : GameServerPacketBase
{
    private MorphedCharacterInfo _characterInfo;

    public CharacterInfoPacket(byte[] bytes) : base(bytes)
    {
    }

    public MorphedCharacterInfo CharacterInfo
    {
        get => _characterInfo;
        private set => _characterInfo = value;
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out _characterInfo);
    }
}