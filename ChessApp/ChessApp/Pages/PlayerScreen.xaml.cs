using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessApp.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts.Forms;
using SkiaSharp;
using Microcharts;

namespace ChessApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerScreen : ContentPage
    {
        private Player _player;

        public PlayerScreen()
        {
            InitializeComponent();

        }

        public PlayerScreen(Player _player)
        {
            InitializeComponent();
            this._player = _player;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            playerName.FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label));
            playerName.Text = _player.PName;
            playerName.TextColor = Color.Black;
            playerRating.FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label));
            playerRating.Text = "Rating: " + _player.Rating.ToString();
            playerRating.TextColor = Color.Gray;

            List<Game> allGames = await App.Database.GetGameListAsync();
            for (int x = 0; x < allGames.Count; x++)
            {
                if (allGames[x].p1ID != _player.ID && allGames[x].p2ID != _player.ID)
                {
                    allGames.RemoveAt(x);
                    x--;
                }
            }
            pGamesListView.ItemsSource = allGames.OrderByDescending(g => g.gDate);
            
            List<Microcharts.Entry> entries = new List<Microcharts.Entry>();

            allGames = oneGameDay(allGames);
            allGames = allGames.OrderByDescending(g => g.gDate).ToList();

            double curRating = _player.Rating;

            for (int x = 0; x < 8; x++)
            {
                DateTime curDate = DateTime.Today.AddDays(-7 * x);
                foreach (Game _g in allGames)
                {
                    if (_g.gDate.Date > curDate.Date)
                    {
                        curRating = _g.p1Rating;

                        if (allGames[0].p2ID == _player.ID)
                        {
                            curRating = _g.p2Rating;
                        }
                    }
                }

                entries.Add(new Microcharts.Entry((float)(curRating - 100))
                {
                    Label = curDate.ToString("MM/dd"),
                    ValueLabel = curRating.ToString()
                });
            }
            entries.Reverse();

            LineChart tempChart = new LineChart() { Entries = entries };

            tempChart.LabelTextSize = 36;

            playerRatingChart.Chart = tempChart;
        }

        private async void DeletePlayer_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This delete the player, continue?", "Yes, delete", "Cancel"))
            {
                App.Database.DeletePlayer(_player);
                await DisplayAlert("Info", "Player deleted", "OK");
                await Navigation.PopAsync();
            }
        }

        private List<Game> oneGameDay(List<Game> gameList)
        {
            for (int x = 0; x < gameList.Count - 1; x++)
            {
                if (gameList[x].gDate.Date.Equals(gameList[x + 1].gDate.Date))
                {
                    gameList.RemoveAt(x + 1);
                    x--;
                }
            }
            return gameList;
        }

        private async void pGamesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (await DisplayAlert("WARNING", "This delete the game, continue?", "Yes, delete", "Cancel"))
            {
                App.Database.DeleteGame((Game)(pGamesListView.SelectedItem));
                await DisplayAlert("Info", "Game deleted", "OK");
                List<Game> _gameList = await App.Database.GetGameListAsync();
                pGamesListView.ItemsSource = _gameList.OrderByDescending(p => p.gDate);
                OnAppearing();
            }
        }
    }
}