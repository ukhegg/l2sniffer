using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState;

public class MorphedCharacter
{
    public MorphedCharacterInfo Info { get; }
    public Coordinates3d Location { get; private set; }

    public MorphedCharacter(MorphedCharacterInfo info)
    {
        Info = info;
        Location = Info.Coordinates;
    }

    public void Update(MorphedCharacterInfo info)
    {
        int a = 9;
    }

    public void MoveToLocation(Coordinates3d current, Coordinates3d dst)
    {
        Location = current;
    }
}

public class GameSession
{
    private MainCharacter _mainCharacter;
    private IDictionary<uint, MorphedCharacter> _characters;

    public GameSession()
    {
        _characters = new Dictionary<uint, MorphedCharacter>();
    }

    public MainCharacter MainCharacter
    {
        get => _mainCharacter;
        private set => _mainCharacter = value;
    }

    public void OnCharSelected(SelectedCharInfo selectedCharInfo)
    {
        MainCharacter = new MainCharacter(selectedCharInfo);
    }

    public void UpdateCharacter(MorphedCharacterInfo characterInfo)
    {
        if (_characters.TryGetValue(characterInfo.ObjectId, out MorphedCharacter c))
        {
            c.Update(characterInfo);
        }
        else
        {
            c = new MorphedCharacter(characterInfo);
        }
    }

    public void MoveToLocation(uint objectId, Coordinates3d current, Coordinates3d dst)
    {
        if (!_characters.TryGetValue(objectId, out MorphedCharacter c))
        {
            Console.WriteLine($"unknwon object ID {objectId}");
            return;
        }

        c.MoveToLocation(current, dst);
    }
}