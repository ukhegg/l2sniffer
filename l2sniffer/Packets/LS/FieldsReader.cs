namespace L2sniffer.Packets.LS;

public ref struct FieldsReader
{
    private ReadOnlySpan<byte> _packetData;

    public FieldsReader(ReadOnlySpan<byte> packetData)
    {
        _packetData = packetData;
    }

    public void Read(out byte val)
    {
        val = _packetData[0];
        _packetData = _packetData.Slice(1);
    }

    public void Read(out UInt16 val)
    {
        val = BitConverter.ToUInt16(_packetData);
        _packetData = _packetData.Slice(2);
    }

    public void Read(out UInt32 val)
    {
        val = BitConverter.ToUInt32(_packetData);
        _packetData = _packetData.Slice(4);
    }

    public void Read(out UInt64 val)
    {
        val = BitConverter.ToUInt64(_packetData);
        _packetData = _packetData.Slice(8);
    }

    public void Read(out double val)
    {
        val = BitConverter.ToDouble(_packetData);
        _packetData = _packetData.Slice(sizeof(double));
    }
}