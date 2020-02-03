using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace ChessApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tabbed : Xamarin.Forms.TabbedPage
    {
        public Tabbed()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
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
        }
	}
}