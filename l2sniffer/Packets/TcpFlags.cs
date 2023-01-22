namespace L2sniffer.Packets;

public ref struct TcpFlags
{
    private ushort _flags;

    public TcpFlags(ushort flags)
    {
        _flags = flags;
    }

    public bool Fin => (_flags & (ushort)0x0001) != 0;
    public bool Syn => (_flags & (ushort)0x0002) != 0;
    public bool Rst => (_flags & (ushort)0x0004) != 0;
    public bool Push => (_flags & (ushort)0x0008) != 0;
    public bool Ack => (_flags & (ushort)0x0010) != 0;
    public bool Urg => (_flags & (ushort)0x0020) != 0;
    public bool Echo => (_flags & (ushort)0x0040) != 0;
    public bool CWR => (_flags & (ushort)0x0080) != 0;
    public bool Ecn => (_flags & (ushort)0x0100) != 0;
}