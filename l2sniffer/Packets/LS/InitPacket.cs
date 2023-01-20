namespace L2sniffer.Packets.LS;

public class InitPacket : L2PacketBase
{
    private UInt32 _sessionId;
    private UInt32 _protocolVersion;

    public InitPacket(byte[] bytes) : base(bytes)
    {
    }

    public UInt32 SessionId => _sessionId;

    public UInt32 ProtocolVersion => _protocolVersion;

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out _sessionId);
        fieldsReader.Read(out _protocolVersion);
    }
}