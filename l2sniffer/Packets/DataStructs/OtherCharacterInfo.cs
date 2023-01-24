namespace L2sniffer.Packets.DataStructs;

public class OtherCharacterInfo : DataStruct
{
    public BaseCharacterInfo BaseInfo;
    public VisibleEquipment VisibleEquipment;
    public uint PvpFlag;
    public uint Carma;
    public uint MAttackSpeed;
    public uint PAttackSpeed;
    public MovementSpeeds MovementSpeeds;
    public double AttackSpeedMultiplier;
    public double CollisionRadium;
    public double CollisionHeight;
    public uint HairStyle;
    public uint HairColor;
    public uint FaceType;
    public string Title;
    public uint ClanId;
    public uint ClanCrestId;
    public uint AllyId;
    public uint AllyCrestId;
    public byte IsSitting;
    public byte IsRunning;
    public byte IsInCombat;
    public byte IsAlikeDead;
    public byte IsInvisible;
    public byte MountType;
    public byte PrivateStoreType;
    public CubicInfo Cubics;
    public uint AbnormalEffect;
    public ushort RecommendationLevel;
    public byte MountedEnchantEffect;
    public uint ClanCrestLargeId;
    public byte IsFishing;
    public Coordinates3d FishCoordinates;
    public uint NameColor;

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out BaseInfo);
        reader.Read(out uint underwear);
        reader.Read(out VisibleEquipment);
        reader.Read(out PvpFlag);
        reader.Read(out Carma);
        reader.Read(out MAttackSpeed);
        reader.Read(out PAttackSpeed);
        reader.Read(out PvpFlag);
        reader.Read(out Carma);
        reader.Read(out MovementSpeeds);
        reader.Read(out AttackSpeedMultiplier);
        reader.Read(out CollisionRadium);
        reader.Read(out CollisionHeight);
        reader.Read(out HairStyle);
        reader.Read(out HairColor);
        reader.Read(out FaceType);
        reader.Read(out Title);
        reader.Read(out ClanId);
        reader.Read(out ClanCrestId);
        reader.Read(out AllyId);
        reader.Read(out AllyCrestId);
        reader.Read(out uint siegeFlags);
        reader.Read(out IsSitting);
        reader.Read(out IsRunning);
        reader.Read(out IsInCombat);
        reader.Read(out IsAlikeDead);
        reader.Read(out IsInvisible);
        reader.Read(out MountType);
        reader.Read(out PrivateStoreType);
        reader.Read(out Cubics);
        reader.Read(out byte findPartyMembers);
        reader.Read(out AbnormalEffect);
        reader.Read(out byte _);
        reader.Read(out RecommendationLevel);
        reader.Read(out uint _);
        reader.Read(out uint _);
        reader.Read(out uint _);
        reader.Read(out MountedEnchantEffect);
        reader.Read(out byte _);
        reader.Read(out ClanCrestLargeId);
        reader.Read(out byte _);
        reader.Read(out byte heroAura);
        reader.Read(out IsFishing);
        reader.Read(out FishCoordinates);
        reader.Read(out NameColor);
    }
}