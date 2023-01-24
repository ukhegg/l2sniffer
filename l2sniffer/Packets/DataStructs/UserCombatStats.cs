namespace L2sniffer.Packets.DataStructs;

public class UserCombatStats : DataStruct
{
    public uint PAttack; //writeD(_cha.getPAtk(null));
    public uint PAttackSpeed; //writeD(_cha.getPAtkSpd());
    public uint PDef; //writeD(_cha.getPDef(null));
    public uint Evasion; //writeD(_cha.getEvasionRate(null));
    public uint Accuracy; //writeD(_cha.getAccuracy());
    public uint CriticalHit; //writeD(_cha.getCriticalHit(null, null));
    public uint MAttack; //writeD(_cha.getMAtk(null, null));
    public uint MAttackSpeed; //writeD(_cha.getMAtkSpd());
    public uint PAttackSpeed2; //writeD(_cha.getPAtkSpd());
    public uint MDef; //writeD(_cha.getMDef(null, null));

    public override void ReadFields(ref FieldsReader reader)
    {
        reader.Read(out PAttack);
        reader.Read(out PAttackSpeed);
        reader.Read(out PDef);
        reader.Read(out Evasion);
        reader.Read(out Accuracy);
        reader.Read(out CriticalHit);
        reader.Read(out MAttack);
        reader.Read(out MAttackSpeed);
        reader.Read(out PAttackSpeed2);
        reader.Read(out MDef);
    }
}