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
    public partial class AddPlayer : ContentPage
    {
        public AddPlayer()
        {
            InitializeComponent();
        }

        private async void AddPlayerButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PlayerName.Text))
            {
                await App.Database.SavePlayerAsync(new Player
                {
                    PName = PlayerName.Text,
                    Rating = 1000
                });

                PlayerName.Text = string.Empty;
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Fill out all fields correctly", "OK");
            }
        }

        private async void EraseAllData_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This will erase all data, continue?", "Yes, erase", "Cancel"))
            {
                App.Database.ResetPlayerTable();
                App.Database.ResetGameTable();
                await DisplayAlert("Info", "All data erased", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}