using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class MagicEffectIconsPacket : GameServerPacketBase
{
    public MagicEffect[] MagicEffects;

    public MagicEffectIconsPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out ushort effectsCount);
        MagicEffects = new MagicEffect[effectsCount];
        for (var i = 0; i < effectsCount; ++i)
        {
            reader.Read(out MagicEffect effect);
            MagicEffects[i] = effect;
        }
    }
}