namespace L2sniffer.GameState.GameObjects;

public interface IGameObjectsRegistry
{
    void RegisterObject(GameObject obj);

    void RegisterObject(GameObjectId id, GameObject obj);
    
    GameObject GetObject(GameObjectId objectId);

    bool TryGetObject(GameObjectId objectId, out GameObject value);
    
    void DeleteObject(GameObjectId objectId);
}