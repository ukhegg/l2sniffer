using l2sniffer.PacketHandlers;

namespace L2sniffer.Crypto;

public interface ISessionCryptKeysRegistry
{
    void RegisterGameSessionKey(StreamId gameStreamId, uint key);
}