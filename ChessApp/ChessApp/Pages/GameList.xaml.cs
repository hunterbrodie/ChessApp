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
        public GameList()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            List<Game> _gameList = await App.Database.GetGameListAsync();
            gameView.ItemsSource = _gameList.OrderBy(p => p.gDate);

        }
    }
}