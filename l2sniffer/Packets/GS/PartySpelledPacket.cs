using L2sniffer.GameState;
using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class PartySpelledPacket : GameServerPacketBase
{
    public MagicEffect[] Effects;
    public GameObjectId Char;

    public PartySpelledPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out uint sourceType); // writeD(_char instanceof L2Summon ? 2 : 0);
        reader.Read(out Char);
        reader.Read(out uint effectsCount);
        Effects = new MagicEffect[effectsCount];
        for (var i = 0; i < effectsCount; ++i)
        {
            reader.Read(out MagicEffect e);
            Effects[i] = e;
        }
    }
}