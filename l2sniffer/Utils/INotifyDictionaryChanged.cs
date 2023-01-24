using L2sniffer.GameState.GameObjects;

namespace L2sniffer.Utils;

public interface INotifyDictionaryChanged<TKey, TValue>
{
    event EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>>? DictionaryChanged;
}