namespace L2sniffer.Packets.DataStructs;

public class Coordinates3d : DataStruct
{
    public int X;
    public int Y;
    public int Z;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out X);
        reader.Read(out Y);
        reader.Read(out Z);
    }

    public override string ToString()
    {
        return $"{X};{Y},{Z}";
    }

    protected bool Equals(Coordinates3d other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Coordinates3d)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}