using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace L2sniffer.Crypto;

public class LoginSessionDecryptor : L2PacketDecryptorBase
{
    private IBlockCipher _cipher;

    private static byte[] _cipherKey = new byte[]
    {
        0x5F, 0x3B, 0x35, 0x2E, 0x5D, 0x39, 0x34, 0x2D,
        0x33, 0x31, 0x3D, 0x3D, 0x2D, 0x25, 0x78, 0x54,
        0x21, 0x5E, 0x5B, 0x24, 0x00
    };

    public LoginSessionDecryptor()
    {
        var engine = new BlowfishEngine();
        engine.Init(false, new KeyParameter(_cipherKey));
        _cipher = new EcbBlockCipher(engine);
    }

    protected override void DecryptInplace(Span<byte> payloadBytes)
    {
        var blocksCount = payloadBytes.Length / 8;
        int offset = 0;
        for (var i = 0; i < blocksCount; ++i)
        {
            var blockEnd = offset + 8;
            var block = payloadBytes[offset..blockEnd];

            block[0..4].Reverse();
            block[4..8].Reverse();
            _cipher.ProcessBlock(block, block);
            block[0..4].Reverse();
            block[4..8].Reverse();

            offset += 8;
        }
    }

    public byte[] Decrypt(ReadOnlySpan<byte> encryptedBytes)
    {
        var result = encryptedBytes.ToArray();
        DecryptInplace(result);
        return result;
    }
}