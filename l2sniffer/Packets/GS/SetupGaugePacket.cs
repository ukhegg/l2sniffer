namespace L2sniffer.Packets.GS;

public enum GaugeColors : uint
{
    Blue = 0,
    Red = 1,
    Cyan = 2
}

public class SetupGaugePacket : GameServerPacketBase
{
    public GaugeColors Color;
    public uint Time;

    public SetupGaugePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.ReadEnum(out Color);
        reader.Read(out Time);
        reader.Read(out uint time2);
    }
}