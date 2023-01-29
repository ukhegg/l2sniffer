using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using ItsSimple.NetStatData;


namespace L2Clicker;

public class L2Process
{
    delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);


    [DllImport("user32.dll")]
    static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn,
                                         IntPtr lParam);

    static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
    {
        var handles = new List<IntPtr>();

        foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
            EnumThreadWindows(thread.Id,
                              (hWnd, lParam) =>
                              {
                                  handles.Add(hWnd);
                                  return true;
                              }, IntPtr.Zero);

        return handles;
    }

    private readonly Process _process;
    private ApplicationWindow _mainWnd;

    private L2Process(Process process, IApplicationWindowProvider windowProvider)
    {
        _process = process;
        _mainWnd = EnumerateProcessWindowHandles(process.Id)
            .Select(arg => windowProvider.GetApplicationWindow(arg))
            .First(window => window.Title == "Lineage II");
    }

    public ApplicationWindow MainWindow => _mainWnd;

    public int Pid => _process.Id;

    public ConnectionInfo[] OpenPorts
    {
        get
        {
#pragma warning disable CA1416
            var connections = NetStatData
                .GetTcpConnectionsWithProcessInformation(pid => { return pid.owningPid == Pid; }).ToArray();


            return connections.Select(item =>
            {
                var localEndpoint = new IPEndPoint(new IPAddress(item.LocalAddr), item.LocalPort);
                var remoteEndpoint = new IPEndPoint(new IPAddress(item.RemoteAddr), item.RemotePort);
                return new ConnectionInfo() { LocalEndpoint = localEndpoint, RemoteEndpoint = remoteEndpoint };
            }).ToArray();
#pragma warning restore CA1416
        }
    }

    public static L2Process[] FindL2Processes(IApplicationWindowProvider windowProvider)
    {
        var result = new List<L2Process>();
        foreach (var process in Process.GetProcesses())
        {
            if (process.ProcessName == "L2")
            {
                result.Add(new L2Process(process, windowProvider));
            }
        }

        return result.ToArray();
    }
}