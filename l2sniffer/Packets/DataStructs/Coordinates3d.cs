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

    public static Coordinates3d operator +(Coordinates3d a, Coordinates3d b)
    {
        return new Coordinates3d() { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
    }

    public static Coordinates3d operator -(Coordinates3d a, Coordinates3d b)
    {
        return new Coordinates3d() { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
    }

    public double DistanceTo(Coordinates3d other)
    {
        var dx = other.X - X;
        var dy = other.Y - Y;
        var dz = other.Z - Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}