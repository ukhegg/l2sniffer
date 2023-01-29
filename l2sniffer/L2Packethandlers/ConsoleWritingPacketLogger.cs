using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using L2sniffer.Packets;
using L2sniffer.Packets.GC;
using L2sniffer.Packets.GS;
using L2sniffer.Packets.LC;
using L2sniffer.Packets.LS;
using Org.BouncyCastle.Bcpg;
using SharpPcap;

namespace L2sniffer.L2PacketHandlers;

public class ConsoleWritingPacketLogger : IL2PacketLogger
{
    private ulong _packetsLogged = 0;
    private IL2ServerRegistry _serverRegistry;
    private PosixTimeval _captureStart;

    public ConsoleWritingPacketLogger(IL2ServerRegistry serverRegistry)
    {
        _serverRegistry = serverRegistry;
    }

    public void LogHandledPacket(L2PacketBase packet, PacketMetainfo metainfo)
    {
        if (_packetsLogged == 0)
        {
            this._captureStart = metainfo.CaptureTime;
        }

        LogPacket(packet, metainfo, true);
    }

    public void LogUnhandledPacket(L2PacketBase packet, PacketMetainfo metainfo)
    {
        if (_packetsLogged == 0)
        {
            this._captureStart = metainfo.CaptureTime;
        }

        LogPacket(packet, metainfo, false);
    }


    private void LogPacket(L2PacketBase packet, PacketMetainfo metainfo, bool isHandled)
    {
        try
        {
            var srcEndpoint = new IPEndPoint(metainfo.TopLevelIpDirection.Source, metainfo.TransportPorts.Source);
            var dstEndpoint = new IPEndPoint(metainfo.TopLevelIpDirection.Destination,
                                             metainfo.TransportPorts.Destination);
            if (_serverRegistry.IsLoginServer(srcEndpoint))
            {
                HandlePacket(packet.As<LoginServerPacketBase>(), metainfo, isHandled);
            }
            else if (_serverRegistry.IsLoginServer(dstEndpoint))
            {
                HandlePacket(packet.As<LoginClientPacketBase>(), metainfo, isHandled);
            }
            else if (_serverRegistry.IsGameServer(srcEndpoint))
            {
                HandlePacket(packet.As<GameServerPacketBase>(), metainfo, isHandled);
            }
            else if (_serverRegistry.IsGameServer(dstEndpoint))
            {
                HandlePacket(packet.As<GameClientPacketBase>(), metainfo, isHandled);
            }
            else
            {
                Console.WriteLine(
                    $"+{GetCaptureTimeDelta(metainfo.CaptureTime)}  #{_packetsLogged}: {packet.Bytes.Length} bytes");
            }
        }
        finally
        {
            _packetsLogged++;
        }
    }

    private void HandlePacket<TPacketType>(TypeL2PacketBase<TPacketType> packet,
                                           PacketMetainfo metainfo,
                                           bool isHandled) where TPacketType : Enum
    {
        var directionInfo = GetDirectionInfo<TPacketType>();
        HandlePacket(directionInfo.Item3, directionInfo.Item1, directionInfo.Item2, directionInfo.Item4,
                     packet,
                     metainfo.CaptureTime,
                     isHandled);
    }

    private Tuple<string, string, string, string> GetDirectionInfo<TPacketType>() where TPacketType : Enum
    {
        if (typeof(TPacketType) == typeof(GameServerPacketTypes))
        {
            return new("S", "C", "Game", "-->");
        }

        if (typeof(TPacketType) == typeof(GameClientPacketTypes))
        {
            return new("S", "C", "Game", "<--");
        }

        if (typeof(TPacketType) == typeof(LoginServerPacketTypes))
        {
            return new("S", "C", "Login", "-->");
        }

        if (typeof(TPacketType) == typeof(LoginClientPacketTypes))
        {
            return new("S", "C", "Login", "<--");
        }

        throw new ArgumentException("unknown packet type");
    }

    private void HandlePacket<TPacketType>(string sessionName,
                                           string participant1,
                                           string participant2,
                                           string direction,
                                           TypeL2PacketBase<TPacketType> packet,
                                           PosixTimeval captureTime,
                                           bool isHandled) where TPacketType : Enum
    {
        var handleStatus = isHandled ? '+' : '-';

        if (!Enum.IsDefined(typeof(TPacketType), packet.PacketType))
        {
            Console.WriteLine();
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        var packetTypeString = Enum.IsDefined(typeof(TPacketType), packet.PacketType)
            ? $"{packet.PacketType.ToString()}(0x{(byte)(object)packet.PacketType:x2})"
            : $"({typeof(TPacketType).Name})0x{(byte)(object)packet.PacketType:x2}??";

        var delta = GetCaptureTimeDelta(captureTime);
        var absTime = DateTime.UnixEpoch.AddSeconds((uint)captureTime.Value);
        Console.WriteLine(
            $"{absTime}(+{delta})  #{_packetsLogged:D5} {sessionName,-5}  {participant1} {direction} {participant2}:  {handleStatus} {packetTypeString,-30}  {packet.Bytes.Length} bytes");
    }

    private string GetCaptureTimeDelta(PosixTimeval captureTime)
    {
        var dt = captureTime.Value - _captureStart.Value;
        var timeSpan = new TimeSpan((long)dt * 10000000);
        return timeSpan.ToString();
    }
}