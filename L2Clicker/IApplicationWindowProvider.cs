using Interceptor;

namespace L2Clicker;

public interface IApplicationWindowProvider
{
    ApplicationWindow GetApplicationWindow(IntPtr hwnd);
}

public class ApplicationWindowProvider : IApplicationWindowProvider
{
    private Input _input;

    public ApplicationWindowProvider(Input input)
    {
        _input = input;
    }

    public ApplicationWindow GetApplicationWindow(IntPtr hwnd)
    {
        return new ApplicationWindow(hwnd, _input);
    }
}