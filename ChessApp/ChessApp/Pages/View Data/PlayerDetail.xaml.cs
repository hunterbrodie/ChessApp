using ChessApp.Classes;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChessApp.Pages.View_Data
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerDetail : ContentPage
    {
        private Player _player;
        private List<Game> _gameList;
        private int[] wlt = { 0, 0, 0 };
        public PlayerDetail(Player _player)
        {
            InitializeComponent();
            this._player = _player;

            PlayerNameEntry.Text = _player.PName;

            if (Device.RuntimePlatform == Device.Android)
            {
                EditIconName.HeightRequest = 48;
            }


            playerRatingLabel.Text = "Rating: " + _player.Rating.ToString();
            playerRatingLabel.FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label));
            playerRatingLabel.TextColor = Color.Gray;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            playerRatingLabel.Text = "Rating: " + _player.Rating.ToString();

            GetGamesList();

            GenerateGraph();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.Database.UpdatePlayerAsync(_player).Wait();
        }

        private async void DeletePlayerButton_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This delete the player, continue?", "Yes, delete", "Cancel"))
            {
                App.Database.DeletePlayer(_player);
                await DisplayAlert("Info", "Player deleted", "OK");
                await Navigation.PopAsync();
            }
        }
        
        private async void playerGamesView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (await DisplayAlert("WARNING", "This delete the game, continue?", "Yes, delete", "Cancel"))
            {
                PlayerGameDisplay temp = (PlayerGameDisplay)(playerGamesView.SelectedItem);
                App.Database.DeleteGameFromID(temp.gID);
                await DisplayAlert("Info", "Game deleted", "OK");

                GetGamesList();

                _player = await App.Database.GetPlayerAsync(_player.ID);
                playerRatingLabel.Text = "Rating: " + _player.Rating.ToString();

                GenerateGraph();
            }
        }

        private void GraphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateGraph();
        }

        private void GetGamesList()
        {
            List<PlayerGameDisplay> gameViewSrc = new List<PlayerGameDisplay>();
            _gameList = App.Database.GetGameListAsync().Result;
            for (int x = 0; x < _gameList.Count; x++)
            {
                if (_gameList[x].p1ID != _player.ID && _gameList[x].p2ID != _player.ID)
                {
                    _gameList.RemoveAt(x);
                    x--;
                }
            }
            _gameList = _gameList.OrderBy(g => g.gDate).ToList();

            for (int x = 0; x < _gameList.Count; x++)
            {
                if (_gameList[x].p1ID == _player.ID)
                {
                    if (_gameList[x].p1Result == 1)
                    {
                        wlt[0]++;

                        double change;

                        if (x < _gameList.Count - 1)
                        {
                            if (_gameList[x + 1].p1ID == _player.ID)
                            {
                                change = _gameList[x + 1].p1Rating - _gameList[x].p1Rating;
                            }
                            else
                            {
                                change = _gameList[x + 1].p2Rating - _gameList[x].p1Rating;
                            }
                        }
                        else
                        {
                            change = _player.Rating - _gameList[x].p1Rating;
                        }

                        gameViewSrc.Add(new PlayerGameDisplay()
                        {
                            gID = _gameList[x].ID,
                            Opponent = App.Database.GetPlayerAsync(_gameList[x].p2ID).Result.PName,
                            gDate = _gameList[x].gDate,
                            wlt = "Win",
                            change = Math.Round(change, 2)
                        });
                    }
                    else if (_gameList[x].p1Result == 0)
                    {
                        wlt[1]++;

                        double change;

                        if (x < _gameList.Count - 1)
                        {
                            if (_gameList[x + 1].p1ID == _player.ID)
                            {
                                change = _gameList[x + 1].p1Rating - _gameList[x].p1Rating;
                            }
                            else
                            {
                                change = _gameList[x + 1].p2Rating - _gameList[x].p1Rating;
                            }
                        }
                        else
                        {
                            change = _player.Rating - _gameList[x].p1Rating;
                        }
                        gameViewSrc.Add(new PlayerGameDisplay()
                        {
                            gID = _gameList[x].ID,
                            Opponent = App.Database.GetPlayerAsync(_gameList[x].p2ID).Result.PName,
                            gDate = _gameList[x].gDate,
                            wlt = "Loss",
                            change = Math.Round(change, 2)
                        });
                    }
                    else
                    {
                        wlt[2]++;

                        double change;

                        if (x < _gameList.Count - 1)
                        {
                            if (_gameList[x + 1].p1ID == _player.ID)
                            {
                                change = _gameList[x + 1].p1Rating - _gameList[x].p1Rating;
                            }
                            else
                            {
                                change = _gameList[x + 1].p2Rating - _gameList[x].p1Rating;
                            }
                        }
                        else
                        {
                            change = _player.Rating - _gameList[x].p1Rating;
                        }
                        gameViewSrc.Add(new PlayerGameDisplay()
                        {
                            gID = _gameList[x].ID,
                            Opponent = App.Database.GetPlayerAsync(_gameList[x].p2ID).Result.PName,
                            gDate = _gameList[x].gDate,
                            wlt = "Tie",
                            change = Math.Round(change, 2)
                        });
                    }

                }
                else if (_gameList[x].p2ID == _player.ID)
                {
                    if (_gameList[x].p1Result == 1)
                    {
                        wlt[1]++;

                        double change;

                        if (x < _gameList.Count - 1)
                        {
                            if (_gameList[x + 1].p1ID == _player.ID)
                            {
                                change = _gameList[x + 1].p1Rating - _gameList[x].p2Rating;
                            }
                            else
                            {
                                change = _gameList[x + 1].p2Rating - _gameList[x].p2Rating;
                            }
                        }
                        else
                        {
                            change = _player.Rating - _gameList[x].p2Rating;
                        }
                        gameViewSrc.Add(new PlayerGameDisplay()
                        {
                            gID = _gameList[x].ID,
                            Opponent = App.Database.GetPlayerAsync(_gameList[x].p1ID).Result.PName,
                            gDate = _gameList[x].gDate,
                            wlt = "Loss",
                            change = Math.Round(change, 2)
                        });
                    }
                    else if (_gameList[x].p1Result == 0)
                    {
                        wlt[0]++;

                        double change;

                        if (x < _gameList.Count - 1)
                        {
                            if (_gameList[x + 1].p1ID == _player.ID)
                            {
                                change = _gameList[x + 1].p1Rating - _gameList[x].p2Rating;
                            }
                            else
                            {
                                change = _gameList[x + 1].p2Rating - _gameList[x].p2Rating;
                            }
                        }
                        else
                        {
                            change = _player.Rating - _gameList[x].p2Rating;
                        }
                        gameViewSrc.Add(new PlayerGameDisplay()
                        {
                            gID = _gameList[x].ID,
                            Opponent = App.Database.GetPlayerAsync(_gameList[x].p1ID).Result.PName,
                            gDate = _gameList[x].gDate,
                            wlt = "Win",
                            change = Math.Round(change, 2)
                        });
                    }
                    else
                    {
                        wlt[2]++;

                        double change;

                        if (x < _gameList.Count - 1)
                        {
                            if (_gameList[x + 1].p1ID == _player.ID)
                            {
                                change = _gameList[x + 1].p1Rating - _gameList[x].p2Rating;
                            }
                            else
                            {
                                change = _gameList[x + 1].p2Rating - _gameList[x].p2Rating;
                            }
                        }
                        else
                        {
                            change = _player.Rating - _gameList[x].p2Rating;
                        }
                        gameViewSrc.Add(new PlayerGameDisplay()
                        {
                            gID = _gameList[x].ID,
                            Opponent = App.Database.GetPlayerAsync(_gameList[x].p1ID).Result.PName,
                            gDate = _gameList[x].gDate,
                            wlt = "Tie",
                            change = Math.Round(change, 2)
                        });
                    }
                }
            }



            playerGamesView.ItemsSource = gameViewSrc.OrderByDescending(g => g.gDate);
        }

        private void GenerateGraph()
        {
            if (_gameList != null)
            {
                if (GraphType.SelectedItem.Equals("Rating"))
                {
                    List<Microcharts.Entry> entries = new List<Microcharts.Entry>();
                    List<Game> gameDayList = oneGameDay(_gameList);
                    gameDayList = gameDayList.OrderByDescending(g => g.gDate).ToList();

                    double curRating = _player.Rating;

                    for (DateTime _date = DateTime.Today; _date.Date >= DateTime.Today.AddDays(-56).Date; _date = _date.AddDays(-7))
                    {
                        foreach (Game _g in gameDayList)
                        {
                            if (_g.gDate.Date > _date.Date)
                            {
                                curRating = _g.p1Rating;

                                if (_g.p2ID == _player.ID)
                                {
                                    curRating = _g.p2Rating;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        entries.Add(new Microcharts.Entry((float)(curRating - 1000))
                        {
                            Label = _date.ToString("MM/dd"),
                            ValueLabel = curRating.ToString()
                        });

                    }

                    entries.Reverse();

                    playerChartView.Chart = new LineChart() { Entries = entries, LabelTextSize = 28 };
                }
                else
                {
                    Microcharts.Entry[] entries = new Microcharts.Entry[3];
                    entries[0] = new Microcharts.Entry(wlt[0])
                    {
                        Label = "Wins",
                        ValueLabel = wlt[0].ToString(),
                        Color = SKColor.Parse("#77dd77")
                    };
                    entries[1] = new Microcharts.Entry(wlt[1])
                    {
                        Label = "Losses",
                        ValueLabel = wlt[1].ToString(),
                        Color = SKColor.Parse("#ff6961")
                    };
                    entries[2] = new Microcharts.Entry(wlt[2])
                    {
                        Label = "Ties",
                        ValueLabel = wlt[2].ToString(),
                        Color = SKColor.Parse("#AEC6CF")
                    };

                    playerChartView.Chart = new DonutChart() { Entries = entries, LabelTextSize = 28 };
                }
            }
        }

        private List<Game> oneGameDay(List<Game> oneGameList)
        {
            for (int x = 0; x < oneGameList.Count - 1; x++)
            {
                if (oneGameList[x].gDate.Date.Equals(oneGameList[x + 1].gDate.Date))
                {
                    oneGameList.RemoveAt(x + 1);
                    x--;
                }
            }
            return oneGameList;
        }

        private void PlayerNameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            _player.PName = PlayerNameEntry.Text;
        }
    }

    public class PlayerGameDisplay
    {
        public string Opponent { get; set; }
        public DateTime gDate { get; set; }
        public string wlt { get; set; }
        public Color wltColor
        {
            get
            {
                if (wlt.Equals("Win"))
                {
                    return Color.FromHex("#77dd77");
                }
                else if (wlt.Equals("Loss"))
                {
                    return Color.FromHex("#ff6961");
                }
                else
                {
                    return Color.FromHex("#AEC6CF");
                }
            }
        }
        public int gID { get; set; }
        public double change { get; set; }
        public string changeStr
        {
            get
            {
                if (change >= 0)
                {
                    return "+" + change;
                }
                else
                {
                    return change.ToString();
                }
            }
        }
        public Color changeColor
        {
            get
            {
                if (change > 0)
                {
                    return Color.FromHex("#77dd77");
                }
                else if (change < 0)
                {
                    return Color.FromHex("#ff6961");
                }
                else
                {
                    return Color.FromHex("#AEC6CF");
                }
            }
        }
        public string gDateStr { get { return gDate.ToShortDateString(); } }
    }
}