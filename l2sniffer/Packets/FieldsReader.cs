using System.Reflection;
using System.Runtime.InteropServices;
using L2sniffer.Utils;

namespace L2sniffer.Packets;

public ref struct FieldsReader
{
    public enum StringType
    {
        Ansii,
        Wide
    }

    private ReadOnlySpan<byte> _packetData;

    public FieldsReader(ReadOnlySpan<byte> packetData)
    {
        _packetData = packetData;
    }

    public void Read(out string val, StringType stringType = StringType.Wide)
    {
        var chars = new List<char>();
        ushort nextSymbol;
        Read(out nextSymbol);
        while (nextSymbol != 0)
        {
            chars.Add(Convert.ToChar(nextSymbol));
            Read(out nextSymbol);
        }

        val = new string(chars.ToArray());
    }

    public void Read(out char val)
    {
        val = (char)_packetData[0];
        _packetData = _packetData.Slice(1);
    }

    public void Read(ref char[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out byte val)
    {
        val = _packetData[0];
        _packetData = _packetData.Slice(1);
    }

    public void Read(ref byte[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out ushort val)
    {
        val = BitConverter.ToUInt16(_packetData);
        _packetData = _packetData[2..];
    }

    public void Read(ref ushort[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out short val)
    {
        val = BitConverter.ToInt16(_packetData);
        _packetData = _packetData[2..];
    }

    public void Read(ref short[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out uint val)
    {
        val = BitConverter.ToUInt32(_packetData);
        _packetData = _packetData[4..];
    }

    public void Read(ref uint[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out int val)
    {
        val = BitConverter.ToInt32(_packetData);
        _packetData = _packetData[4..];
    }

    public void Read(ref int[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out ulong val)
    {
        val = BitConverter.ToUInt64(_packetData);
        _packetData = _packetData[8..];
    }

    public void Read(ref ulong[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out long val)
    {
        val = BitConverter.ToInt64(_packetData);
        _packetData = _packetData[8..];
    }

    public void Read(ref long[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read(out double val)
    {
        val = BitConverter.ToDouble(_packetData);
        _packetData = _packetData[sizeof(double)..];
    }

    public void Read(ref double[] val)
    {
        for (var i = 0; i < val.Length; ++i)
        {
            Read(out val[i]);
        }
    }

    public void Read<T>(out T val) where T : DataStruct?, new()
    {
        val = new T();
        val.ReadFields(ref this);
    }

    public void Read<T>(T[] array) where T : DataStruct?, new()
    {
        if (array == null) throw new ArgumentException("array must not be null");
        for (int i = 0; i < array.Length; ++i)
        {
            Read(out array[i]);
        }
    }

    public void ReadEnum<T>(out T val) where T : Enum
    {
        if (Enum.GetUnderlyingType(typeof(T)) == typeof(byte))
        {
            byte _;
            Read(out _);
            val = (T)(object)_;
        }
        else if (Enum.GetUnderlyingType(typeof(T)) == typeof(ushort))
        {
            ushort _;
            Read(out _);
            val = (T)(object)_;
        }

        else if (Enum.GetUnderlyingType(typeof(T)) == typeof(uint))
        {
            uint _;
            Read(out _);
            val = (T)(object)_;
        }

        else if (Enum.GetUnderlyingType(typeof(T)) == typeof(ulong))
        {
            ulong _;
            Read(out _);
            val = (T)(object)_;
        }

        else if (Enum.GetUnderlyingType(typeof(T)) == typeof(char))
        {
            char _;
            Read(out _);
            val = (T)(object)_;
        }

        else if (Enum.GetUnderlyingType(typeof(T)) == typeof(short))
        {
            short _;
            Read(out _);
            val = (T)(object)_;
        }

        else if (Enum.GetUnderlyingType(typeof(T)) == typeof(int))
        {
            int _;
            Read(out _);
            val = (T)(object)_;
        }

        else if (Enum.GetUnderlyingType(typeof(T)) == typeof(long))
        {
            long _;
            Read(out _);
            val = (T)(object)_;
        }

        else throw new Exception("unknown enum underlying type");
    }

    public void ReadEnum<T>(ref T[] val) where T : Enum, new()
    {
        for (var i = 0; i < val.Length; ++i)
        {
            ReadEnum(out val[i]);
        }
    }


    public void Skip<T>() where T : unmanaged
    {
        Skip(Marshal.SizeOf<T>());
    }

    public void Skip(int bytes)
    {
        _packetData = _packetData[bytes..];
    }
}