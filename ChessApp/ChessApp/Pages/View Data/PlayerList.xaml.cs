using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessApp.Classes;
using ChessApp.Pages.View_Data;
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
            /*switch (Device.RuntimePlatform)
            {
                case Device.iOS:

                    break;
                case Device.Android:

                default:

                    break;
            }*/
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            List<Player> _playerList = await App.Database.GetPlayerListAsync();

            playerView.ItemsSource = _playerList.OrderByDescending(p => p.Rating);
        }

        private async void playerView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new PlayerDetail((Player)(playerView.SelectedItem))
            {
                Title = "Player View"
            });
        }

        private void AddPlayerToolBar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPlayer() 
            {
                Title = "Add Player"
            });
        }
    }

}