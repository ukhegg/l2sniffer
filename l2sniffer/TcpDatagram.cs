namespace L2sniffer;

public ref struct TcpDatagram
{
    public ReadOnlySpan<byte> Bytes { get; set; }
}