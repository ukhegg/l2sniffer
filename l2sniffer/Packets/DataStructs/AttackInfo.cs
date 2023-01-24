using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

[Flags]
public enum AttackFlags : byte
{
    Soulshot = 0x10,
    Crit = 0x20,
    Shield = 0x40,
    Miss = 0x80
}

public class Hit : DataStruct
{
    public GameObjectId TargetId;
    public uint Damage;
    public AttackFlags Flags;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out TargetId);
        reader.Read(out Damage);
        reader.ReadEnum(out Flags);
    }
}


public class AttackInfo : DataStruct
{
    public GameObjectId AttackerId;
    public Hit FirstHit;
    public Coordinates3d Coordinates;
    public Hit[] Hits;
    
    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out AttackerId);
        reader.Read(out FirstHit);
        reader.Read(out Coordinates);
        reader.Read(out ushort hitsCount);
        Hits = new Hit[hitsCount];
        for (var i = 0; i < hitsCount; ++i)
        {
            reader.Read(out Hit hit);
            Hits[i] = hit;
        }
    }
}