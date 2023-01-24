namespace L2sniffer.Packets.DataStructs;

public class UserInfo : DataStruct
{
    public BaseCharacterInfo BaseInfo;
    public uint Level; //writeD(_cha.getLevel());

    public uint Exp; //writeD(_cha.getExp());

    public CharStats Stas;
    public uint MaxHp; //writeD(_cha.getMaxHp());
    public uint CurrentHp; //writeD((int) _cha.getCurrentHp());
    public uint MaxMp; //writeD(_cha.getMaxMp());
    public uint CurrentMp; //writeD((int) _cha.getCurrentMp());
    public uint Sp; //writeD(_cha.getSp());
    public uint CurrentLoad; //writeD(_cha.getCurrentLoad());
    public uint MaxLoad; //writeD(_cha.getMaxLoad());

    //writeD(0x28); // unknown
    public Accessories Accessories;
    public VisibleEquipment VisibleEquipment;
    public Accessories Accessories2;
    public VisibleEquipment VisibleEquipment2;

    public UserCombatStats CombatStats;
    public uint PvpFlag; //writeD(_cha.getPvpFlag()); // 0-non-pvp  1-pvp = violett name
    public uint Karma; //writeD(_cha.getKarma());

    public MovementSpeedsEx MovementSpeedsEx;
    public double AttackSpeedMultiplier; //writeF(_cha.getAttackSpeedMultiplier());

    public CollisionInfo PetOrCharCollisionInfo;

    public uint HairStyle; //writeD(_cha.getHairStyle());
    public uint HairColor; //writeD(_cha.getHairColor());
    public uint FaceType; //writeD(_cha.getFace());
    public uint BuilderLevel; //writeD((_cha.getAccessLevel() > 0) ? 1 : 0); // builder level 

    //String title = _cha.getTitle();
    //if (_cha.getInvisible() == 1 && _cha.isGM()) title = "Invisible";
    //if (_cha.getPoly().isMorphed())
    //    title += " - " + NpcTable.getInstance().getTemplate(_cha.getPoly().getPolyId()).name;
    public ClanAndAllyInfo ClanAndAllyInfo;
    public uint IsClanLeader; //writeD(_cha.isClanLeader() ? 0x60 : 0); // siege-flags  0x40 - leader rights  0x20 - ??

    public byte MountType; //writeC(_cha.getMountType()); // mount type

    public byte PrivateStoreType; //writeC(_cha.getPrivateStoreType());

    public byte HasDwarfenCraft; //writeC(_cha.hasDwarvenCraft() ? 1 : 0);

    public uint PkCount; //writeD(_cha.getPkKills());
    public uint PvpCount; //writeD(_cha.getPvpKills());

    public CubicInfo CubicInfo;

    public byte SearchForParty; //writeC(0x00); //1-find party members

    public uint AbnormalEffect; //writeD(_cha.getAbnormalEffect());
    //writeC(0x00);

    public uint ClanPrivileges; //writeD(_cha.getClanPrivileges());

    public uint Swim; //writeD(0x00);//writeD(0x100); //swim
    //writeD(0x00);
    //writeD(0x00);
    //writeD(0x00);
    //writeD(0x00);
    //writeD(0x00);
    //writeD(0x00);

    public ushort RecomendationsLeft; //writeH(_cha.getRecomLeft()); //c2  recommendations remaining

    public ushort RecomendationsCount; //writeH(_cha.getRecomHave()); //c2  recommendations received

    //writeD(0x00);
    public ushort InventoryLimit; //writeH(_cha.GetInventoryLimit());

    public CharacterClassIds ClassId; //writeD(_cha.getClassId().getId());

    //writeD(0x00); // special effects? circles around player...
    public uint MaxCp; //writeD(_cha.getMaxCp());
    public uint CurrentCp; //writeD((int) _cha.getCurrentCp());
    public byte EnchantEffect; //writeC(_cha.isMounted() ? 0 : _cha.getEnchantEffect());

    //writeC(0x00); //team circle around feet 1= Blue, 2 = red

    public uint ClanCrestLargeId; //writeD(_cha.getClanCrestLargeId());

    //writeC((_cha.isHero() || (_cha.isGM() && Config.GM_HERO_AURA)) ? 1 : 0); //0x01: symbol on char menu ctrl+I  
    public byte HeroAura; //writeC((_cha.isHero() || (_cha.isGM() && Config.GM_HERO_AURA)) ? 1 : 0); //0x01: Hero Aura

    public FishingInfo FishingInfo;

