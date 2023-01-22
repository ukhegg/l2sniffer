using L2sniffer.Packets;
using PacketDotNet;

namespace L2sniffer.PacketHandlers;

internal class TcpReorderer : IPacketHandler<TcpPacket>
{
    public class PendingTcp
    {
        public PendingTcp(TcpPacket packet, PacketMetainfo metainfo)
        {
            Packet = packet;
            Metainfo = metainfo;
        }

        public TcpPacket Packet;
        public PacketMetainfo Metainfo;
    }

    class TcpComparer : IComparer<PendingTcp>
    {
        public static IComparer<PendingTcp> Instance = new TcpComparer();

        public int Compare(PendingTcp x, PendingTcp y)
        {
            if (x.Packet.SequenceNumber != y.Packet.SequenceNumber)
            {
                return x.Packet.SequenceNumber.CompareTo(y.Packet.SequenceNumber);
            }

            return x.Packet.Bytes.Length.CompareTo(y.Packet.Bytes.Length);
        }
    }

    enum State
    {
        Initial,
        Standby,
        Accumulating,
        Teardown
    }

    private static uint _maxReorderBufferSize = 10;

    private State _state = State.Initial;
    private uint _waitingSeq = 0;
    private SortedSet<PendingTcp> _pendingPackets = new(TcpComparer.Instance);

    private readonly IAssembledTcpHandler _handler;

    public TcpReorderer(IAssembledTcpHandler handler)
    {
        _handler = handler;
    }

