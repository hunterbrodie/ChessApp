﻿using ChessApp.Classes;
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
        private int[] whiteWLT = { 0, 0, 0 };
        private int[] blackWLT = { 0, 0, 0 };



        public PlayerDetail(Player _player)
        {
            InitializeComponent();
            this._player = _player;

            PlayerNameEntry.Text = _player.PName;
            EditIconName.HeightRequest = 48;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

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
                        
                        whiteWLT[0]++;

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
                        whiteWLT[1]++;

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
                        whiteWLT[2]++;

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
                        blackWLT[1]++;

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
                        blackWLT[0]++;

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
                        blackWLT[2]++;

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

            string[] picker = { "Rating: " + _player.Rating.ToString(), "Wins/Losses/Ties", "White/Black Stats" };
            GraphType.ItemsSource = picker;

            playerGamesView.ItemsSource = gameViewSrc.OrderByDescending(g => g.gDate);
        }

        private void GenerateGraph()
        {
            if (_gameList != null && GraphType.SelectedItem != null)
            {
                if (GraphType.SelectedItem.ToString().Substring(0, 6).Equals("Rating"))
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
                else if (GraphType.SelectedItem.ToString().Equals("White/Black Stats"))
                {
                    Microcharts.Entry[] entries = new Microcharts.Entry[6];
                    entries[0] = new Microcharts.Entry(whiteWLT[0])
                    {
                        Label = "White Wins",
                        ValueLabel = whiteWLT[0].ToString(),
                        Color = SKColor.Parse("#77dd77")
                    };
                    entries[1] = new Microcharts.Entry(whiteWLT[1])
                    {
                        Label = "White Losses",
                        ValueLabel = whiteWLT[1].ToString(),
                        Color = SKColor.Parse("#ff6961")
                    };
                    entries[2] = new Microcharts.Entry(whiteWLT[2])
                    {
                        Label = "White Ties",
                        ValueLabel = whiteWLT[2].ToString(),
                        Color = SKColor.Parse("#AEC6CF")
                    };
                    entries[5] = new Microcharts.Entry(blackWLT[0])
                    {
                        Label = "Black Wins",
                        ValueLabel = blackWLT[0].ToString(),
                        Color = SKColor.Parse("#1d731d")
                    };
                    entries[4] = new Microcharts.Entry(blackWLT[1])
                    {
                        Label = "Black Losses",
                        ValueLabel = blackWLT[1].ToString(),
                        Color = SKColor.Parse("#9c0800")
                    };
                    entries[3] = new Microcharts.Entry(blackWLT[2])
                    {
                        Label = "Black Ties",
                        ValueLabel = blackWLT[2].ToString(),
                        Color = SKColor.Parse("#36515b")
                    };

                    playerChartView.Chart = new DonutChart() { Entries = entries, LabelTextSize = 28 };
                }
                else
                {
                    Microcharts.Entry[] entries = new Microcharts.Entry[3];
                    entries[0] = new Microcharts.Entry(blackWLT[0] + whiteWLT[0])
                    {
                        Label = "Wins",
                        ValueLabel = (whiteWLT[0] + blackWLT[0]).ToString(),
                        Color = SKColor.Parse("#77dd77")
                    };
                    entries[1] = new Microcharts.Entry((whiteWLT[1] + blackWLT[1]))
                    {
                        Label = "Losses",
                        ValueLabel = (whiteWLT[1] + blackWLT[1]).ToString(),
                        Color = SKColor.Parse("#ff6961")
                    };
                    entries[2] = new Microcharts.Entry((whiteWLT[2] + blackWLT[2]))
                    {
                        Label = "Ties",
                        ValueLabel = (whiteWLT[2] + blackWLT[2]).ToString(),
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