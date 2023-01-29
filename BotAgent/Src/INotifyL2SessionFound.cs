using System.ComponentModel;
using System.Net;

namespace BotAgent;

public class L2SessionFoundEventArgs : EventArgs
{
    public L2SessionTypes SessionType { get; set; }
    public ConversationDirections Direction { get; set; }
    public IPEndPoint ServerEndpoint { get; set; }
    public IPEndPoint ClientEndpoint { get; set; }
}

public delegate void L2SessionFoundEventHandler(object? sender, L2SessionFoundEventArgs e);

public interface INotifyL2SessionFound
{
    event L2SessionFoundEventHandler OnL2SessionFound;
}