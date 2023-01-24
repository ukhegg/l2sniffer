using System.Text;

namespace L2sniffer.Packets.GS;

public class SystemMessagePacket : GameServerPacketBase
{
    public enum MessagePartTypes : uint
    {
        Text = 0x00,
        Number = 0x01,
        NpcName = 0x02,
        ItemName = 0x03,
        SkillName = 0x04
    }

    public class MessagePart : DataStruct
    {
        public MessagePartTypes Type;
        public object MessageData;

        public override void ReadFields(ref FieldsReader reader)
        {
            reader.ReadEnum(out Type);
            switch (Type)
            {
                case MessagePartTypes.Text:
                    reader.Read(out string _1);
                    MessageData = _1;
                    break;
                case MessagePartTypes.Number:
                    reader.Read(out uint _2);
                    MessageData = _2;
                    break;
                case MessagePartTypes.NpcName:
                    reader.Read(out uint _3);
                    MessageData = _3;
                    break;
                case MessagePartTypes.ItemName:
                    reader.Read(out uint _4);
                    MessageData = _4;
                    break;
                case MessagePartTypes.SkillName:
                    reader.Read(out uint _5);
                    MessageData = _5;
                    reader.Read(out uint mustBe1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return Type switch
            {
                MessagePartTypes.Text => $"{{ str {(string)MessageData} }}",
                MessagePartTypes.Number => $"{{ uint {(uint)MessageData} }}",
                MessagePartTypes.NpcName => $"{{ npc name {(uint)MessageData} }}",
                MessagePartTypes.ItemName => $"{{ item name {(uint)MessageData} }}",
                MessagePartTypes.SkillName => $"{{ skill name {(uint)MessageData} }}",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public uint MessageId;
    public MessagePart[] Parts;

    public SystemMessagePacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out MessageId);
        fieldsReader.Read(out uint partsCount);
        Parts = new MessagePart[partsCount];
        for (var i = 0; i < partsCount; ++i)
        {
            fieldsReader.Read(out MessagePart mp);
            Parts[i] = mp;
        }
    }

    public override string ToString()
    {
        var sBuilder = new StringBuilder();
        foreach (var part in Parts)
        {
            sBuilder.Append(part);
        }

        return sBuilder.ToString();
    }
}