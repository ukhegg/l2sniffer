namespace L2sniffer.Packets.DataStructs;

public enum ShortcutTypes : uint
{
    Item = 1,
    Skill = 2,
    Action = 3,
    Macro = 4,
    Recipe = 5,
}

public class L2Shortcut : DataStruct
{
    public ShortcutTypes Type;
    public uint SlotPage;
    public uint Slot;
    public uint Id;
    public uint Level;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.ReadEnum(out Type);
        reader.Read(out uint slotAbs);
        SlotPage = slotAbs / 12;
        Slot = slotAbs % 12;
        reader.Read(out Id);
        if (Type == ShortcutTypes.Skill)
        {
            reader.Read(out Level);
        }

        reader.Read(out uint unknown);
    }
}