    public void HandlePacket(TcpPacket packet, PacketMetainfo metainfo)
    {
        switch (_state)
        {
            case State.Initial:
                HandleInitial(packet, metainfo);
                break;
            case State.Standby:
                HandleStandby(packet, metainfo);
                break;
            case State.Accumulating:
                HandleAccumulating(packet, metainfo);
                break;
            case State.Teardown:
                HandleTeardown(packet, metainfo);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Reset()
    {
        _pendingPackets.Clear();
        _waitingSeq = 0;
        _state = State.Initial;
    }

    public void SaveBufferAndReset(Action<SortedSet<PendingTcp>> callback)
    {
        if (_pendingPackets.Any()) callback(_pendingPackets);
        Reset();
    }

    private void HandleInitial(TcpPacket p, PacketMetainfo metainfo)
    {
        if (p.Flags2().Fin)
        {
            _handler.HandleReordered(p, metainfo);
            return;
        }

        if (p.Flags2().Syn)
        {
            if (_pendingPackets.Count == 0)
            {
                _handler.HandleReordered(p, metainfo);
                _waitingSeq = p.SequenceNumber + 1;
                _state = State.Standby;
                return;
            }

            _pendingPackets.Add(new PendingTcp(p, metainfo));
            _waitingSeq = p.SequenceNumber;
            _state = State.Accumulating;

            TryFlushInitial();
            return;
        }

        _pendingPackets.Add(new PendingTcp(p, metainfo));
        if (_pendingPackets.Count < _maxReorderBufferSize)
        {
            return;
        }

        _state = State.Accumulating;
        _waitingSeq = _pendingPackets.First().Packet.SequenceNumber;
        TryFlush();
    }

    private void HandleStandby(TcpPacket p, PacketMetainfo metainfo)
    {
        if (p.Flags2().Fin)
        {
            if (SeqNumberIsValid(p))
            {
                _handler.HandleReordered(p, metainfo);
                _waitingSeq = (uint)(_waitingSeq + p.PayloadData.Length + 1);
                _state = State.Teardown;
                return;
            }

            ProcessPacketOutOfOrderWarning(p, metainfo);
            return;
        }

        if (p.Flags2().Syn)
        {
            ProcessPacketOutOfOrderWarning(p, metainfo);
            return;
        }

        if (SeqNumberIsValid(p))
        {
            _waitingSeq = (uint)(_waitingSeq + p.PayloadData.Length);
            _handler.HandleReordered(p, metainfo);
        }
        else
        {
            if (p.SequenceNumber < _waitingSeq)
            {
                ProcessPacketOutOfOrderWarning(p, metainfo);
                return;
            }

            _state = State.Accumulating;
            _pendingPackets.Add(new PendingTcp(p, metainfo));
        }
    }

    private void HandleTeardown(TcpPacket p, PacketMetainfo metainfo)
    {
        if (SeqNumberIsValid(p))
        {
            _handler.HandleReordered(p, metainfo);
        }
        else
        {
            ProcessPacketOutOfOrderWarning(p, metainfo);
        }
    }

    private void HandleAccumulating(TcpPacket p, PacketMetainfo metainfo)
    {
        if ((p.SequenceNumber < _waitingSeq) || p.Flags2().Syn || p.Flags2().Fin)
        {
            ProcessPacketOutOfOrderWarning(p, metainfo);
            return;
        }

        if (SeqNumberIsValid(p))
        {
            _waitingSeq = (uint)(_waitingSeq + p.PayloadData.Length);
            _handler.HandleReordered(p, metainfo);
        }
        else
        {
            _pendingPackets.Add(new PendingTcp(p, metainfo));
            if (_pendingPackets.Count > _maxReorderBufferSize)
            {
                var pt = _pendingPackets.First();
                ProcessIntervalMissingWarning(pt.Packet);
                _handler.HandleReordered(pt.Packet, pt.Metainfo);
                _waitingSeq = (uint)(pt.Packet.SequenceNumber + pt.Packet.PayloadData.Length);
                _pendingPackets.Remove(pt);
            }
        }

        TryFlush();
    }

    void TryFlushInitial()
    {
        if (_pendingPackets.Count == 0) return;
        var firstPacket = _pendingPackets.First();
        if (firstPacket.Packet.Flags2().Syn)
        {
            _waitingSeq++;
            _handler.HandleReordered(firstPacket.Packet, firstPacket.Metainfo);
            _pendingPackets.Remove(firstPacket);
        }

        TryFlush();
    }

    void TryFlush()
    {
        if (_pendingPackets.Count == 0) return;

        foreach (var pt in _pendingPackets)
        {
            if (SeqNumberIsValid(pt.Packet))
            {
                if (pt.Packet.Flags2().Syn)
                {
                    ProcessPacketOutOfOrderWarning(pt.Packet, pt.Metainfo);
                    _waitingSeq++;
                }
                else
                {
                    if (pt.Packet.Flags2().Fin)
                    {
                        _handler.HandleReordered(pt.Packet, pt.Metainfo);
                        if (!ReferenceEquals(pt, _pendingPackets.First()))
                        {
                            while (!ReferenceEquals(_pendingPackets.First(), pt))
                            {
                                _pendingPackets.Remove(_pendingPackets.First());
                            }
                        }

                        _state = State.Initial;
                        _waitingSeq = 0;
                        return;
                    }

                    _waitingSeq = (uint)(_waitingSeq + pt.Packet.PayloadData.Length);
                    _handler.HandleReordered(pt.Packet, pt.Metainfo);
                }
            }
            else
            {
                if (pt.Packet.Flags2().Syn)
                {
                    ProcessPacketOutOfOrderWarning(pt.Packet, pt.Metainfo);
                    _waitingSeq = pt.Packet.SequenceNumber + 1;

                    if (!ReferenceEquals(pt, _pendingPackets.First()))
                    {
                        while (!ReferenceEquals(_pendingPackets.First(), pt))
                        {
                            _pendingPackets.Remove(_pendingPackets.First());
                        }
                    }
                    else
                    {
                        _pendingPackets.Remove(_pendingPackets.First());
                    }

                    return;
                }

                if (pt.Packet.Flags2().Fin)
                {
                    ProcessPacketOutOfOrderWarning(pt.Packet, pt.Metainfo);
                    _waitingSeq = pt.Packet.SequenceNumber + 1;

                    if (!ReferenceEquals(pt, _pendingPackets.First()))
                    {
                        while (!ReferenceEquals(_pendingPackets.First(), pt))
                        {
                            _pendingPackets.Remove(_pendingPackets.First());
                        }
                    }
                    else
                    {
                        _pendingPackets.Remove(_pendingPackets.First());
                    }

                    return;
                }

                if (!ReferenceEquals(pt, _pendingPackets.First()))
                {
                    while (!ReferenceEquals(_pendingPackets.First(), pt))
                    {
                        _pendingPackets.Remove(_pendingPackets.First());
                    }
                }

                return;
            }
        }

        _pendingPackets.Clear();
        _state = State.Standby;
    }

    void DoEmergencyBufferFlush(SortedSet<PendingTcp> pendingPackets)
    {
        foreach (var pt in pendingPackets)
        {
            if (SeqNumberIsValid(pt.Packet))
            {
                _waitingSeq = (uint)(_waitingSeq + pt.Packet.PayloadData.Length);
                _handler.HandleReordered(pt.Packet, pt.Metainfo);
            }
            else
            {
                ProcessIntervalMissingWarning(pt.Packet);
                _waitingSeq = (uint)(pt.Packet.SequenceNumber + pt.Packet.PayloadData.Length);
                _handler.HandleReordered(pt.Packet, pt.Metainfo);
            }
        }
    }

    void ProcessIntervalMissingWarning(TcpPacket p)
    {
        _handler.HandleIntervalMissing(_waitingSeq, p.SequenceNumber);
    }

    void ProcessPacketOutOfOrderWarning(TcpPacket packet, PacketMetainfo metainfo)
    {
        if (packet.SequenceNumber < _waitingSeq
            && packet.SequenceNumber + packet.PayloadData.Length > _waitingSeq)
        {
            var overlapSize = _waitingSeq - packet.SequenceNumber;
            _waitingSeq = (uint)(packet.SequenceNumber + packet.PayloadData.Length);
            _handler.HandlePartialOverlap(packet, metainfo, overlapSize);
        }
        else
        {
            _handler.HandleOutOfIndexPacket(packet, metainfo);
        }
    }

    bool SeqNumberIsValid(TcpPacket packet)
    {
        return packet.SequenceNumber == _waitingSeq;
    }
}