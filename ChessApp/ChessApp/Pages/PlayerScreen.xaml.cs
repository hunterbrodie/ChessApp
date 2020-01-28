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

            pTitle.FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label));
            pTitle.Text = "Rating";
            pTitle.TextColor = Color.Black;
            pSubtitle.FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label));
            pSubtitle.Text = _player.Rating.ToString();
            pSubtitle.TextColor = Color.Gray;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            List<Game> allGames = await App.Database.GetGameListAsync();
            for (int x = 0; x < allGames.Count; x++)
            {
                if (allGames[x].p1ID != _player.ID && allGames[x].p2ID != _player.ID)
                {
                    allGames.RemoveAt(x);
                    x--;
                }
            }
            allGames.OrderByDescending(g => g.gDate);
            pGamesListView.ItemsSource = allGames;
            
            List<Microcharts.Entry> entries = new List<Microcharts.Entry>();

            allGames = oneGameDay(allGames);
            allGames.OrderBy(g => g.gDate);

            foreach (Game _game in allGames)
            {
                double rating = _game.p1Rating;

                if (_game.p2ID == _player.ID)
                {
                    rating = _game.p2Rating;
                }

                entries.Add(new Microcharts.Entry((float)(rating))
                {
                    Label = _game.gDate.ToShortDateString(),
                    ValueLabel = rating.ToString(),
                });
            }

            playerRatingChart.Chart = new LineChart() { Entries = entries };

        }

        private async void DeletePlayer_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This delete the player, continue?", "Yes, delete", "Cancel"))
            {
                await App.Database.DeletePlayer(_player);
                await DisplayAlert("Info", "Player deleted", "OK");
                await Navigation.PopModalAsync();
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
                await App.Database.DeleteGame((Game)(pGamesListView.SelectedItem));
                await DisplayAlert("Info", "Game deleted", "OK");
                List<Game> _gameList = await App.Database.GetGameListAsync();
                pGamesListView.ItemsSource = _gameList.OrderByDescending(p => p.gDate);
            }
        }
    }
}