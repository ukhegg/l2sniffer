using l2sniffer.PacketHandlers;

namespace L2sniffer.Crypto;

public interface IPacketDecryptorProvider
{
    IL2PacketDecryptor GetGameSessionDecryptor(StreamId gameStreamId);

    IL2PacketDecryptor? GetLoginSessionDecryptor(StreamId loginStreamId);
}