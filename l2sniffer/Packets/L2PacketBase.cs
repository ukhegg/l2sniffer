﻿using L2sniffer.Packets.LS;
using PacketDotNet.Utils;

namespace L2sniffer.Packets;

public class L2PacketBase
{
    private readonly byte[] _bytes;

    public L2PacketBase(byte[] bytes)
    {
        _bytes = bytes;
        var fieldsReader = new FieldsReader(_bytes);
        fieldsReader.Read(out UInt16 length);
        if (length != bytes.Length)
        {
            throw new ArgumentException($"Length in packet({length}) does not match bytes[] length({bytes.Length})");
        }

        fieldsReader.Read(out byte packetTypeRaw);
        PacketTypeRaw = packetTypeRaw;
        ReadPayloadFields(fieldsReader);
    }

    public ReadOnlySpan<byte> Bytes => new(_bytes);

    public UInt16 Length => BitConverter.ToUInt16(Bytes);

    protected byte PacketTypeRaw { get; }

    protected virtual void ReadPayloadFields(FieldsReader fieldsReader)
    {
    }
}