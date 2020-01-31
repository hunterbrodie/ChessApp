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
        private ListView _listView;
        public PlayerList()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _listView = this.playerView;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            List<Player> _playerList = await App.Database.GetPlayerListAsync();
            if (_playerList.Count > 0)
            {
                _listView.ItemsSource = _playerList.OrderByDescending(p => p.Rating);
                listFrame.Content = _listView;
            }
            else
            {
                listFrame.Content = new Label()
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "No players are stored.\nClick the edit tab to add players and games.",
                    FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label))
                };
            }
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