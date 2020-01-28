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
    public partial class Edit : ContentPage
    {
        public Edit()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Player1.ItemsSource = await App.Database.GetPlayerListAsync();
            Player2.ItemsSource = await App.Database.GetPlayerListAsync();
        }

        async void AddPlayer_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PlayerName.Text))
            {
                await App.Database.SavePlayerAsync(new Player
                {
                    PName = PlayerName.Text,
                    Rating = 100
                });

                PlayerName.Text = string.Empty;
            }
            Player1.ItemsSource = await App.Database.GetPlayerListAsync();
            Player2.ItemsSource = await App.Database.GetPlayerListAsync();
        }



        async void AddGame_Clicked(object sender, EventArgs e)
        {
            if (Player1.SelectedIndex > -1 && Player2.SelectedIndex > -1 && Player1.SelectedIndex != Player2.SelectedIndex)
            {
                Player tempWinner = (Player)(Player1.SelectedItem);
                Player tempLoser = (Player)(Player2.SelectedItem);
                double p1Result = 1;
                /*if (Result.SelectedItem.Equals("Won Against"))
                {
                    p1Result = 1;
                }
                else if (Result.SelectedItem.Equals("Lost To"))
                {
                    p1Result = 0;
                }*/
                await App.Database.SaveGameAsync(new Game
                {
                    p1ID = tempWinner.ID,
                    p2ID = tempLoser.ID,
                    gDate = GameDate.Date.ToShortDateString(),
                    p1Rating = tempWinner.Rating,
                    p2Rating = tempLoser.Rating,
                    p1Result = p1Result
                });

                GameDate.Date = DateTime.Now;
                Player1.SelectedIndex = -1;
                Player2.SelectedIndex = -1;
                //Result.SelectedIndex = -1;

                int temp = await App.Database.RecalculateRatings();

                /*double eA = 1 / (1 + Math.Pow(10, (tempLoser.Rating - tempWinner.Rating) / 40));
                double eB = 1 - eA;

                tempWinner.Rating = Math.Round(tempWinner.Rating + 16 * (1 - eA), 2);
                tempLoser.Rating = Math.Round(tempLoser.Rating + 16 * (0 - eB), 2);

                await App.Database.UpdatePlayerAsync(tempWinner);
                await App.Database.UpdatePlayerAsync(tempLoser);*/

                Player1.ItemsSource = await App.Database.GetPlayerListAsync();
                Player2.ItemsSource = await App.Database.GetPlayerListAsync();

            }
            else
            {
                await DisplayAlert("Error", "Fill out all fields correctly", "OK");
            }
        }

        private async void Erase_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This will erase all data, continue?", "Yes, I want to erase", "Cancel"))
            {
                await App.Database.ResetPlayerTable();
                await App.Database.ResetGameTable();
                await DisplayAlert("Info", "All data erased", "OK");
            }
        }
    }
}