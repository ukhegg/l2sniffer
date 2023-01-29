using Interceptor;

namespace L2Clicker;

public class Program
{
    public static int Main(string[] args)
    {
        Input input = new Input();
        input.KeyboardFilterMode = KeyboardFilterMode.All;
        Console.WriteLine("loading Interceptor driver");
        if (!input.Load())
        {
            Console.WriteLine("failed to load driver");
            return -1;
        }

        input.KeyPressDelay = 1;
        var windowProvider = new ApplicationWindowProvider(input);

        var l2Processes = new Dictionary<int, L2Process>();

        while (true)
        {
            var processes = L2Process.FindL2Processes(windowProvider);

            foreach (var process in processes)
            {
                if (l2Processes.ContainsKey(process.Pid)) continue;

                Console.WriteLine(
                    $"Found l2 window with hwnd 0x{process.MainWindow.Hwnd:x8},Title {process.MainWindow.Title}");
                l2Processes[process.Pid] = process;

                foreach (var endPoint in process.OpenPorts)
                {
                    Console.WriteLine($"    open connection:{endPoint.LocalEndpoint}->{endPoint.RemoteEndpoint}");
                }

                Console.ReadKey();
                Console.WriteLine("doing some work)");
                Thread.Sleep(500);
                Console.WriteLine("NOW!");
                var controller = new L2WindowController(process);

                while (true)
                {
                    controller.ChangeShortcutPanel(3);
                    //controller.Stand();
                    //controller.SocialDance();
                    //controller.Sit();
                    Thread.Sleep(1000);
                    controller.ChangeShortcutPanel(1);
                    Thread.Sleep(100);
                }

            }


            Thread.Sleep(100);
        }
    }
}