using l2sniffer.PacketHandlers;
using PacketDotNet;

namespace L2sniffer.StreamHandlers;

public class TcpSegmentsSplitter : IPacketHandler<TcpPacket>
{
    private IDatagramStreamReader _streamReader;
    private IDatagramStreamHandler _datagramHandler;
    private byte[] _pendingBytes;

    public TcpSegmentsSplitter(IpDirection ipDirection,
                               TransportDirection ports,
                               IDatagramStreamReader streamReader,
                               IDatagramStreamHandler datagramHandler)
    {
        IpDir = ipDirection;
        Ports = ports;
        _streamReader = streamReader;
        _datagramHandler = datagramHandler;
        _pendingBytes = Array.Empty<byte>();
    }

    public TransportDirection Ports { get; }

    public IpDirection IpDir { get; }

    public void HandlePacket(TcpPacket packet, PacketMetainfo metainfo)
    {
        _pendingBytes = Concat(_pendingBytes, packet.PayloadData);
        while (_pendingBytes.Length > _streamReader.HeaderLength)
        {
            var recordLength = _streamReader.GetRecordLength(_pendingBytes);
            if (_pendingBytes.Length < recordLength)
            {
                return;
            }

            var recordAndLeft = Split(_pendingBytes, recordLength);
            _datagramHandler.HandleDatagram(recordAndLeft.Item1, metainfo);
            _pendingBytes = recordAndLeft.Item2;
        }
    }

    private byte[] Concat(byte[] x, byte[] y)
    {
        if (x.Length == 0) return y;
        if (y.Length == 0) return x;
        var z = new byte[x.Length + y.Length];
        x.CopyTo(z, 0);
        y.CopyTo(z, x.Length);
        return z;
    }

    private Tuple<byte[], byte[]> Split(byte[] data, uint position)
    {
        var first = new byte[position];
        var second = new byte[data.Length - position];
        Array.Copy(data, 0, first, 0, first.Length);
        Array.Copy(data, position, second, 0, second.Length);
        return new Tuple<byte[], byte[]>(first, second);
    }
}