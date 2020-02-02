using ChessApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChessApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGame : ContentPage
    {
        public AddGame()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Player1.ItemsSource = await App.Database.GetPlayerListAsync();
            Player2.ItemsSource = await App.Database.GetPlayerListAsync();
            GameTime.Time = DateTime.Now.TimeOfDay;
        }

        private async void AddGameButton_Clicked(object sender, EventArgs e)
        {
            if (Player1.SelectedIndex > -1 && Player2.SelectedIndex > -1 && Player1.SelectedIndex != Player2.SelectedIndex)
            {
                Player tempWinner = (Player)(Player1.SelectedItem);
                Player tempLoser = (Player)(Player2.SelectedItem);
                double p1Result = .5;
                if (Result.SelectedItem.Equals("Won Against"))
                {
                    p1Result = 1;
                }
                else if (Result.SelectedItem.Equals("Lost To"))
                {
                    p1Result = 0;
                }
                await App.Database.SaveGameAsync(new Game
                {
                    p1ID = tempWinner.ID,
                    p2ID = tempLoser.ID,
                    gDate = GameDate.Date + GameTime.Time,
                    p1Rating = tempWinner.Rating,
                    p2Rating = tempLoser.Rating,
                    p1Result = p1Result
                });

                GameDate.Date = DateTime.Now;
                Player1.SelectedIndex = -1;
                Player2.SelectedIndex = -1;
                Result.SelectedIndex = -1;

                await App.Database.RecalculateRatings();

                Player1.ItemsSource = await App.Database.GetPlayerListAsync();
                Player2.ItemsSource = await App.Database.GetPlayerListAsync();

                await Navigation.PopAsync();

            }
            else
            {
                await DisplayAlert("Error", "Fill out all fields correctly", "OK");
            }
        }

        private async void EraseGames_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This will erase all games, continue?", "Yes, erase", "Cancel"))
            {
                App.Database.ResetGameTable();
                await DisplayAlert("Info", "All games erased", "OK");
                Player1.ItemsSource = await App.Database.GetPlayerListAsync();
                Player2.ItemsSource = await App.Database.GetPlayerListAsync();
            }
            await Navigation.PopAsync();
        }
    }
}