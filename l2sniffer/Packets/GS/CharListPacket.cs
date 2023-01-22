using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class CharListPacket : GameServerPacketBase
{
    public CharacterInfo[] Characters{ get; private set; }

    public CharListPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out uint charCount);
        Characters = new CharacterInfo[charCount];
        for (var i = 0; i < charCount; ++i)
        {
            fieldsReader.Read(out Characters[i]);
        }
    }
}