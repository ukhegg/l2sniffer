using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Packets.DataStructs;

public class OtherUserInfo : DataStruct
{
    public Coordinates3d Coordinates;
    public int Heading;
    public GameObjectId ObjectId;
    public string Name;
    public RaceTypes Race;
    public uint Sex;
    public CharacterClassIds ClassOrBaseClassId;
    public VisibleEquipment VisibleEquipment;
    public uint PvpFlag;
    public uint Karma;
    public uint MAttackSpeed;
    public uint PAttackSpeed;
    public MovementSpeedsEx MovementSpeedsEx;
    public double AttackSpeedMultiplier;
    public CollisionInfo CollisionInfo;
    public uint HairStyle;
    public uint HairColor;
    public uint FaceType;
    public ClanAndAllyInfo ClanAndAlly;
    public uint SiegeFlags;

    public byte IsSitting;
    public byte IsRunning;
    public byte IsInCombat;
    public byte IsAlikeDead;

    public byte IsInvisible;
    public byte MountType;
    public byte PrivateStoreType; //1-sellshop;
    public CubicInfo CubicInfo;

    public byte SearchForParty;
    public uint AbnormalEffect;
    public byte Unused;
    public ushort RecommendationsCount;
    public uint Unused2;

    public uint Unknown1;
    public uint Unknown2;

    public byte EnchantEffect;
    public byte Unknown3;
    public uint ClanCrestLargeId;
    public byte HeroSymbol;
    public byte HeroAura;
    public FishingInfo FishingInfo;
    public uint NameColor;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out Coordinates);
        reader.Read(out Heading);
        reader.Read(out ObjectId);
        reader.Read(out Name);
        reader.ReadEnum(out Race);
        reader.Read(out Sex);
        reader.ReadEnum(out ClassOrBaseClassId);
        reader.Read(out VisibleEquipment);
        reader.Read(out PvpFlag);
        reader.Read(out Karma);
        reader.Read(out MAttackSpeed);
        reader.Read(out PAttackSpeed);
        reader.Read(out MovementSpeedsEx);
        reader.Read(out AttackSpeedMultiplier);
        reader.Read(out CollisionInfo);
        reader.Read(out HairStyle);
        reader.Read(out HairColor);
        reader.Read(out FaceType);
        reader.Read(out ClanAndAlly);
        reader.Read(out SiegeFlags);

        reader.Read(out IsSitting);
        reader.Read(out IsRunning);
        reader.Read(out IsInCombat);
        reader.Read(out IsAlikeDead);

        reader.Read(out IsInvisible);
        reader.Read(out MountType);
        reader.Read(out PrivateStoreType);
        reader.Read(out CubicInfo);

        reader.Read(out SearchForParty);
        reader.Read(out AbnormalEffect);
        reader.Read(out Unused);
        reader.Read(out RecommendationsCount);
        reader.Read(out Unused2);

        reader.Read(out Unknown1);
        reader.Read(out Unknown2);

        reader.Read(out EnchantEffect);
        reader.Read(out Unknown3);
        reader.Read(out ClanCrestLargeId);
        reader.Read(out HeroSymbol);
        reader.Read(out HeroAura);
        reader.Read(out FishingInfo);
        reader.Read(out NameColor);
    }
}