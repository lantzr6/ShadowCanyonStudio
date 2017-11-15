using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TriviaTraverse.Services;
using Xamarin.Forms;
using Android.Content;
using TriviaTraverse.Droid;
using Android.Util;
using Xamarin.Facebook;
using Xamarin.Facebook.AppEvents;

namespace TriviaTraverse.Droid
{
    [Activity(Label = "TriviaTraverse", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {


        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            //GoogleService._context = this.ApplicationContext;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            

            //FacebookSdk.SdkInitialize(this.ApplicationContext);  //depricated - now auto initializes

            LoadApplication(new App());
        }
    }
}

