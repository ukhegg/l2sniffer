using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class MoveToLocationPacket : GameServerPacketBase
{
    public MoveToLocationPacket(byte[] bytes) : base(bytes)
    {
    }

    public uint ObjectId;
    public Coordinates3d Dst;
    public Coordinates3d Current;

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out ObjectId);
        fieldsReader.Read(out Dst);
        fieldsReader.Read(out Current);
    }
}