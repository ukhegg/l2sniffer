namespace L2sniffer.Packets.GS;

public class LogoutPacket : GameServerPacketBase
{
    public LogoutPacket(byte[] bytes) : base(bytes)
    {
    }
}