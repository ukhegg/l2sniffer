using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public interface IGameObjectsProvider
{
    Player CreatePlayer(SelectedCharInfo charInfo);

    OtherPlayer CreateOtherPlayer(OtherUserInfo charInfo);

    MorphedCharacter CreateMorphed(MorphedCharacterInfo charInfo);

    Npc CreateNpc(MorphedCharacterInfo info);
    
    GameObject GetZeroObject();
}