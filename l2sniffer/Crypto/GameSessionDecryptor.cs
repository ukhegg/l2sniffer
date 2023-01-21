namespace L2sniffer.Crypto;

public class GameSessionDecryptor : L2PacketDecryptorBase
{
    private byte[] _xorKey =
    {
        0x94, 0x35, 0x00, 0x00,
        0xa1, 0x6c, 0x54, 0x87
    };

    public GameSessionDecryptor(uint key)
    {
        var keyBytes = BitConverter.GetBytes(key);
        for (var i = 0; i < 4; ++i)
        {
            _xorKey[i] = keyBytes[3 - i];
        }
    }

    protected override void DecryptInplace(Span<byte> bytes)
    {
        var sz = bytes.Length;
        int temp = 0;
        for (int i = 0; i < sz; i++)
        {
            int temp2 = bytes[i];
            bytes[i] = (byte)(temp2 ^ _xorKey[i & 7] ^ temp);
            temp = temp2;
        }

        uint old = BitConverter.ToUInt32(_xorKey);
        old = (uint)(old + sz);
        var newBytes = BitConverter.GetBytes(old);
        Array.Copy(newBytes, 0, _xorKey, 0, 4);
    }
}