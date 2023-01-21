using L2sniffer.Packets;

namespace L2sniffer.Crypto;

public abstract class L2PacketDecryptorBase : IL2PacketDecryptor
{
    public byte[] DecryptPacket(ReadOnlySpan<byte> packetBytes)
    {
        var result = packetBytes.ToArray();
        DecryptInplace(new Span<byte>(result)[2..]);
        return result;
    }

    protected abstract void DecryptInplace(Span<byte> payloadBytes);
}