using L2sniffer;
using L2sniffer.Crypto;
using L2sniffer.L2PacketHandlers;
using l2sniffer.PacketHandlers;
using L2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;
using Ninject;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace Tests;

public class TestHelper
{
    class DatagramAccumulator : IDatagramStreamHandlerProvider
    {
        class DatagramWriter : IDatagramStreamHandler
        {
            private readonly List<Tuple<byte[], StreamId>> _datagrams;

            public DatagramWriter(List<Tuple<byte[], StreamId>> datagrams)
            {
                _datagrams = datagrams;
            }

            public void HandleDatagram(byte[] datagram, PacketMetainfo metainfo)
            {
                if (metainfo.TopLevelIpDirection == null || metainfo.TransportPorts == null) return;
                _datagrams.Add(new Tuple<byte[], StreamId>(datagram,
                                                           new StreamId(metainfo.TopLevelIpDirection,
                                                                        metainfo.TransportPorts)));
            }

            public void HandleMissingInterval(uint loweBound, uint upperBound)
            {
                throw new NotImplementedException();
            }
        }

        public List<Tuple<byte[], StreamId>> Datagrams = new();

        public IDatagramStreamHandler GetDatagramHandler(StreamId streamId)
        {
            return new DatagramWriter(Datagrams);
        }

        public IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports)
        {
            return new DatagramWriter(Datagrams);
        }
    }

    public string TestFilePath(string relPath)
    {
        var workDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        if (workDirectory == null)
        {
            throw new Exception("work folder not found");
        }

        return Path.Combine(workDirectory, "test files", relPath);
    }

    public List<Tuple<byte[], StreamId>> GetL2Packets(string file)
    {
        try
        {
            var device = new CaptureFileReaderDevice(TestFilePath(file));
            device.Open();

            IKernel kernel = new StandardKernel();
            //l2 handlers
            var decryptorProvider = kernel.Get<PacketDecryptorProvider>();
            kernel.Bind<IPacketDecryptorProvider>().ToMethod(context => decryptorProvider);
            kernel.Bind<ISessionCryptKeysRegistry>().ToMethod(context => decryptorProvider);
            kernel.Bind<IDatagramStreamReaderProvider>().To<L2DatagramStreamReader.Provider>();
            
            //packet handlers bindings
            kernel.Bind<IPacketHandler<EthernetPacket>>().To<EthernetPacketHandler>();
            kernel.Bind<IPacketHandler<IPv4Packet>>().To<IpV4PacketHandler>();
            kernel.Bind<ITcpAssemblerProvider>().To<TcpReordererProvider>();
            kernel.Bind<IPacketHandler<TcpPacket>>().To<TcpStreamSplitter>();

            //datagam handlers
            var datagramAccumulator = kernel.Get<DatagramAccumulator>();
            kernel.Bind<IDatagramStreamHandlerProvider>().ToMethod(context => datagramAccumulator)
                .WhenInjectedInto<TcpSegmentsSplitterProvider>();
            kernel.Bind<IDatagramStreamHandlerProvider>().To<TcpSegmentsSplitterProvider>()
                .WhenInjectedInto<TcpStreamSplitter>();

            var packetHandlerProvider = kernel.Get<PacketHandlerProvider>();
            kernel.Bind<IPacketHandlerProvider>().ToMethod(context => packetHandlerProvider);
            kernel.Bind<IPacketHandlerRegistry>().ToMethod(context => packetHandlerProvider);

            var handlerRegistry = kernel.Get<IPacketHandlerRegistry>();
            handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<EthernetPacket>>());
            handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<IPv4Packet>>());
            handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<TcpPacket>>());


            

            kernel.Get<CaptureProcessor>().ProcessCapture(device);
            return datagramAccumulator.Datagrams;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<byte[]> GetL2Packets(string file, StreamId streamId)
    {
        var result = new List<byte[]>();
        foreach (var tuple in GetL2Packets(file))
        {
            if (!tuple.Item2.Equals(streamId)) continue;
            result.Add(tuple.Item1);
        }

        return result;
    }
    
    public Tuple<byte[], StreamId> GetL2Packet(string file, int index)
    {
        var datagrams = GetL2Packets(file);
        return datagrams[index];
    }

    public byte[] GetL2Packet(string file, StreamId streamId, int index)
    {
        return GetL2Packets(file, streamId)[index];
    }

    public List<byte[]> GetL2Packets(string file, StreamId streamId,
                                     IL2PacketDecryptor decryptor,
                                     bool skipFirst = true)
    {
        var packets = GetL2Packets(file, streamId);
        var result = new List<byte[]>();
        for (var i = 0; i < packets.Count; ++i)
        {
            if (i == 0 && skipFirst)
            {
                result.Add(packets[0]);
            }
            else
            {
                result.Add(decryptor.DecryptPacket(packets[i]));
            }
        }

        return result;
    }

    public byte[] GetL2Packet(string file, StreamId streamId,
                              IL2PacketDecryptor decryptor,
                              int index,
                              bool skipFirst = true)
    {
        return GetL2Packets(file, streamId, decryptor, skipFirst)[index];
    }
}