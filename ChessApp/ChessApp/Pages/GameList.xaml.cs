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
    public partial class GameList : ContentPage
    {
        private ListView _listView;

        public GameList()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _listView = this.gameView;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            List<Game> _gameList = await App.Database.GetGameListAsync();
            if (_gameList.Count > 0)
            {
                _listView.ItemsSource = _gameList.OrderByDescending(p => p.gDate);
                listFrame.Content = _listView;
            }
            else
            {
                listFrame.Content = new Label()
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "No games are stored.\n\nClick the edit tab to add players and games.",
                    FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label))
                };
            }
        }

        private async void gameView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
}