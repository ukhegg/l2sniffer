namespace L2sniffer.Packets.LC;

public class LoginClientPacketTypedPacket : L2PacketBase
{
    public LoginClientPacketTypedPacket(byte[] bytes) : base(bytes)
    {
    }

    public LoginClientPacketTypes PacketType => (LoginClientPacketTypes)PacketTypeRaw;
}