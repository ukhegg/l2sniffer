using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState;

public class MainCharacter
{
    private readonly SelectedCharInfo _selectedChar;
    private MorphedCharacterInfo _currentState;

    public MainCharacter(SelectedCharInfo selectedChar)
    {
        _selectedChar = selectedChar;
    }

    public string Name => _selectedChar.Name;

    
    public void Update(MorphedCharacterInfo charInfo)
    {
        _currentState = charInfo;
    }
}