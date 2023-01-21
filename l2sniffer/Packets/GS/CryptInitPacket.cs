using System.Buffers.Binary;

namespace L2sniffer.Packets.GS;

public class CryptInitPacket : GameServerPacketBase
{
    public uint XorKey;

    public CryptInitPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out byte _);
        fieldsReader.Read(out XorKey);
        XorKey = BinaryPrimitives.ReverseEndianness(XorKey);
        fieldsReader.Read(out uint _);
        fieldsReader.Read(out uint _);
    }
}