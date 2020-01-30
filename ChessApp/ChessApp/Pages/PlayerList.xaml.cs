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
            List<Player> _playerList = await App.Database.GetPlayerListAsync();
            playerView.ItemsSource = _playerList.OrderBy(p => p.Rating).Reverse();
        }

        private async void playerView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new PlayerScreen((Player)(playerView.SelectedItem))
            {
                Title = "Player View"
            });
        }
    }
}