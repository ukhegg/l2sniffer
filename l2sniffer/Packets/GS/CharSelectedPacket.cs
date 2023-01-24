using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class CharSelectedPacket : GameServerPacketBase
{
    private SelectedCharInfo _selectedChar;

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out _selectedChar);
    }

    public CharSelectedPacket(byte[] bytes) : base(bytes)
    {
    }

    public SelectedCharInfo SelectedChar => _selectedChar;
}