    public uint NameColor; //writeD(_cha.getNameColor());

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out BaseInfo);
        reader.Read(out Level); //writeD(_cha.getLevel());

        reader.Read(out Exp); //writeD(_cha.getExp());

        reader.Read(out Stas);
        reader.Read(out MaxHp); //writeD(_cha.getMaxHp());
        reader.Read(out CurrentHp); //writeD((int) _cha.getCurrentHp());
        reader.Read(out MaxMp); //writeD(_cha.getMaxMp());
        reader.Read(out CurrentMp); //writeD((int) _cha.getCurrentMp());
        reader.Read(out Sp); //writeD(_cha.getSp());
        reader.Read(out CurrentLoad); //writeD(_cha.getCurrentLoad());
        reader.Read(out MaxLoad); //writeD(_cha.getMaxLoad());

        reader.Read(out uint _); //writeD(0x28); // unknown
        reader.Read(out Accessories);
        reader.Read(out VisibleEquipment);
        reader.Read(out Accessories2);
        reader.Read(out VisibleEquipment2);

        reader.Read(out CombatStats);
        reader.Read(out PvpFlag); //writeD(_cha.getPvpFlag()); // 0-non-pvp  1-pvp = violett name
        reader.Read(out Karma); //writeD(_cha.getKarma());

        reader.Read(out MovementSpeedsEx);
        reader.Read(out AttackSpeedMultiplier); //writeF(_cha.getAttackSpeedMultiplier());

        reader.Read(out PetOrCharCollisionInfo);

        reader.Read(out HairStyle); //writeD(_cha.getHairStyle());
        reader.Read(out HairColor); //writeD(_cha.getHairColor());
        reader.Read(out FaceType); //writeD(_cha.getFace());
        reader.Read(out BuilderLevel); //writeD((_cha.getAccessLevel() > 0) ? 1 : 0); // builder level 

        //String title = _cha.getTitle();
        //if (_cha.getInvisible() == 1 && _cha.isGM()) title = "Invisible";
        //if (_cha.getPoly().isMorphed())
        //    title += " - " + NpcTable.getInstance().getTemplate(_cha.getPoly().getPolyId()).name;
        reader.Read(out ClanAndAllyInfo);
        reader.Read(
            out IsClanLeader); //writeD(_cha.isClanLeader() ? 0x60 : 0); // siege-flags  0x40 - leader rights  0x20 - ??

        reader.Read(out MountType); //writeC(_cha.getMountType()); // mount type

        reader.Read(out PrivateStoreType); //writeC(_cha.getPrivateStoreType());

        reader.Read(out HasDwarfenCraft); //writeC(_cha.hasDwarvenCraft() ? 1 : 0);

        reader.Read(out PkCount); //writeD(_cha.getPkKills());
        reader.Read(out PvpCount); //writeD(_cha.getPvpKills());

        reader.Read(out CubicInfo);

        reader.Read(out SearchForParty); //writeC(0x00); //1-find party members

        reader.Read(out AbnormalEffect); //writeD(_cha.getAbnormalEffect());
        reader.Read(out byte _); //writeC(0x00);

        reader.Read(out ClanPrivileges); //writeD(_cha.getClanPrivileges());

        reader.Read(out Swim); //writeD(0x00);//writeD(0x100); //swim
        reader.Read(out uint _); //writeD(0x00);
        reader.Read(out uint _); //writeD(0x00);
        reader.Read(out uint _); //writeD(0x00);
        reader.Read(out uint _); //writeD(0x00);
        reader.Read(out uint _); //writeD(0x00);
        reader.Read(out uint _); //writeD(0x00);

        reader.Read(out RecomendationsLeft); //writeH(_cha.getRecomLeft()); //c2  recommendations remaining

        reader.Read(out RecomendationsCount); //writeH(_cha.getRecomHave()); //c2  recommendations received
        reader.Read(out uint _); //writeD(0x00);
        reader.Read(out InventoryLimit); //writeH(_cha.GetInventoryLimit());

        reader.ReadEnum(out ClassId); //writeD(_cha.getClassId().getId());
        reader.Read(out uint _); //writeD(0x00); // special effects? circles around player...
        reader.Read(out MaxCp); //writeD(_cha.getMaxCp());
        reader.Read(out CurrentCp); //writeD((int) _cha.getCurrentCp());
        reader.Read(out EnchantEffect); //writeC(_cha.isMounted() ? 0 : _cha.getEnchantEffect());

        reader.Read(out byte _); //writeC(0x00); //team circle around feet 1= Blue, 2 = red

        reader.Read(out ClanCrestLargeId); //writeD(_cha.getClanCrestLargeId());
        reader.Read(
            out byte _); //writeC((_cha.isHero() || (_cha.isGM() && Config.GM_HERO_AURA)) ? 1 : 0); //0x01: symbol on char menu ctrl+I  
        reader.Read(
            out HeroAura); //writeC((_cha.isHero() || (_cha.isGM() && Config.GM_HERO_AURA)) ? 1 : 0); //0x01: Hero Aura

        reader.Read(out FishingInfo); //writeC(0x00); //Fishing Mode -Not Used-
        reader.Read(out NameColor); //writeD(_cha.getNameColor());
    }
}