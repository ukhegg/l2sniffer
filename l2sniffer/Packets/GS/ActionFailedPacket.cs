namespace L2sniffer.Packets.GS;

public class ActionFailedPacket : GameServerPacketBase
{
    public ActionFailedPacket(byte[] bytes) : base(bytes)
    {
    }
}