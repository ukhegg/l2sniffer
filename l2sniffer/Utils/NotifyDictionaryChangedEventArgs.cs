namespace L2sniffer.Utils;

public class NotifyDictionaryChangedEventArgs<TKey, TValue> : EventArgs
{
    public TKey Key { get; }

    public TValue? OldValue { get; private set; }

    public TValue? NewValue { get; private set; }

    public NotifyDictionaryChangedEventArgs(TKey key)
    {
        Key = key;
    }

    public static NotifyDictionaryChangedEventArgs<TKey, TValue> CaseAdd(TKey key, TValue newValue)
    {
        var result = new NotifyDictionaryChangedEventArgs<TKey, TValue>(key);
        result.NewValue = newValue;
        return result;
    }

    public static NotifyDictionaryChangedEventArgs<TKey, TValue> CaseReplace(TKey key, TValue oldValue, TValue newValue)
    {
        var result = new NotifyDictionaryChangedEventArgs<TKey, TValue>(key);
        result.OldValue = oldValue;
        result.NewValue = newValue;
        return result;
    }

    public static NotifyDictionaryChangedEventArgs<TKey, TValue> CaseRemove(TKey key, TValue oldValue)
    {
        var result = new NotifyDictionaryChangedEventArgs<TKey, TValue>(key);
        result.OldValue = oldValue;
        return result;
    }
}