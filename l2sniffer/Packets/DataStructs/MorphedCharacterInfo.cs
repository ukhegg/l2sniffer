namespace L2sniffer.Packets.DataStructs;

public class MorphedCharacterInfo : DataStruct
{
    public uint ObjectId;
    public uint NpcTypeId;
    public uint PositiveCarma;
    public Coordinates3d? Coordinates;

    public uint Heading;

    //skip uint
    public uint MAttackSpeed;
    public uint PAttackSpeed;
    public MovementSpeeds MovementSpeeds;
    public double AttackSpeedMultiplier;
    public double CollisionRadius;
    public double CollisionHeight;

    public uint RightHandWeapon;

    //skip uint
    public uint LeftHandWeapon;

    //skip byte
    public byte IsRunning;
    public byte IsInCombat;
    public byte IsAlikeDead;
    public byte InvisibleStatus;
    public string Name;

    public string Title;

    //skip 3 uint
    public ushort AbnormalEffect;
    //skip 19 bytes

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out ObjectId);
        reader.Read(out NpcTypeId);
        reader.Read(out PositiveCarma);
        reader.Read(out Coordinates);
        reader.Read(out Heading);
        reader.Read(out uint _);
        reader.Read(out MAttackSpeed);
        reader.Read(out PAttackSpeed);
        reader.Read(out MovementSpeeds);
        reader.Read(out AttackSpeedMultiplier);
        reader.Read(out CollisionRadius);
        reader.Read(out CollisionHeight);
        reader.Read(out RightHandWeapon);
        reader.Read(out uint _);
        reader.Read(out LeftHandWeapon);
        reader.Read(out byte nameAboveCharacter);
        reader.Read(out IsRunning);
        reader.Read(out IsInCombat);
        reader.Read(out IsAlikeDead);
        reader.Read(out InvisibleStatus);
        reader.Read(out Name, FieldsReader.StringType.Ansii);
        reader.Read(out Title, FieldsReader.StringType.Ansii);
        reader.Skip(3 * sizeof(uint));
        reader.Read(out AbnormalEffect);
        reader.Skip(19);
    }
}