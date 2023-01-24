using System.Net;
using L2sniffer;
using L2sniffer.Crypto;
using L2sniffer.GameData;
using L2sniffer.GameState.GameObjects;
using L2sniffer.L2PacketHandlers;
using l2sniffer.PacketHandlers;
using L2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;
using Ninject;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace SnifferApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"SharpPcap {Pcap.SharpPcapVersion}, ReadingCaptureFile");
            Console.WriteLine();
            // read the file from stdin or from the command line arguments
            string capFile = "C:/Games/l2big.pcap";
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

            //l2 handlers
            var decryptorProvider = kernel.Get<PacketDecryptorProvider>();
            kernel.Bind<IPacketDecryptorProvider>().ToMethod(context => decryptorProvider);
            kernel.Bind<ISessionCryptKeysRegistry>().ToMethod(context => decryptorProvider);
            kernel.Bind<IDatagramStreamReaderProvider>().To<L2DatagramStreamReader.Provider>();

            //infrustructure
            kernel.Bind<IL2PacketLogger>().To<ConsoleWritingPacketLogger>();
            kernel.Bind<IL2ServerRegistry>().To<L2ServerRegistry>().InSingletonScope();
            kernel.Bind<INpcNameProvider>().To<NpcNameProvider>().InSingletonScope();
            kernel.Bind<string>().ToConstant("npcname-e.txt").WhenInjectedInto<NpcNameProvider>();
            kernel.Bind<IItemNameProvider>().To<ItemNameProvider>().InSingletonScope();
            kernel.Bind<string>().ToConstant("itemname-e.txt").WhenInjectedInto<ItemNameProvider>();
            kernel.Bind<IActionNameProvider>().To<ActionNameProvider>().InSingletonScope();
            kernel.Bind<string>().ToConstant("actionname-e.txt").WhenInjectedInto<ActionNameProvider>();
            kernel.Bind<IGameObjectsProvider>().To<GameObjectsProvider>().InSingletonScope();
            kernel.Bind<IGameObjectsRegistry>().To<GameObjectRegistry>().InSingletonScope();


            //packet handlers bindings
            kernel.Bind<IPacketHandler<EthernetPacket>>().To<EthernetPacketHandler>();
            kernel.Bind<IPacketHandler<IPv4Packet>>().To<IpV4PacketHandler>();
            kernel.Bind<ITcpAssemblerProvider>().To<TcpReordererProvider>();
            kernel.Bind<IPacketHandler<TcpPacket>>().To<TcpStreamSplitter>();

            //datagam handlers
            var l2DatagramHandlerProvider = kernel.Get<L2DatagramHandlerProvider>();
            kernel.Bind<IDatagramStreamHandlerProvider>().ToMethod(context => l2DatagramHandlerProvider)
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


            kernel.Get<IL2ServerRegistry>().RegisterLoginServer(
                new IPEndPoint(IPAddress.Parse("83.166.99.220"), 2106));

            kernel.Get<CaptureProcessor>().ProcessCapture(device, 0);

            Console.WriteLine();
            Console.WriteLine("Read done!");
            Console.Read();
        }
    }
}