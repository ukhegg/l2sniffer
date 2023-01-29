using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;

namespace BotAgent;

public class GameSessionDecryptorProvider : IDecryptorProvider
{
    private IPacketDecryptorProvider _universalDecryptor;

    public GameSessionDecryptorProvider(IPacketDecryptorProvider universalDecryptor)
    {
        _universalDecryptor = universalDecryptor;
    }

    public IL2PacketDecryptor GetDecryptor(StreamId streamId)
    {
        return _universalDecryptor.GetGameSessionDecryptor(streamId);
    }
}