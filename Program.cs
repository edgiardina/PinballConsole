using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PinballApi;
using PinballApi.Interfaces;
using PinballConsole;
using Terminal.Gui;

Application.Init();

try
{
    //get configuration values from appsettings.json
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    AppSettings settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();

    // use DI to build the main window
    var serviceProvider = new ServiceCollection()
        .AddLogging()
        .AddTransient<RankingDataSource>()
        .AddTransient<MainWindow>()
        .AddSingleton<IPinballRankingApi, PinballRankingApi>(x => new PinballRankingApi(settings.IfpaApiKey))
        .BuildServiceProvider();

    var mainWindow = serviceProvider.GetRequiredService<MainWindow>();

    Application.Run<SplashWindow>();
    Application.Run(mainWindow);
}
finally
{
    Application.Shutdown();
}