using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;
using L2sniffer.Packets;
using L2sniffer.Packets.LC;

namespace L2sniffer.L2PacketHandlers;

public class LoginClientPacketHandler : L2PacketHandlerBase<LoginClientPacketBase, LoginClientPacketTypes>
{
    public LoginClientPacketHandler(StreamId streamId,
                                    IPacketDecryptorProvider packetDecryptorProvider,
                                    IL2PacketLogger packetLogger)
        : base(packetLogger, packetDecryptorProvider, streamId)
    {
    }

    protected override void RegisterHandlers(IHandlersRegistry handlersRegistry)
    {
        
    }

    protected override IL2PacketDecryptor SelectDecryptor(IPacketDecryptorProvider decryptorProvider, StreamId streamId)
    {
        return decryptorProvider.GetLoginSessionDecryptor(streamId);
    }

    protected override void ProcessUnhandledPacket(LoginClientPacketBase packet, PacketMetainfo metainfo)
    {
        
    }
}