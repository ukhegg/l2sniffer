using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets.LC;

namespace L2sniffer.L2PacketHandlers;

public class LoginClientPacketHandler : L2PacketHandlerBase<LoginClientPacketBase>
{
    public LoginClientPacketHandler(StreamId streamId,
                                    IPacketDecryptorProvider packetDecryptorProvider,
                                    IL2PacketLogger packetLogger)
        : base(packetLogger, packetDecryptorProvider,streamId)
    {
    }

    protected override void ProcessPacket(LoginClientPacketBase packet)
    {
    }

    protected override IL2PacketDecryptor GetDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetLoginSessionDecryptor(streamId);
    }
}