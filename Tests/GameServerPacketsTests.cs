using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.DataStructs;
using L2sniffer.Packets.GS;

namespace Tests;

public class GameServerPacketsTests
{
    private readonly TestHelper _testHelper = new TestHelper();
    private IL2PacketDecryptor _decryptor;
    private readonly StreamId _gameServerStream = StreamId.From("83.166.99.220", 7777).To("192.168.1.101", 53585);

    [SetUp]
    public void SetUp()
    {
        _decryptor = new GameSessionDecryptor(0x8e620000);
    }

    [Test]
    public void TestCanParseCryptInitPacket()
    {
        var packetBytes = _testHelper.GetL2Packet("l2-1.pcap", _gameServerStream, 0);
        Assert.That(packetBytes.Length, Is.EqualTo(16));

        var p = new CryptInitPacket(packetBytes);
        Assert.That(p.PacketType, Is.EqualTo(GameServerPacketTypes.CryptInit));
        Assert.That(p.XorKey, Is.EqualTo(0x8e620000));
    }

    [Test]
    public void TestCanParseMoveToLocationPacket()
    {
        var bytes = _testHelper.GetL2Packet("l2-1.pcap", _gameServerStream, _decryptor, 22);
        Assert.That(bytes.Length, Is.EqualTo(31));
        var packet = new MoveToLocationPacket(bytes);

        Assert.That(packet.PacketType, Is.EqualTo(GameServerPacketTypes.MoveToLocation));
        Assert.That(packet.Dst, Is.EqualTo(new Coordinates3d() { X = 46348, Y = 175201, Z = -4976 }));
        Assert.That(packet.Current, Is.EqualTo(new Coordinates3d() { X = 46311, Y = 175126, Z = -4976 }));
    }

    [Test]
    public void TestCanParseCharListPacket()
    {
        var bytes = _testHelper.GetL2Packet("l2-1.pcap", _gameServerStream, _decryptor, 1);
        Assert.That(bytes.Count, Is.EqualTo(324));

        var packet = new CharListPacket(bytes);
        Assert.That(packet.PacketType, Is.EqualTo(GameServerPacketTypes.CharList));
        Assert.That(packet.Characters.Length, Is.EqualTo(1));

        Assert.That(packet.Characters[0].Name, Is.EqualTo("spoiliness"));
        Assert.That(packet.Characters[0].Login, Is.EqualTo("uspoil"));
        Assert.That(packet.Characters[0].Login, Is.EqualTo("uspoil"));
        Assert.That(packet.Characters[0].CoordinatedUnused,
                    Is.EqualTo(new Coordinates3d() { X = 48483, Y = 176526, Z = -4976 }));
        Assert.That(packet.Characters[0].Hp, Is.EqualTo(2639.2019999999998));
        Assert.That(packet.Characters[0].Mp, Is.EqualTo(419.70145000000554));
        Assert.That(packet.Characters[0].Sp, Is.EqualTo(0x0000ae8e));
        Assert.That(packet.Characters[0].BaseKlassId, Is.EqualTo(0x00000037));
        Assert.That(packet.Characters[0].LastLogin, Is.EqualTo(0x01));
        Assert.That(packet.Characters[0].AugLevel, Is.EqualTo(2));
    }

    [Test]
    public void TestCharacterInfoPacket()
    {
        var bytes = _testHelper.GetL2Packet("l2-1.pcap", _gameServerStream, _decryptor, 14);
        Assert.That(bytes.Count, Is.EqualTo(182));

        var packet = new CharacterInfoPacket(bytes);
        Assert.That(packet.PacketType, Is.EqualTo(GameServerPacketTypes.NpcOrCharacterInfo));

        Assert.That(packet.CharacterInfo.Name, Is.Empty);
        Assert.That(packet.CharacterInfo.MovementSpeeds.RunSpeed, Is.EqualTo(150));
    }

    [Test]
    public void TestOtherCharacterInfoPacket()
    {
        var bytes = _testHelper.GetL2Packet("l2-1.pcap", _gameServerStream, _decryptor, 14);
        Assert.That(bytes.Count, Is.EqualTo(182));

        var packet = new CharacterInfoPacket(bytes);
        Assert.That(packet.PacketType, Is.EqualTo(GameServerPacketTypes.NpcOrCharacterInfo));

        Assert.That(packet.CharacterInfo.Name, Is.Empty);
        Assert.That(packet.CharacterInfo.MovementSpeeds.RunSpeed, Is.EqualTo(150));
    }

    [Test]
    public void TestCanReadItemsListPacket()
    {
        var bytes = _testHelper.GetL2Packet("l2-1.pcap", _gameServerStream, _decryptor, 88);
        Assert.That(bytes.Count, Is.EqualTo(2303));

        var packet = new ItemsListPacket(bytes);
        Assert.That(packet.PacketType, Is.EqualTo(GameServerPacketTypes.ItemsList));
        
        Assert.That(packet.Items.ShowWindow, Is.EqualTo(0x00));
        Assert.That(packet.Items.Items.Length, Is.EqualTo(82));

        Assert.That(packet.Items.Items[0].ItemId, Is.EqualTo(2074));
        Assert.That(packet.Items.Items[0].ItemCount, Is.EqualTo(1));
    }
}