using L2sniffer;
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
            kernel.Bind<IPacketHandler<EthernetPacket>>().To<EthernetPacketHandler>();
            kernel.Bind<IPacketHandler<IPv4Packet>>().To<IpV4PacketHandler>();
            kernel.Bind<IPacketHandler<TcpPacket>>().To<TcpStreamSplitHandler>();
            kernel.Bind<ITcpStreamHandlerProvider>().To<TcpSegmentsSplitterProvider>();

            kernel.Bind<IDatagramStreamReaderProvider>().To<L2DatagramStreamReader.Provider>();

            var datagramAccumulator = kernel.Get<DatagramAccumulator>();
            kernel.Bind<IDatagramStreamHandlerProvider>().ToMethod(context => datagramAccumulator);


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

    public Tuple<byte[], StreamId> GetL2Packet(string file, int index)
    {
        var datagrams = GetL2Packets(file);
        return datagrams[index];
    }
}