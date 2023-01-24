using System.Collections;
using L2sniffer.Packets.DataStructs;
using L2sniffer.Utils;

namespace L2sniffer.GameState.GameObjects;

public class PlayerAttributes : IReadOnlyDictionary<UpdateAttributeTypes, uint>,
    INotifyDictionaryChanged<UpdateAttributeTypes, uint>
{
    private IDictionary<UpdateAttributeTypes, uint> _attributes;

    public PlayerAttributes()
    {
        _attributes = new Dictionary<UpdateAttributeTypes, uint>();
    }

    private void UpdateAttribute(UpdateAttributeTypes type, uint value)
    {
        var dictionaryChanged = DictionaryChanged;
        if (!_attributes.ContainsKey(type))
        {
            _attributes[type] = value;
            if (dictionaryChanged != null)
            {
                dictionaryChanged(this,
                                  NotifyDictionaryChangedEventArgs<UpdateAttributeTypes, uint>.CaseAdd(type, value));
            }
        }
        else
        {
            var oldValue = _attributes[type];
            if (oldValue == value) return;

            _attributes[type] = value;
            if (dictionaryChanged != null)
            {
                dictionaryChanged(this,
                                  NotifyDictionaryChangedEventArgs<UpdateAttributeTypes, uint>.CaseReplace(
                                      type, oldValue, value));
            }
        }
    }

    public void Update(SelectedCharInfo charInfo)
    {
        UpdateAttribute(UpdateAttributeTypes.Level, charInfo.Level);
        UpdateAttribute(UpdateAttributeTypes.Exp, charInfo.Stats.Str);
        UpdateAttribute(UpdateAttributeTypes.Str, charInfo.Stats.Str);
        UpdateAttribute(UpdateAttributeTypes.Dex, charInfo.Stats.Dex);
        UpdateAttribute(UpdateAttributeTypes.Con, charInfo.Stats.Con);
        UpdateAttribute(UpdateAttributeTypes.Int, charInfo.Stats.Int);
        UpdateAttribute(UpdateAttributeTypes.Wit, charInfo.Stats.Wit);
        UpdateAttribute(UpdateAttributeTypes.Men, charInfo.Stats.Men);
        UpdateAttribute(UpdateAttributeTypes.CurHp, (uint)charInfo.Hp);
        UpdateAttribute(UpdateAttributeTypes.CurMp, (uint)charInfo.Mp);
        UpdateAttribute(UpdateAttributeTypes.Sp, charInfo.Sp);
    }

    public void Update(OtherUserInfo playerInfo)
    {
        UpdateAttribute(UpdateAttributeTypes.PvpFlag, playerInfo.PvpFlag);
        UpdateAttribute(UpdateAttributeTypes.Karma, playerInfo.Karma);
        UpdateAttribute(UpdateAttributeTypes.CastSpeed, playerInfo.MAttackSpeed);
        UpdateAttribute(UpdateAttributeTypes.AttackSpeed, playerInfo.PAttackSpeed);
    }

    public void Update(UserInfo info)
    {
        UpdateAttribute(UpdateAttributeTypes.Level, info.Level);
        UpdateAttribute(UpdateAttributeTypes.Exp, info.Exp);
        UpdateAttribute(UpdateAttributeTypes.Str, info.Stas.Str);
        UpdateAttribute(UpdateAttributeTypes.Con, info.Stas.Con);
        UpdateAttribute(UpdateAttributeTypes.Dex, info.Stas.Dex);
        UpdateAttribute(UpdateAttributeTypes.Men, info.Stas.Men);
        UpdateAttribute(UpdateAttributeTypes.Int, info.Stas.Int);
        UpdateAttribute(UpdateAttributeTypes.Wit, info.Stas.Wit);
        UpdateAttribute(UpdateAttributeTypes.MaxHp, info.MaxHp);
        UpdateAttribute(UpdateAttributeTypes.CurHp, info.CurrentHp);
        UpdateAttribute(UpdateAttributeTypes.MaxMp, info.MaxMp);
        UpdateAttribute(UpdateAttributeTypes.CurMp, info.CurrentMp);
        UpdateAttribute(UpdateAttributeTypes.Sp, info.Sp);
        UpdateAttribute(UpdateAttributeTypes.CurLoad, info.CurrentLoad);
        UpdateAttribute(UpdateAttributeTypes.MaxLoad, info.MaxLoad);
        UpdateAttribute(UpdateAttributeTypes.PAttack, info.CombatStats.PAttack);
        UpdateAttribute(UpdateAttributeTypes.AttackSpeed, info.CombatStats.PAttackSpeed);
        UpdateAttribute(UpdateAttributeTypes.PDef, info.CombatStats.PDef);
        UpdateAttribute(UpdateAttributeTypes.Evasion, info.CombatStats.Evasion);
        UpdateAttribute(UpdateAttributeTypes.Accuracy, info.CombatStats.Accuracy);
        UpdateAttribute(UpdateAttributeTypes.Critical, info.CombatStats.CriticalHit);
        UpdateAttribute(UpdateAttributeTypes.MAttack, info.CombatStats.MAttack);
        UpdateAttribute(UpdateAttributeTypes.CastSpeed, info.CombatStats.MAttackSpeed);
        UpdateAttribute(UpdateAttributeTypes.PvpFlag, info.PvpFlag);
        UpdateAttribute(UpdateAttributeTypes.Karma, info.Karma);
        UpdateAttribute(UpdateAttributeTypes.CurCp, info.CurrentCp);
        UpdateAttribute(UpdateAttributeTypes.MaxCp, info.MaxCp);
    }

    public void Update(CharStatusUpdate charStatus)
    {
        foreach (var attribute in charStatus.Attributes)
        {
            UpdateAttribute(attribute.Type, attribute.NewValue);
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _attributes.GetEnumerator();
    }

    public IEnumerator<KeyValuePair<UpdateAttributeTypes, uint>> GetEnumerator()
    {
        return _attributes.GetEnumerator();
    }

    public int Count => _attributes.Count;

    public bool ContainsKey(UpdateAttributeTypes key)
    {
        return _attributes.ContainsKey(key);
    }

    public bool TryGetValue(UpdateAttributeTypes key, out uint value)
    {
        return _attributes.TryGetValue(key, out value);
    }

    public uint this[UpdateAttributeTypes key] => _attributes[key];

    public IEnumerable<UpdateAttributeTypes> Keys => _attributes.Keys;

    public IEnumerable<uint> Values => _attributes.Values;

    public event EventHandler<NotifyDictionaryChangedEventArgs<UpdateAttributeTypes, uint>>? DictionaryChanged;


}