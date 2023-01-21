using L2sniffer.Crypto;
using L2sniffer.Packets;
using PacketDotNet;

namespace Tests;

public class LoginSessionCipherTests
{
    private TestHelper _testHelper = new TestHelper();

    [Test]
    public void CanParseLoginInitPacket()
    {
        var packet = _testHelper.GetL2Packet("l2-1.pcap", 2);
        var l2Packet = new L2PacketBase(packet.Item1);


        var cipher = new LoginSessionDecryptor();
        var decrypted = cipher.Decrypt(l2Packet.PayloadBytes.ToArray());

        Assert.That(
            decrypted, Is.EqualTo(new[]
            {
                0x03, 0xb1, 0x7b, //type
                0x01, 0x00, 0xb1, 0x47, 0xfc, 0x1b,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x30, 0x00, 0x38, 0x18, 0x31, 0x3c, 0xc5, 0x00,
                0x34, 0x00, 0x2e
            })
        );
    }
}