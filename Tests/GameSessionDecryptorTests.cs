using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.Packets.GS;

namespace Tests;

public class GameSessionDecryptorTests
{
    private TestHelper _testHelper = new TestHelper();
    private readonly StreamId _gameServerStream = StreamId.From("83.166.99.220", 7777).To("192.168.1.101", 53585);

    [Test]
    public void TestCanDecryptGameSessionPacket()
    {
        var p = _testHelper.GetL2Packet("l2-1.pcap", 10);
        var decryptor = new GameSessionDecryptor(0x8e620000);
        var decryptedPacket = decryptor.DecryptPacket<L2PacketBase>(p.Item1);
        Assert.That(decryptedPacket.PacketTypeRaw, Is.EqualTo(19));
    }

    [Test]
    public void TestCanDecryptPacketsSequence()
    {
        var decryptor = new GameSessionDecryptor(0x8e620000);
        var packets = _testHelper.GetL2Packets("l2-1.pcap", _gameServerStream);
        var decrypted = new byte[packets.Count][];
        for (var i = 0; i < packets.Count; ++i)
        {
            decrypted[i] = i == 0 ? packets[i] : decryptor.DecryptPacket(packets[i]);
        }

        Assert.That(decrypted[0][2], Is.EqualTo((byte)GameServerPacketTypes.CryptInit));
        Assert.That(decrypted[1][2], Is.EqualTo((byte)GameServerPacketTypes.CharList));
        Assert.That(decrypted[2][2], Is.EqualTo((byte)GameServerPacketTypes.SignsSky));
        Assert.That(decrypted[3][2], Is.EqualTo((byte)GameServerPacketTypes.CharSelected));
        Assert.That(decrypted[4][2], Is.EqualTo((byte)GameServerPacketTypes.ExtendedCodes));
        Assert.That(decrypted[5][2], Is.EqualTo((byte)GameServerPacketTypes.QuestList));
    }
}