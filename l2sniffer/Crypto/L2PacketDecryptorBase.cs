using L2sniffer.Packets;

namespace L2sniffer.Crypto;

public abstract class L2PacketDecryptorBase : IL2PacketDecrypt
{
    public L2PacketBase DecryptPacket(L2PacketBase packet)
    {
        var result = packet.Bytes.ToArray();
        var encryptedBytes = new Span<byte>(result)[2..];
        DecryptInplace(encryptedBytes);
        return new L2PacketBase(result);
    }

    protected abstract void DecryptInplace(Span<byte> payloadBytes);
}