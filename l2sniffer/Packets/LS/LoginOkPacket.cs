namespace L2sniffer.Packets.LS;

public class LoginOkPacket : LoginServerPacketBase
{
    private uint _sessionKey1;
    private uint _sessionKey2;
    private uint _sig1;
    private uint _sig2;

    public LoginOkPacket(byte[] bytes) : base(bytes)
    {
    }

    public uint SessionKey1 => _sessionKey1;
    public uint SessionKey2 => _sessionKey2;
    public uint Sig1 => _sig1;
    public uint Sig2 => _sig2;

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out _sessionKey1);
        fieldsReader.Read(out _sessionKey2);
        fieldsReader.Skip<uint>();
        fieldsReader.Skip<uint>();
        fieldsReader.Read(out _sig1);
        fieldsReader.Skip<uint>();
        fieldsReader.Skip<uint>();
    }
}