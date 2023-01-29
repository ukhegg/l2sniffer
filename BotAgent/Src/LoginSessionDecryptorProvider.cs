using L2sniffer.Crypto;
using l2sniffer.PacketHandlers;

namespace BotAgent;

public class LoginSessionDecryptorProvider : IDecryptorProvider
{
    private IPacketDecryptorProvider _universaleDecryptor;

    public LoginSessionDecryptorProvider(IPacketDecryptorProvider universaleDecryptor)
    {
        _universaleDecryptor = universaleDecryptor;
    }

    public IL2PacketDecryptor GetDecryptor(StreamId streamId)
    {
        return _universaleDecryptor.GetLoginSessionDecryptor(streamId);
    }
}