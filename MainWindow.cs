using Microsoft.Extensions.Configuration;
using PinballApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace PinballConsole
{
    public class MainWindow : Window
    {
        ListView ListView { get; set; }

        PinballRankingApiV2 pinballRankingApiV2 { get; set; }

        public MainWindow()
        {
            //TODO: move this into a ViewModel
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            AppSettings settings = config.GetRequiredSection("AppSettings").Get<AppSettings>();

            pinballRankingApiV2 = new PinballRankingApiV2(settings.IfpaApiKey);

            Title = "IFPA";


            var players = Task.Run(() => pinballRankingApiV2.GetWpprRanking()).Result; 

            ListView = new ListView(players.Rankings.Select(n => n.FirstName + " " + n.LastName).ToList())
            {
                X = 0,
                Y = 0,
                Height = Dim.Fill(2),
                Width = Dim.Percent(40)
            }; ;

            Add(ListView);
        }

        public override async void OnLoaded()
        {
            base.OnLoaded();            

          
        }
    }
}
