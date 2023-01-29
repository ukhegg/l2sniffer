using System.Runtime.InteropServices;
using System.Text;
using Interceptor;
using Keyboard;

namespace L2Clicker;

public class ApplicationWindow
{
    private readonly Input _input;

    [DllImport("user32.dll")]
    private static extern int GetWindowText(int hWnd, StringBuilder title, int size);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hwnd);

    [DllImport("user32.dll")]
    internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public IntPtr Hwnd { get; }

    public string Title
    {
        get
        {
            StringBuilder title = new StringBuilder(256);
            GetWindowText((int)Hwnd, title, 256);
            return title.ToString();
        }
    }

    public ApplicationWindow(IntPtr hwnd, Input input)
    {
        _input = input;
        Hwnd = hwnd;
    }

    public void SetForeground()
    {
        SetForegroundWindow(Hwnd);
    }

    public void SendKeys(string text)
    {
        SetForeground();

        _input.SendText(text);
    }

    public void SendChatCommand(string command)
    {
        SetForeground();
        _input.SendKeys(Keys.Enter);
        _input.SendText(command);
        _input.SendKeys(Keys.Enter);
    }

    public void SendModifiedText(string text, bool holdAlt, bool holdShift)
    {
        SetForeground();
        if (holdAlt)
        {
            _input.SendKey(Keys.RightAlt, KeyState.Down);
        }

        if (holdShift)
        {
            _input.SendKey(Keys.LeftShift, KeyState.Down);
        }

        _input.SendText(text);
        if (holdAlt)
        {
            _input.SendKey(Keys.RightAlt, KeyState.Up);
        }

        if (holdShift)
        {
            _input.SendKey(Keys.LeftShift, KeyState.Up);
        }
    }
}