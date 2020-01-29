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
            pGamesListView.ItemsSource = allGames.OrderByDescending(g => g.gDate);
            
            List<Microcharts.Entry> entries = new List<Microcharts.Entry>();

            allGames = oneGameDay(allGames);
            allGames = allGames.OrderBy(g => g.gDate).ToList();

            //double ratingCur = 100;

            //if (Math.Abs(DateTime.Today.Month - allGames[0].gDate.Month) > 3)

            /*for (DateTime date = allGames[0].gDate; date.Date <= DateTime.Today.Date; date = date.AddDays(7))
            {
                if (allGames.Count > 0 && date.Date == allGames[0].gDate.Date)
                {
                    ratingCur = allGames[0].p1Rating;

                    if (allGames[0].p2ID == _player.ID)
                    {
                        ratingCur = allGames[0].p2Rating;
                    }

                    if (date.Day == 15)
                    {
                        entries.Add(new Microcharts.Entry((float)(ratingCur))
                        {
                            ValueLabel = ratingCur.ToString(),
                            Color = SKColor.Parse("#ff0000"),
                            Label = date.ToString("MMMM")
                        });
                    }
                    else
                    {
                        entries.Add(new Microcharts.Entry((float)(ratingCur))
                        {
                            ValueLabel = ratingCur.ToString(),
                            Color = SKColor.Parse("#ff0000")
                        });
                    }

                    allGames.RemoveAt(0);

                }
                else if (date.Day == 15)
                {
                    entries.Add(new Microcharts.Entry((float)(ratingCur))
                    {
                        Label = date.ToString("MMMM")
                    });
                }
                else
                {
                    entries.Add(new Microcharts.Entry((float)(ratingCur)));
                }
            }

            playerRatingChart.Chart = new LineChart() { Entries = entries };*/

        }

        private async void DeletePlayer_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This delete the player, continue?", "Yes, delete", "Cancel"))
            {
                App.Database.DeletePlayer(_player);
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
                App.Database.DeleteGame((Game)(pGamesListView.SelectedItem));
                await DisplayAlert("Info", "Game deleted", "OK");
                List<Game> _gameList = await App.Database.GetGameListAsync();
                pGamesListView.ItemsSource = _gameList.OrderByDescending(p => p.gDate);
            }
        }
    }
}