using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

public enum UpdateAttributeTypes : uint
{
    Level = 0x01,
    Exp = 0x02,
    Str = 0x03,
    Dex = 0x04,
    Con = 0x05,
    Int = 0x06,
    Wit = 0x07,
    Men = 0x08,
    CurHp = 0x09,
    MaxHp = 0x0a,
    CurMp = 0x0b,
    MaxMp = 0x0c,
    Sp = 0x0d,
    CurLoad = 0x0e,
    MaxLoad = 0x0f,
    PAttack = 0x11,
    AttackSpeed = 0x12,
    PDef = 0x13,
    Evasion = 0x14,
    Accuracy = 0x15,
    Critical = 0x16,
    MAttack = 0x17,
    CastSpeed = 0x18,
    MDef = 0x19,
    PvpFlag = 0x1a,
    Karma = 0x1b,
    CurCp = 0x21,
    MaxCp = 0x22
}

public class UpdateAttribute : DataStruct
{
    public UpdateAttributeTypes Type;
    public uint NewValue;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.ReadEnum(out Type);
        reader.Read(out NewValue);
    }
}

public class CharStatusUpdate : DataStruct
{
    public GameObjectId ObjectId;
    public UpdateAttribute[] Attributes;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out ObjectId);
        reader.Read(out uint count);
        Attributes = new UpdateAttribute[count];
        for (var i = 0; i < count; ++i)
        {
            reader.Read(out UpdateAttribute attribute);
            Attributes[i] = attribute;
        }
    }
}