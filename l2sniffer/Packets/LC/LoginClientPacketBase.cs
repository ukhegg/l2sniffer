namespace L2sniffer.Packets.LC;

public class LoginClientPacketBase : L2PacketBase
{
    public LoginClientPacketBase(byte[] bytes) : base(bytes)
    {
    }

    public LoginClientPacketTypes PacketType => (LoginClientPacketTypes)PacketTypeRaw;
}