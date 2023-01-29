using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class ShortcutInitPacket : GameServerPacketBase
{
    public L2Shortcut[] Shortcuts;

    public ShortcutInitPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out uint shortcutsCount);
        Shortcuts = new L2Shortcut[shortcutsCount];
        for (var i = 0; i < shortcutsCount; ++i)
        {
            reader.Read(out L2Shortcut sc);
            Shortcuts[i] = sc;
        }
    }
}