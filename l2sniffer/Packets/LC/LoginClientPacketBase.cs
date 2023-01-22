namespace L2sniffer.Packets.LC;

public class LoginClientPacketBase : TypeL2PacketBase<LoginClientPacketTypes>
{
    public LoginClientPacketBase(byte[] bytes) : base(bytes)
    {
    }
}