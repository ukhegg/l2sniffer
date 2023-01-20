namespace L2sniffer.Packets.LS;

public enum LoginServerPacketTypes : byte
{
    Init = 0x00,
    LogicFail = 0x01,
    AccountKicked = 0x02,
    LoginOk = 0x03,
    ServerList = 0x04,
    PlayFail = 0x06,
    PlayOk = 0x07,
    GgAuth = 0x0b
}