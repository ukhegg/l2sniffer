using L2sniffer.Crypto;
using L2sniffer.Packets;
using L2sniffer.Packets.LS;

namespace Tests;

public class L2PacketsTest
{
    private TestHelper _testHelper = new TestHelper();
    private IL2PacketDecrypt _decryptor = new LoginSessionCipher();

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
        var loginOkPacket = _decryptor.DecryptPacket(new L2PacketBase(packet.Item1)).As<LoginOkPacket>();

        Assert.That(loginOkPacket.PacketType, Is.EqualTo(LoginServerPacketTypes.LoginOk));
        Assert.That(loginOkPacket.Length, Is.EqualTo(50));
        Assert.That(loginOkPacket.Bytes.Length, Is.EqualTo(loginOkPacket.Length));
        Assert.That(loginOkPacket.SessionKey1, Is.EqualTo(0x00017bb1));
        Assert.That(loginOkPacket.SessionKey2, Is.EqualTo(0x1bfc47b1));
    }
}