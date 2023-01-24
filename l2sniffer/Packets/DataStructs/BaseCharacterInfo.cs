using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

public class BaseCharacterInfo : DataStruct
{
    public Coordinates3d Coordinates = null!;
    public uint Heading;
    public GameObjectId ObjectId;
    public string Name;
    public uint Race;
    public uint Sex;
    public uint ClassOrBaseClassId;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Coordinates);
        reader.Read(out Heading);
        reader.Read(out ObjectId);
        reader.Read(out Name);
        reader.Read(out Race);
        reader.Read(out Sex);
        reader.Read(out ClassOrBaseClassId);
    }
}