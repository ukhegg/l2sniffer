using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class GameObjectsProvider : IGameObjectsProvider
{
    private IItemNameProvider _itemNameProvider;
    private IActionNameProvider _actionNameProvider;
    private INpcNameProvider _npcNameProvider;
    private Player _player;
    private IGameObjectsRegistry _objectsRegistry;

    public GameObjectsProvider(IItemNameProvider itemNameProvider,
                               IActionNameProvider actionNameProvider,
                               INpcNameProvider npcNameProvider,
                               IGameObjectsRegistry objectsRegistry)
    {
        _itemNameProvider = itemNameProvider;
        _actionNameProvider = actionNameProvider;
        _npcNameProvider = npcNameProvider;
        _objectsRegistry = objectsRegistry;
    }

    public GameObject GetZeroObject()
    {
        return _player;
    }

    public Player CreatePlayer(SelectedCharInfo charInfo)
    {
        _player = new Player(charInfo, _itemNameProvider, _actionNameProvider, _objectsRegistry);
        return _player;
    }

    public OtherPlayer CreateOtherPlayer(OtherUserInfo charInfo)
    {
        return new OtherPlayer(charInfo, _actionNameProvider);
    }

    public MorphedCharacter CreateMorphed(MorphedCharacterInfo charInfo)
    {
        return new MorphedCharacter(charInfo);
    }

    public Npc CreateNpc(MorphedCharacterInfo info)
    {
        return new Npc(info, _npcNameProvider.GetNpcName(info.RealNpcType), _actionNameProvider, _objectsRegistry);
    }
}