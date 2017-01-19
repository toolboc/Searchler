using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Searchler.Pages
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //VideoPlayer.Play();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            //if (Device.OS == TargetPlatform.Android)
                //VideoPlayer.IsVisible = true;
        }

        /// <summary>
        /// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //VideoPlayer.Pause();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            //if (Device.OS == TargetPlatform.Android)
                //VideoPlayer.IsVisible = false;
        }
    }
}
