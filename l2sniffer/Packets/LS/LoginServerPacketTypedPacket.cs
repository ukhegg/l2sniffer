namespace L2sniffer.Packets.LS;

public class LoginServerPacketTypedPacket : L2PacketBase
{
    public LoginServerPacketTypedPacket(byte[] bytes) : base(bytes)
    {
    }

    public LoginServerPacketTypes PacketType => (LoginServerPacketTypes)PacketTypeRaw;
}