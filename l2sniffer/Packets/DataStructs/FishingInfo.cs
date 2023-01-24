namespace L2sniffer.Packets.DataStructs;

public class FishingInfo : DataStruct
{
    public byte IsFishing;
    public Coordinates3d FishingCoordinats;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out IsFishing);
        reader.Read(out FishingCoordinats);
    }
}