using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

public enum WaitTypes : uint
{
    Sitting = 0x00,
    Standing = 0x01,
    FakeDeathStart = 0x03,
    FakeDeathStop = 0x04
}

public class ChangeWaitTypeData : DataStruct
{
    public GameObjectId ObjectId;
    public WaitTypes WaitType;
    public Coordinates3d Coordinates;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out ObjectId);
        reader.ReadEnum(out WaitType);
        reader.Read(out Coordinates);
    }
}