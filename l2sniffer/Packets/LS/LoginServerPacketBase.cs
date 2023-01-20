namespace L2sniffer.Packets.LS;

public class LoginServerPacketBase : L2PacketBase
{
    public LoginServerPacketBase(byte[] bytes) : base(bytes)
    {
    }

    public LoginServerPacketTypes PacketType => (LoginServerPacketTypes)PacketTypeRaw;
}