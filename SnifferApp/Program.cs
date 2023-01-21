using L2sniffer;
using l2sniffer.PacketHandlers;
using L2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;
using Ninject;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace SnifferApp
{
    class DummyDatagramHandler : IDatagramStreamHandler
    {
        public void HandleDatagram(byte[] datagram, PacketMetainfo metainfo)
        {
            Console.WriteLine($"handling datagram {datagram.Length} bytes");
        }

        public class Provider : IDatagramStreamHandlerProvider
        {
            public IDatagramStreamHandler GetDatagramHandler(StreamId streamId)
            {
                return GetDatagramHandler(streamId.IpDirection, streamId.Ports);
            }

            public IDatagramStreamHandler GetDatagramHandler(IpDirection ipDirection, TransportDirection ports)
            {
                return new DummyDatagramHandler();
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var ver = Pcap.SharpPcapVersion;

            /* Print SharpPcap version */
            Console.WriteLine("SharpPcap {0}, ReadingCaptureFile", ver);
            Console.WriteLine();

            Console.WriteLine();

            // read the file from stdin or from the command line arguments
            string capFile = "C:/Games/l2-1.pcap";
            Console.WriteLine("opening '{0}'", capFile);

            ICaptureDevice device;

            try
            {
                // Get an offline device
                device = new CaptureFileReaderDevice(capFile);
                // Open the device
                device.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception when opening file" + e.ToString());
                return;
            }

            IKernel kernel = new StandardKernel();
            kernel.Bind<IPacketHandler<EthernetPacket>>().To<EthernetPacketHandler>();
            kernel.Bind<IPacketHandler<IPv4Packet>>().To<IpV4PacketHandler>();
            kernel.Bind<IPacketHandler<TcpPacket>>().To<TcpStreamSplitHandler>();
            kernel.Bind<ITcpStreamHandlerProvider>().To<TcpSegmentsSplitterProvider>();

            kernel.Bind<IDatagramStreamReaderProvider>().To<L2DatagramStreamReader.Provider>();

            var datagramAccumulator = kernel.Get<DummyDatagramHandler.Provider>();
            kernel.Bind<IDatagramStreamHandlerProvider>().ToMethod(context => datagramAccumulator);


            var packetHandlerProvider = kernel.Get<PacketHandlerProvider>();
            kernel.Bind<IPacketHandlerProvider>().ToMethod(context => packetHandlerProvider);
            kernel.Bind<IPacketHandlerRegistry>().ToMethod(context => packetHandlerProvider);

            var handlerRegistry = kernel.Get<IPacketHandlerRegistry>();
            handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<EthernetPacket>>());
            handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<IPv4Packet>>());
            handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<TcpPacket>>());

            kernel.Get<CaptureProcessor>().ProcessCapture(device);
        }
    }
}