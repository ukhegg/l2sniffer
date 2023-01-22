namespace L2sniffer.Packets.LS;

public class LoginServerPacketBase : TypeL2PacketBase<LoginServerPacketTypes>
{
    public LoginServerPacketBase(byte[] bytes) : base(bytes)
    {
    }
}