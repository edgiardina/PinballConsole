using PinballApi.Extensions;
using PinballApi.Models.WPPR.Universal.Rankings;
using Terminal.Gui;

namespace PinballConsole
{
    public class MainWindow : Window
    {
        ListView ListView { get; set; }

        public MainWindow(RankingDataSource rankingDataSource)
        {
            Title = "IFPA";

            var listViewDataSource = rankingDataSource;
            ListView = new ListView(listViewDataSource)
            {
                X = 0,
                Y = 0,
                Height = Dim.Fill(0),
                Width = Dim.Percent(40),
            };

            Add(ListView);

            var _scrollBar = new ScrollBarView(ListView, true);

            _scrollBar.ChangedPosition += () =>
            {
                ListView.TopItem = _scrollBar.Position;
                if (ListView.TopItem != _scrollBar.Position)
                {
                    _scrollBar.Position = ListView.TopItem;
                }
                ListView.SetNeedsDisplay();
            };

            _scrollBar.OtherScrollBarView.ChangedPosition += () =>
            {
                ListView.LeftItem = _scrollBar.OtherScrollBarView.Position;
                if (ListView.LeftItem != _scrollBar.OtherScrollBarView.Position)
                {
                    _scrollBar.OtherScrollBarView.Position = ListView.LeftItem;
                }
                ListView.SetNeedsDisplay();
            };

            ListView.DrawContent += (e) =>
            {
                _scrollBar.Size = ListView.Source.Count - 1;
                _scrollBar.Position = ListView.TopItem;
                _scrollBar.OtherScrollBarView.Size = ListView.Maxlength - 1;
                _scrollBar.OtherScrollBarView.Position = ListView.LeftItem;
                _scrollBar.Refresh();
            };


            var rightPane = new FrameView("Player")
            {
                X = Pos.Right(ListView),
                Y = 0,
                Width = Dim.Fill(1),
                Height = Dim.Fill(0),

                CanFocus = true,
                Shortcut = Key.CtrlMask | Key.S,
            };
            Add(rightPane);

            var playerInfoFrame = new FrameView("Player Info")
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill(1),
                Height = 10,
            };

            var rankLabel = new Label("Rank: ") { X = 1, Y = 1 };
            playerInfoFrame.Add(rankLabel);

            var leftButton = new Button("Tournament Results")
            {
                Y = Pos.AnchorEnd() - 1
            };
            leftButton.Clicked += () =>
            {

            };

            // show positioning vertically using Pos.AnchorEnd
            var centerButton = new Button("Player v Player")
            {
                X = Pos.Center(),
                Y = Pos.AnchorEnd() - 1
            };
            centerButton.Clicked += () =>
            {

            };

            // show positioning vertically using another window and Pos.Bottom
            var rightButton = new Button("Championship Series")
            {
                Y = Pos.Y(centerButton)
            };
            rightButton.Clicked += () =>
            {

            };

            // Center three buttons with 5 spaces between them
            // TODO: Use Pos.Width instead of (Right-Left) when implemented (#502)
            leftButton.X = Pos.Left(centerButton) - (Pos.Right(leftButton) - Pos.Left(leftButton)) - 5;
            rightButton.X = Pos.Right(centerButton) + 5;

            rightPane.Add(leftButton);
            rightPane.Add(centerButton);
            rightPane.Add(rightButton);

            rightPane.Add(playerInfoFrame);

            ListView.SelectedItemChanged += (e) =>
            {
                var player = (Ranking)e.Value;
                rightPane.Title = $"{player.Name}";
                rankLabel.Text = "Rank: " + player.CurrentRank.OrdinalSuffix();
            };

            // Quit when Q is pressed
            this.KeyDown += (e) =>
            {
                if (e.KeyEvent.Key == Key.Q || e.KeyEvent.Key == Key.q)
                {
                    Application.RequestStop();
                }
            };

        }

        public override async void OnLoaded()
        {
            base.OnLoaded();
            ListView.SelectedItem = 0;

        }
    }
}
