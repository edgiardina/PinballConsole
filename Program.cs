using PinballConsole;
using Terminal.Gui;

Application.Init();

try
{    
    Application.Run<SplashWindow>();
    Application.Run<MainWindow>();
}
finally
{
    Application.Shutdown();
}