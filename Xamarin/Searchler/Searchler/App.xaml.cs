using Searchler.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Searchler
{
    public partial class App : Application
    {
        //public static MobileServiceClient MobileService = new MobileServiceClient("https://searchlerapp.azurewebsites.net");

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainMenuPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
