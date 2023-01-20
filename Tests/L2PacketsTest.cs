using L2sniffer.Packets.LS;

namespace Tests;

public class L2PacketsTest
{
    private TestHelper _testHelper = new TestHelper();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CanParseLoginInitPacket()
    {
        var packet = _testHelper.GetL2Packet("l2-1.pcap", 0);
        var initPacket = new InitPacket(packet.Item1);
        Assert.That(initPacket.Length, Is.EqualTo(11));
        Assert.That(initPacket.Bytes.Length, Is.EqualTo(initPacket.Length));
        Assert.That(initPacket.SessionId, Is.EqualTo(0x1bfc47b1));
        Assert.That(initPacket.ProtocolVersion, Is.EqualTo(0x0000785a));
    }
}