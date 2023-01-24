using System.Runtime.CompilerServices;
using L2sniffer.Packets;

namespace L2sniffer.Crypto;

public interface IL2PacketDecryptor
{
    byte[] DecryptPacket(ReadOnlySpan<byte> packetBytes);
}

public static class L2PacketDecryptorExtension
{
    public static T DecryptPacket<T>(this IL2PacketDecryptor? decryptor, ReadOnlySpan<byte> packetBytes)
        where T : L2PacketBase
    {
        byte[] decryptedBytes = decryptor.DecryptPacket(packetBytes);
        return (T)Activator.CreateInstance(typeof(T), decryptedBytes);
    }
}