﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessApp.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChessApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameList : ContentPage
    {

        public GameList()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            App.Database.RecalculateRatings().Wait();
            List<Game> _gameList = await App.Database.GetGameListAsync();
            gameView.ItemsSource = _gameList.OrderByDescending(p => p.gDate);
        }

        private async void gameView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /*if (await DisplayAlert("WARNING", "This delete the game, continue?", "Yes, delete", "Cancel"))
            {
                App.Database.DeleteGame((Game)(gameView.SelectedItem));
                await DisplayAlert("Info", "Game deleted", "OK");
                List<Game> _gameList = await App.Database.GetGameListAsync();
                gameView.ItemsSource = _gameList.OrderByDescending(p => p.gDate);
            }*/
            if (await DisplayAlert("Game Info", ((Game)(gameView.SelectedItem)).Disp, "Delete?", "Back"))
            {
                if (await DisplayAlert("WARNING", "This delete the game, continue?", "Yes, delete", "Cancel"))
                {
                    App.Database.DeleteGame((Game)(gameView.SelectedItem));
                    await DisplayAlert("Info", "Game deleted", "OK");
                    List<Game> _gameList = await App.Database.GetGameListAsync();
                    gameView.ItemsSource = _gameList.OrderByDescending(p => p.gDate);
                }
            }
        }

        private void AddGameToolBar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddGame() 
            { 
                Title = "Add Game"
            });
        }
    }
}