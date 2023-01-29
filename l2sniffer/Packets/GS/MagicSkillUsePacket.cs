using L2sniffer.GameData;
using L2sniffer.GameState;
using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.Packets.GS;

public class MagicSkillUsePacket : GameServerPacketBase
{
    public MagicSkillUseInfo SkillUseInfo;

    public MagicSkillUsePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader reader)
    {
        reader.Read(out SkillUseInfo);
    }
}