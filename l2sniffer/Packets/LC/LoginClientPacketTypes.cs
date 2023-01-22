namespace L2sniffer.Packets.LC;

public enum LoginClientPacketTypes : byte
{
    RequestAuthLogin = 0x00,
    RequestServerLogin = 0x02,
    RequestServerList = 0x05,
    RequestGgAuth = 0x07,
}