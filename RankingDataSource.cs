using NStack;
using PinballApi.Extensions;
using PinballApi.Interfaces;
using PinballApi.Models.WPPR.Universal.Rankings;
using System.Collections;
using Terminal.Gui;

namespace PinballConsole
{
    // This is basically the same implementation used by the UICatalog main window
    public class RankingDataSource : IListDataSource
    {
        int _rankColumnWidth = 10;
        private List<Ranking> rankingResults;
        BitArray marks;
        int count, len;

        public RankingDataSource(IPinballRankingApi pinballRankingApi)
        {
            PinballRankingApi = pinballRankingApi;
        }

        IPinballRankingApi PinballRankingApi { get; set; }

        public List<Ranking> RankingResults
        {
            get => rankingResults;
            set
            {
                if (value != null)
                {
                    count = value.Count;
                    marks = new BitArray(count);
                    rankingResults = value;
                    len = GetMaxLengthItem();
                }
            }
        }
        public bool IsMarked(int item)
        {
            if (item >= 0 && item < count)
                return marks[item];
            return false;
        }

        public int Count => RankingResults != null ? RankingResults.Count : 0;

        public int Length => len;


        public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width, int start = 0)
        {
            container.Move(col, line);
            // Equivalent to an interpolated string like $"{Scenarios[item].Name, -widtestname}"; if such a thing were possible
            var s = String.Format(String.Format("{{0,{0}}}", -_rankColumnWidth), RankingResults[item].CurrentRank.OrdinalSuffix());
            RenderUstr(driver, $"{s} {RankingResults[item].Name}", col, line, width, start);
        }

        public void SetMark(int item, bool value)
        {
            if (item >= 0 && item < count)
                marks[item] = value;
        }

        int GetMaxLengthItem()
        {
            if (rankingResults?.Count == 0)
            {
                return 0;
            }

            int maxLength = 0;
            for (int i = 0; i < rankingResults.Count; i++)
            {
                var s = String.Format(String.Format("{{0,{0}}}", -_rankColumnWidth), RankingResults[i].CurrentRank.OrdinalSuffix());
                var sc = $"{s} {RankingResults[i].Name}";
                var l = sc.Length;
                if (l > maxLength)
                {
                    maxLength = l;
                }
            }

            return maxLength;
        }

        // A slightly adapted method from: https://github.com/gui-cs/Terminal.Gui/blob/fc1faba7452ccbdf49028ac49f0c9f0f42bbae91/Terminal.Gui/Views/ListView.cs#L433-L461
        private void RenderUstr(ConsoleDriver driver, ustring ustr, int col, int line, int width, int start = 0)
        {
            int used = 0;
            int index = start;
            while (index < ustr.Length)
            {
                (var rune, var size) = Utf8.DecodeRune(ustr, index, index - ustr.Length);
                var count = System.Rune.ColumnWidth(rune);
                if (used + count >= width) break;
                driver.AddRune(rune);
                used += count;
                index += size;
            }

            while (used < width)
            {
                driver.AddRune(' ');
                used++;
            }
        }

        public IList ToList()
        {
            var players = Task.Run(() => PinballRankingApi.RankingSearch(RankingType.Wppr)).Result;
            RankingResults = players.Rankings.ToList();
            return RankingResults;
        }
    }
}
