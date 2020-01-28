using System;
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
    public partial class PlayerList : ContentPage
    {
        public PlayerList()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            List<Player> players = await App.Database.GetPlayerListAsync();
            playerView.ItemsSource = players.OrderBy(p => p.Rating).Reverse();
        }

        private async void playerView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PlayerScreen pScreen = new PlayerScreen((Player)(playerView.SelectedItem));
            pScreen.Title = ((Player)(playerView.SelectedItem)).PName;
            NavigationPage navigationPage = new NavigationPage(pScreen);
            await Navigation.PushModalAsync(navigationPage);
        }
    }
}