using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Xaml;

namespace ChessApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tabbed : TabbedPage
    {
        public Tabbed()
        {
            InitializeComponent();
            Children.Add(new NavigationPage(new PlayerList())
            {
                Title = "Rankings",
                IconImageSource = "ranking_icon"
            });
            Children.Add(new NavigationPage(new GameList())
            {
                Title = "Games",
                IconImageSource = "games_icon"
            });
            Children.Add(new NavigationPage(new Edit())
            {
                Title = "Edit",
                IconImageSource = "edit_icon"
            });
        }
	}
}