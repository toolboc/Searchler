using Searchler.Models;
using Octane.Xam.VideoPlayer.Constants;
using Octane.Xam.VideoPlayer.Licensing;
using System;
using Xamarin.Forms;
using Octane.Xam.VideoPlayer;

namespace Searchler.Pages
{
    public partial class MainMenuPage : MasterDetailPage
    {
        MasterPage masterPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuPage"/> class.
        /// </summary>
        public MainMenuPage()
        {
            masterPage = new MasterPage(this);
            Master = masterPage;
            Detail = new NavigationPage(new WelcomePage());

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
