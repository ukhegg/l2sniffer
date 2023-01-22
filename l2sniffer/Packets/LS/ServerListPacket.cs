namespace L2sniffer.Packets.LS;

public class ServerListPacket : LoginServerPacketBase
{
    public struct ServerInfo
    {
        public byte Id;
        public uint Ip;
        public uint Port;
        public byte AgeLimit;
        public byte IsPvp;
        public ushort Online;
        public ushort MaxOnline;
        public byte IsTest;
    }

    public ServerListPacket(byte[] bytes) : base(bytes)
    {
    }

    protected override void ReadPayloadFields(FieldsReader fieldsReader)
    {
        fieldsReader.Read(out byte serversCount);
        fieldsReader.Read(out byte reserved);

        for (var i = 0; i < serversCount; ++i)
        {
            var serverInfo = new ServerInfo();
            fieldsReader.Read(out serverInfo.Id);
            fieldsReader.Read(out serverInfo.Ip);
            fieldsReader.Read(out serverInfo.Port);
            fieldsReader.Read(out serverInfo.AgeLimit);
            fieldsReader.Read(out serverInfo.IsPvp);
            fieldsReader.Read(out serverInfo.Online);
            fieldsReader.Read(out serverInfo.MaxOnline);
            fieldsReader.Read(out serverInfo.IsTest);
            Servers.Add(serverInfo);
        }
    }

    public List<ServerInfo> Servers { get; } = new();
}