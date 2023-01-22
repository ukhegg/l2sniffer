using System.Net;
using L2sniffer.Packets;
using L2sniffer.Packets.GC;
using L2sniffer.Packets.GS;
using L2sniffer.Packets.LC;
using L2sniffer.Packets.LS;
using Org.BouncyCastle.Bcpg;

namespace L2sniffer.L2PacketHandlers;

public class ConsoleWritingPacketLogger : IL2PacketLogger
{
    private ulong _packetsLogged = 0;
    private IL2ServerRegistry _serverRegistry;

    public ConsoleWritingPacketLogger(IL2ServerRegistry serverRegistry)
    {
        _serverRegistry = serverRegistry;
    }

    public void LogPacket(L2PacketBase packet, PacketMetainfo metainfo)
    {
        try
        {
            var srcEndpoint = new IPEndPoint(metainfo.TopLevelIpDirection.Source, metainfo.TransportPorts.Source);
            var dstEndpoint = new IPEndPoint(metainfo.TopLevelIpDirection.Destination,
                                             metainfo.TransportPorts.Destination);
            if (_serverRegistry.IsLoginServer(srcEndpoint))
            {
                HandleLsToLc(packet.As<LoginServerPacketBase>(), metainfo);
            }
            else if (_serverRegistry.IsLoginServer(dstEndpoint))
            {
                HandleLcToLs(packet.As<LoginClientPacketBase>(), metainfo);
            }
            else if (_serverRegistry.IsGameServer(srcEndpoint))
            {
                HandleGsToGc(packet.As<GameServerPacketBase>(), metainfo);
            }
            else if (_serverRegistry.IsGameServer(dstEndpoint))
            {
                HandleGcToGs(packet.As<GameClientPacketBase>(), metainfo);
            }
            else
            {
                Console.WriteLine($"#{_packetsLogged}: {packet.Bytes.Length} bytes");
            }
        }
        finally
        {
            _packetsLogged++;
        }
    }

    private void HandleLsToLc(LoginServerPacketBase packet, PacketMetainfo metainfo)
    {
        Console.WriteLine($"#{_packetsLogged} Login  S --> C:  {packet.PacketType}  {packet.Bytes.Length} bytes");
    }

    private void HandleLcToLs(LoginClientPacketBase packet, PacketMetainfo metainfo)
    {
        Console.WriteLine($"#{_packetsLogged} Login  C --> S:  {packet.PacketType}  {packet.Bytes.Length} bytes");
    }

    private void HandleGsToGc(GameServerPacketBase packet, PacketMetainfo metainfo)
    {
        Console.WriteLine($"#{_packetsLogged} Game   S --> C:  {packet.PacketType}  {packet.Bytes.Length} bytes");
    }

    private void HandleGcToGs(GameClientPacketBase packet, PacketMetainfo metainfo)
    {
        Console.WriteLine($"#{_packetsLogged} Game   C --> S:  {packet.PacketType}  {packet.Bytes.Length} bytes");
    }
}