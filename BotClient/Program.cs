using System.Threading.Channels;
using BotAgent;
using Grpc.Core;
using Grpc.Net.Client;

namespace BotClient;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var channel = GrpcChannel.ForAddress("http://localhost:13642");
        var botClient = new SniffingService.SniffingServiceClient(channel);

        var reply = botClient.StartSniffing(new StartSniffingRequest()
        {
            ServerIp = "83.166.99.220",
            LoginServerPort = 2106
        });
        Console.WriteLine($"Sniffing server 83.166.99.220 through interface {reply.CapturingOnInterface}");

        Console.WriteLine("Waiting for Lineage II sessions...");
        var notifications = botClient.ReceiveL2SessionsNotifications(new GetL2SessionsNotificationsRequest());
        var cts = new CancellationTokenSource();
        try
        {
            await foreach (var sessionData in notifications.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
            {
                Console.WriteLine($"Found new {sessionData.SessionType} session: " +
                                  $"    Server={sessionData.ServerIp}:{sessionData.ServerPort}" +
                                  $"    Client={sessionData.ClientIp}:{sessionData.ClientPort}");
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            Console.WriteLine("Stream cancelled.");
        }

        Console.ReadKey();
        return 0;
    }
}