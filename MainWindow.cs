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
            var listViewDataSource = new RankingDataSource(players.Rankings.ToList());
            ListView = new ListView(listViewDataSource)
            {
                X = 0,
                Y = 0,
                Height = Dim.Fill(2),
                Width = Dim.Percent(40)      
                
            };

            Add(ListView);


            var _scrollBar = new ScrollBarView(ListView, true);

            _scrollBar.ChangedPosition += () => {
                ListView.TopItem = _scrollBar.Position;
                if (ListView.TopItem != _scrollBar.Position)
                {
                    _scrollBar.Position = ListView.TopItem;
                }
                ListView.SetNeedsDisplay();
            };

            _scrollBar.OtherScrollBarView.ChangedPosition += () => {
                ListView.LeftItem = _scrollBar.OtherScrollBarView.Position;
                if (ListView.LeftItem != _scrollBar.OtherScrollBarView.Position)
                {
                    _scrollBar.OtherScrollBarView.Position = ListView.LeftItem;
                }
                ListView.SetNeedsDisplay();
            };

            ListView.DrawContent += (e) => {
                _scrollBar.Size = ListView.Source.Count - 1;
                _scrollBar.Position = ListView.TopItem;
                _scrollBar.OtherScrollBarView.Size = ListView.Maxlength - 1;
                _scrollBar.OtherScrollBarView.Position = ListView.LeftItem;
                _scrollBar.Refresh();
            };
        }

        public override async void OnLoaded()
        {
            base.OnLoaded();            

          
        }
    }
}
