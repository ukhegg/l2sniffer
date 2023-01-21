using L2sniffer.Crypto;
using L2sniffer.Packets.LS;

namespace Tests;

public class LoginServerPacketsTest
{
    private TestHelper _testHelper = new TestHelper();
    private IL2PacketDecryptor _decryptor = new LoginSessionDecryptor();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CanParseLoginInitPacket()
    {
        var packet = _testHelper.GetL2Packet("l2-1.pcap", 0);
        var initPacket = new InitPacket(packet.Item1);
        Assert.That(initPacket.PacketType, Is.EqualTo(LoginServerPacketTypes.Init));
        Assert.That(initPacket.Length, Is.EqualTo(11));
        Assert.That(initPacket.Bytes.Length, Is.EqualTo(initPacket.Length));
        Assert.That(initPacket.SessionId, Is.EqualTo(0x1bfc47b1));
        Assert.That(initPacket.ProtocolVersion, Is.EqualTo(0x0000785a));
    }

    [Test]
    public void CanParseLoginOkPacket()
    {
        var packet = _testHelper.GetL2Packet("l2-1.pcap", 2);
        var loginOkPacket = _decryptor.DecryptPacket<LoginOkPacket>(packet.Item1);

        Assert.That(loginOkPacket.PacketType, Is.EqualTo(LoginServerPacketTypes.LoginOk));
        Assert.That(loginOkPacket.Length, Is.EqualTo(50));
        Assert.That(loginOkPacket.Bytes.Length, Is.EqualTo(loginOkPacket.Length));
        Assert.That(loginOkPacket.SessionKey1, Is.EqualTo(0x00017bb1));
        Assert.That(loginOkPacket.SessionKey2, Is.EqualTo(0x1bfc47b1));
    }

    [Test]
    public void TestPlayOkPacket()
    {
        var p = _testHelper.GetL2Packet("l2-1.pcap", 6);
        var playOkPacket = _decryptor.DecryptPacket<PlayOkPacket>(p.Item1);

        Assert.That(playOkPacket.PacketType, Is.EqualTo(LoginServerPacketTypes.PlayOk));
        Assert.That(playOkPacket.Length, Is.EqualTo(26));
        Assert.That(playOkPacket.Bytes.Length, Is.EqualTo(playOkPacket.Length));
        Assert.That(playOkPacket.SessionKey1, Is.EqualTo(0x2030));
        Assert.That(playOkPacket.SessionKey2, Is.EqualTo(0x0000));
    }

    [Test]
    public void TestServerListPacket()
    {
        var p = _testHelper.GetL2Packet("l2-1.pcap", 4);
        var serverListPacket = _decryptor.DecryptPacket<ServerListPacket>(p.Item1);

        Assert.That(serverListPacket.PacketType, Is.EqualTo(LoginServerPacketTypes.ServerList));
        Assert.That(serverListPacket.Length, Is.EqualTo(58));
        Assert.That(serverListPacket.Servers.Count, Is.EqualTo(2));

        var s1 = serverListPacket.Servers[0];
        Assert.That(s1.Id, Is.EqualTo(0x01));
        Assert.That(s1.Ip, Is.EqualTo(0xdc63a653));
        Assert.That(s1.Port, Is.EqualTo(0x00001e61));
        Assert.That(s1.AgeLimit, Is.EqualTo(0x00));
        Assert.That(s1.IsPvp, Is.EqualTo(0x01));
        Assert.That(s1.Online, Is.EqualTo(0x00ef));
        Assert.That(s1.MaxOnline, Is.EqualTo(0x03e8));
        Assert.That(s1.IsTest, Is.EqualTo(0x01));

        var s2 = serverListPacket.Servers[1];
        Assert.That(s2.Id, Is.EqualTo(0x00));
        Assert.That(s2.Ip, Is.EqualTo(0x15000000));
        Assert.That(s2.Port, Is.EqualTo(0x00000a02));
        Assert.That(s2.AgeLimit, Is.EqualTo(0x63));
        Assert.That(s2.IsPvp, Is.EqualTo(0x61));
        Assert.That(s2.Online, Is.EqualTo(0x001e));
        Assert.That(s2.MaxOnline, Is.EqualTo(0x0000));
        Assert.That(s2.IsTest, Is.EqualTo(0x01));
    }
}