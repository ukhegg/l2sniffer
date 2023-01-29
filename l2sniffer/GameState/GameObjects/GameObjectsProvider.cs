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
    private ISkillInfoProvider _skillNameProvider;

    public GameObjectsProvider(IItemNameProvider itemNameProvider,
                               IActionNameProvider actionNameProvider,
                               INpcNameProvider npcNameProvider,
                               IGameObjectsRegistry objectsRegistry,
                               ISkillInfoProvider skillNameProvider)
    {
        _itemNameProvider = itemNameProvider;
        _actionNameProvider = actionNameProvider;
        _npcNameProvider = npcNameProvider;
        _objectsRegistry = objectsRegistry;
        _skillNameProvider = skillNameProvider;
    }

    public GameObject GetZeroObject()
    {
        return _player;
    }

    public Player CreatePlayer(CharacterInfo characterInfo, SelectedCharInfo selectedInfo)
    {
        _player = new Player(characterInfo,
                             selectedInfo,
                             _itemNameProvider,
                             _actionNameProvider,
                             _objectsRegistry,
                             _skillNameProvider);
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

    public Equipment CreateEquipment(GameObjectId objectId, uint itemId, uint count)
    {
        var itemInfo = _itemNameProvider.GetItem(itemId);
        return new Equipment(objectId, itemInfo, count);
    }
}