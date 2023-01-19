namespace L2sniffer;

class ByteArrayCompare : IComparer<byte[]>
{
    public static int StaticCompare(byte[]? b1, byte[]? b2)
    {
        if (b1 == b2) return 0;
        if (b1 == null) return -1;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] < b2[i])
            {
                return -1;
            }

            if (b1[i] > b2[i])
            {
                return 1;
            }
        }

        return 0;
    }

    public int Compare(byte[]? b1, byte[]? b2)
    {
        return StaticCompare(b1, b2);
    }
}