using l2sniffer.PacketHandlers;

namespace L2sniffer.StreamHandlers;

public class L2DatagramStreamReader : IDatagramStreamReader
{
    public static IDatagramStreamReader Instance = new L2DatagramStreamReader();

    public uint HeaderLength => 2;

    public uint GetRecordLength(byte[] headerBytes)
    {
        return BitConverter.ToUInt16(headerBytes, 0);
    }

    public class Provider : IDatagramStreamReaderProvider
    {
        public IDatagramStreamReader GetStreamReader(IpDirection ipDirection, TransportDirection ports)
        {
            return L2DatagramStreamReader.Instance;
        }
    }
}