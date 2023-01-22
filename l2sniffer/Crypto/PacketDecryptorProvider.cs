using l2sniffer.PacketHandlers;

namespace L2sniffer.Crypto;

public class PacketDecryptorProvider : IPacketDecryptorProvider,
    ISessionCryptKeysRegistry
{
    private Dictionary<StreamId, uint> _sessionKeys = new Dictionary<StreamId, uint>();

    public IL2PacketDecryptor GetGameSessionDecryptor(StreamId gameStreamId)
    {
        if (!_sessionKeys.TryGetValue(gameStreamId, out uint key))
        {
            throw new Exception("game session key not found");
        }

        return new GameSessionDecryptor(key);
    }

    public IL2PacketDecryptor? GetLoginSessionDecryptor(StreamId loginStreamId)
    {
        return new LoginSessionDecryptor();
    }

    public void RegisterGameSessionKey(StreamId gameStreamId, uint key)
    {
        _sessionKeys[gameStreamId] = key;
        _sessionKeys[gameStreamId.Reverse()] = key;
    }
}