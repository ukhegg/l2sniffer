namespace L2sniffer.Packets.DataStructs;

public class Accessories : DataStruct
{
    public uint Under; //writeD(_cha.getInventory().getPaperdollObjectId(Inventory.PAPERDOLL_UNDER));
    public uint RightEar; //writeD(_cha.getInventory().getPaperdollObjectId(Inventory.PAPERDOLL_REAR));
    public uint LeftEar; //writeD(_cha.getInventory().getPaperdollObjectId(Inventory.PAPERDOLL_LEAR));
    public uint Neck; //writeD(_cha.getInventory().getPaperdollObjectId(Inventory.PAPERDOLL_NECK));
    public uint RightFinger; //writeD(_cha.getInventory().getPaperdollObjectId(Inventory.PAPERDOLL_RFINGER));
    public uint LeftFinger; //writeD(_cha.getInventory().getPaperdollObjectId(Inventory.PAPERDOLL_LFINGER));

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Under);
        reader.Read(out RightEar);
        reader.Read(out LeftEar);
        reader.Read(out Neck);
        reader.Read(out RightFinger);
        reader.Read(out LeftFinger);
    }
}