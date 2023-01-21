namespace L2sniffer.Packets.LS;

public class PlayOkPacket : LoginServerPacketBase
{
    private ushort _sessionKey1;
    private ushort _sessionKey2;

    public PlayOkPacket(byte[] bytes) : base(bytes)
    {
    }

    public ushort SessionKey1 => _sessionKey1;

    public ushort SessionKey2 => _sessionKey2;

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out _sessionKey1);
        fieldsReader.Read(out _sessionKey2);
    }
}