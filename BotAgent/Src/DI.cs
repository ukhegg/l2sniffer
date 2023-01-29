using System.Net;
using L2sniffer;
using L2sniffer.Crypto;
using L2sniffer.L2PacketHandlers;
using l2sniffer.PacketHandlers;
using L2sniffer.PacketHandlers;
using L2sniffer.StreamHandlers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ninject;
using PacketDotNet;

namespace BotAgent;

public class DI
{
    public static void SetupServices(IServiceCollection services)
    {
        var kernel = GetKernel();

        services.TryAddSingleton(kernel.Get<ICaptureProcessor>());
        services.TryAddSingleton(kernel.Get<INetworkSniffer>());
        services.TryAddSingleton(kernel.Get<IL2ServerRegistry>());
        services.TryAddSingleton(kernel.Get<INotifyL2SessionFound>());
        services.TryAddSingleton(kernel.Get<IL2SessionPacketAsyncProvider>());
    }

    static IKernel GetKernel()
    {
        IKernel kernel = new StandardKernel();

        kernel.Bind<INetworkSniffer>().To<NetworkSniffer>().InSingletonScope();
        kernel.Bind<ICaptureProcessor>().To<CaptureProcessor>().InSingletonScope();


//l2 handlers
        var decryptorProvider = kernel.Get<PacketDecryptorProvider>();
        kernel.Bind<IPacketDecryptorProvider>().ToMethod(context => decryptorProvider);
        kernel.Bind<ISessionCryptKeysRegistry>().ToMethod(context => decryptorProvider);
        kernel.Bind<IDatagramStreamReaderProvider>().To<L2DatagramStreamReader.Provider>();
        var l2StreamHandlerProvider = kernel.Get<L2StreamHandlerProvider>();
        kernel.Bind<IL2StreamsHandlerProvider>().ToMethod(context => l2StreamHandlerProvider);
        kernel.Bind<INotifyL2SessionFound>().ToMethod(context => l2StreamHandlerProvider);
        kernel.Bind<IL2SessionPacketAsyncProvider>().ToMethod(context => l2StreamHandlerProvider);

//infrustructure
        kernel.Bind<IL2PacketLogger>().To<ConsoleWritingPacketLogger>();
        kernel.Bind<IL2ServerRegistry>().To<L2ServerRegistry>().InSingletonScope();

//datagam handlers

        kernel.Bind<IDatagramStreamHandlerProvider>().To<TcpSegmentsSplitterProvider>()
            .WhenInjectedInto<TcpStreamSplitter>();
        var l2DatagramHandlerProvider = kernel.Get<L2DecryptingDatagramHandlerProvider>();
        kernel.Bind<IDatagramStreamHandlerProvider>().ToMethod(context => l2DatagramHandlerProvider)
            .WhenInjectedInto<TcpSegmentsSplitterProvider>();

//packet handlers bindings
        kernel.Bind<IPacketHandler<EthernetPacket>>().To<EthernetPacketHandler>();
        kernel.Bind<IPacketHandler<IPv4Packet>>().To<IpV4PacketHandler>();
        kernel.Bind<ITcpAssemblerProvider>().To<TcpReordererProvider>();
        kernel.Bind<IPacketHandler<TcpPacket>>().To<TcpStreamSplitter>();


        var packetHandlerProvider = kernel.Get<PacketHandlerProvider>();
        kernel.Bind<IPacketHandlerProvider>().ToMethod(context => packetHandlerProvider);
        kernel.Bind<IPacketHandlerRegistry>().ToMethod(context => packetHandlerProvider);

        var handlerRegistry = kernel.Get<IPacketHandlerRegistry>();
        handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<EthernetPacket>>());
        handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<IPv4Packet>>());
        handlerRegistry.RegisterPacketHandler(kernel.Get<IPacketHandler<TcpPacket>>());


        return kernel;
    }
}