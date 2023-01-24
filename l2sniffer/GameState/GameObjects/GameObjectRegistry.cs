namespace L2sniffer.GameState.GameObjects;

public class GameObjectRegistry : IGameObjectsRegistry
{
    private IDictionary<GameObjectId, GameObject> _objects;

    public GameObjectRegistry()
    {
        _objects = new Dictionary<GameObjectId, GameObject>();
    }

    public void RegisterObject(GameObject obj)
    {
        Console.WriteLine($"    Registering new game object \'{obj.ObjectName}\',id {obj.ObjectId}");
        _objects[obj.ObjectId] = obj;
    }

    public void DeleteObject(GameObjectId objectId)
    {
        if (_objects.TryGetValue(objectId, out var gameObject))
        {
            Console.WriteLine($"    Deleting game object \'{gameObject.ObjectName}\',id {objectId}");
        }
        
    }

    public void RegisterObject(GameObjectId id, GameObject obj)
    {
        Console.WriteLine($"    Registering existing game object \'{obj.ObjectName}\' under new id {id}");
        _objects[id] = obj;
    }

    public GameObject GetObject(GameObjectId objectId)
    {
        return _objects[objectId];
    }

    public bool TryGetObject(GameObjectId objectId, out GameObject value)
    {
        return _objects.TryGetValue(objectId, out value);
    }
}