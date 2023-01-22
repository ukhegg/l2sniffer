namespace L2sniffer.Packets;

public class TypeL2PacketBase<TTypeEnum> : L2PacketBase
    where TTypeEnum : Enum
{
    public TypeL2PacketBase(byte[] bytes) : base(bytes)
    {
    }

    public TTypeEnum PacketType => (TTypeEnum)(object)PacketTypeRaw;
}