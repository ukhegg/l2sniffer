using L2sniffer;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.GS;

namespace Tests;

public class GameServerPacketsTests
{
    private TestHelper _testHelper = new TestHelper();

    [Test]
    public void TestCanParseCryptInitPacket()
    {
        var packetBytes = _testHelper.GetL2Packet("l2-1.pcap",
                                                  StreamId.From("83.166.99.220", 7777).To("192.168.1.101", 53585),
                                                  0);
        Assert.That(packetBytes.Length, Is.EqualTo(16));
        
        var p = new CryptInitPacket(packetBytes);
        Assert.That(p.PacketType, Is.EqualTo(GameServerPacketTypes.CryptInit));
        Assert.That(p.XorKey, Is.EqualTo(0x8e620000));
    }
}