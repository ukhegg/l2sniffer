using L2sniffer.Crypto;
using L2sniffer.Packets;

namespace Tests;

public class GameSessionDecryptorTests
{
    private TestHelper _testHelper = new TestHelper();

    [Test]
    public void TestCanDecryptGameSessionPacket()
    {
        var p = _testHelper.GetL2Packet("l2-1.pcap", 10);
        var decryptor = new GameSessionDecryptor(0x8e620000);
        var decryptedPacket = decryptor.DecryptPacket<L2PacketBase>(p.Item1);
        Assert.That(decryptedPacket.PacketTypeRaw, Is.EqualTo(19));
    }
}