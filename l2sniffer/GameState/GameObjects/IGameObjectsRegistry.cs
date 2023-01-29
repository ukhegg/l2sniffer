using System.Runtime.CompilerServices;

namespace L2sniffer.GameState.GameObjects;

public interface IGameObjectsRegistry
{
    void RegisterObject(GameObject obj);

    void RegisterObject(GameObjectId id, GameObject obj);

    GameObject GetObject(GameObjectId objectId);

    bool TryGetObject(GameObjectId objectId, out GameObject value);

    void DeleteObject(GameObjectId objectId);
}

public static class IGameObjectsRegistryExtensions
{
    public static T GetObject<T>(this IGameObjectsRegistry registry, GameObjectId id)
        where T : GameObject
    {
        var t = registry.GetObject(id);
        if (t is T) return (T)t;
        throw new InvalidCastException($"{id} is not of type {typeof(T).Name}");
    }
}