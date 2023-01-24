namespace L2sniffer.Packets.DataStructs;

public class MovementSpeeds: DataStruct
{
    public uint RunSpeed;
    public uint WalkSpeed;
    public uint SwimRunSpeed;
    public uint SwimWalkSpeed;
    public uint FlyRunSpeed;
    public uint FlyWalkSpeed;
    public double MovementSpeedMultiplier;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out RunSpeed);
        reader.Read(out WalkSpeed);
        reader.Read(out SwimRunSpeed);
        reader.Read(out SwimWalkSpeed);
        reader.Read(out FlyRunSpeed);
        reader.Read(out FlyWalkSpeed);
        reader.Read(out MovementSpeedMultiplier);
    }
}

public class MovementSpeedsEx : MovementSpeeds
{
    public uint FlRunSpeed;
    public uint FlWalkSpeed;
    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out RunSpeed);
        reader.Read(out WalkSpeed);
        reader.Read(out SwimRunSpeed);
        reader.Read(out SwimWalkSpeed);
        reader.Read(out FlRunSpeed);
        reader.Read(out FlWalkSpeed);
        reader.Read(out FlyRunSpeed);
        reader.Read(out FlyWalkSpeed);
        reader.Read(out MovementSpeedMultiplier);
    }
} 