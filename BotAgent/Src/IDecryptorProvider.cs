using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;

namespace BotAgent;

public interface IDecryptorProvider
{
    IL2PacketDecryptor GetDecryptor(StreamId streamId);
}