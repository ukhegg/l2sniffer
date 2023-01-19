namespace L2sniffer.StreamHandlers;

public interface IDatagramStreamReader
{
    uint HeaderLength { get; }

    uint GetRecordLength(byte[] headerBytes);
}