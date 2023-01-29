using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public interface IGameObjectsProvider
{
    Player CreatePlayer(CharacterInfo characterInfo, SelectedCharInfo selectedInfo);

    OtherPlayer CreateOtherPlayer(OtherUserInfo charInfo);

    MorphedCharacter CreateMorphed(MorphedCharacterInfo charInfo);

    Npc CreateNpc(MorphedCharacterInfo info);

    Equipment CreateEquipment(GameObjectId objectId, uint itemId, uint count);
}