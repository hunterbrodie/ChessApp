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
        private List<Game> _gameList;

        public PlayerScreen(Player _player)
        {
            InitializeComponent();
            this._player = _player;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            playerName = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                Text = _player.PName,
                TextColor = Color.Black
            };

            playerRating = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)),
                Text = "Rating: " + _player.Rating.ToString(),
                TextColor = Color.Gray
            };

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
            _gameList = allGames;

            //GenerateGraph();
           
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
                _player = await App.Database.GetPlayerAsync(_player.ID);
                playerRating.Text = "Rating: " + _player.Rating.ToString();
                //GenerateGraph();
            }
        }

        private void GraphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GenerateGraph();
        }

        /*private void GenerateGraph()
        {

            if (GraphType.SelectedItem.Equals("Rating"))
            {
                List<Microcharts.Entry> entries = new List<Microcharts.Entry>();

                _gameList = oneGameDay(_gameList);
                _gameList = _gameList.OrderByDescending(g => g.gDate).ToList();

                double curRating = _player.Rating;

                for (int x = 0; x < 8; x++)
                {
                    DateTime curDate = DateTime.Today.AddDays(-7 * x);
                    foreach (Game _g in _gameList)
                    {
                        if (_g.gDate.Date > curDate.Date)
                        {
                            curRating = _g.p1Rating;

                            if (_gameList[0].p2ID == _player.ID)
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

                playerRatingChart.Chart = new LineChart() { Entries = entries, LabelTextSize = 36 };
            }
            else
            {
                Microcharts.Entry[] entries = new Microcharts.Entry[3];
                int[] wlt = App.Database.playerWinsLossesTies(_player);
                entries[0] = new Microcharts.Entry(wlt[0])
                {
                    Label = "Wins",
                    ValueLabel = wlt[0].ToString(),
                    Color = SKColor.Parse("#b53737")
                };
                entries[1] = new Microcharts.Entry(wlt[1])
                {
                    Label = "Losses",
                    ValueLabel = wlt[1].ToString(),
                    Color = SKColor.Parse("#296d98")
                };
                entries[2] = new Microcharts.Entry(wlt[2])
                {
                    Label = "Ties",
                    ValueLabel = wlt[2].ToString(),
                    Color = SKColor.Parse("#663a82")
                };

                playerRatingChart.Chart = new DonutChart() { Entries = entries, LabelTextSize = 36 };
            }
        }*/
    }
}