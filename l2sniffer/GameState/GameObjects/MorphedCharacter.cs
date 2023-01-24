using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class MorphedCharacter : GameObject
{
    private readonly NpcName _npcName;
    public MorphedCharacterInfo Info { get; }
    public Coordinates3d Location { get; private set; }

    public MorphedCharacter(MorphedCharacterInfo info)
        : base(info.ObjectId)
    {
        Info = info;
        Location = Info.Coordinates;
    }

    public override void IfMorphedChar(Action<MorphedCharacter> callback)
    {
        callback(this);
    }

    public void Update(MorphedCharacterInfo info)
    {
        Console.WriteLine($"    Updating char {_npcName?.Name} id={this.Info.ObjectId}");
    }

    public void MoveToLocation(Coordinates3d current, Coordinates3d dst)
    {
        Location = current;
        Console.WriteLine($"    Char {_npcName?.Name} id={Info.ObjectId} is moving from {current} to {dst}");
    }

    public void UpdateStatus(CharStatusUpdate statusUpdates)
    {
        foreach (var updateAttribute in statusUpdates.Attributes)
        {
            switch (updateAttribute.Type)
            {
                case UpdateAttributeTypes.Level:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Level to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Exp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Exp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Str:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Str to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Dex:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Dex to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Con:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Con to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Int:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Int to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Wit:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Wit to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Men:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Men to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.CurHp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Cur Hp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.MaxHp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Max Hp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.CurMp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Cur Mp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.MaxMp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Max Mp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Sp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Sp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.CurLoad:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Cur load to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.MaxLoad:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Max Load to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.PAttack:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} PAttack to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.AttackSpeed:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Attack speed to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.PDef:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} PDef to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Evasion:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Evasion to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Accuracy:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Accuracy to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Critical:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Critical to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.MAttack:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} MAttack to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.CastSpeed:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} CastSpeed to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.MDef:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} MDef to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.PvpFlag:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Pvp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.Karma:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Karma to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.CurCp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Cur Cp to {updateAttribute.NewValue}");
                    break;
                case UpdateAttributeTypes.MaxCp:
                    Console.WriteLine(
                        $"    updating {_npcName?.Name} {Info.ObjectId} Max Cp to {updateAttribute.NewValue}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    
}