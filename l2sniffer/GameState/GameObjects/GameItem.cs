using System.ComponentModel;
using L2sniffer.GameData;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState.GameObjects;

public class GameItem : INotifyPropertyChanged
{
    private ItemsListEntry _item;
    private ItemName _itemName;

    public GameItem(ItemsListEntry item, ItemName itemName)
    {
        _item = item;
        _itemName = itemName;
    }

    public string Name => _itemName.Name;

    public uint Count => _item.ItemCount;
    public uint Id => _item.ItemId;

    public void Update(ItemsListEntry itemInfo)
    {
        _item = itemInfo;
        OnPropertyChanged("Count");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}