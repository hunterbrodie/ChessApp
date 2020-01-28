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
    public partial class PlayerScreen : ContentPage
    {
        public PlayerScreen()
        {
            InitializeComponent();
        }
        private Player _player;
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
            allGames.Reverse();
            pGamesListView.ItemsSource = allGames;

            //pSubtitle.Text = allGames[0].ToString();
        }

        private async void DeletePlayer_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("WARNING", "This delete the player, continue?", "Yes, I want to delete", "Cancel"))
            {
                await App.Database.DeletePlayer(_player);
                await DisplayAlert("Info", "Player deleted", "OK");
                await Navigation.PopModalAsync();
            }
        }
    }
}