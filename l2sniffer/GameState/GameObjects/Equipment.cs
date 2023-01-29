using L2sniffer.GameData;

namespace L2sniffer.GameState.GameObjects;

public class Equipment : GameObject
{
    private ItemName _itemName;

    public uint Count { get; private set; } = 0;

    public string Name => _itemName.Name;

    public override string ToString()
    {
        return $"{_itemName.Name} x{Count}";
    }

    public Equipment(GameObjectId objectId,
                     ItemName itemName,
                     uint count) : base(objectId)
    {
        _itemName = itemName;
        Count = count;
    }
}