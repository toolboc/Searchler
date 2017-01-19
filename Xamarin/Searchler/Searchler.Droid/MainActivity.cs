using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Octane.Xam.VideoPlayer.Android;

namespace Searchler.Droid
{
    [Activity(Label = "Searchler", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            FormsVideoPlayer.Init("F6EF91E524643F000F83A2947E8A29E3445871CD");

            LoadApplication(new App());
        }
    }
}

