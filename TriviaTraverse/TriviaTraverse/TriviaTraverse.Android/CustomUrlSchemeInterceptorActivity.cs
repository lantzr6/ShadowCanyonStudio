using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using TriviaTraverse.Services;

namespace TriviaTraverse.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "com.googleusercontent.apps.758281653246-64puivlpijhsekmqh1bnrebtmqp0l680", "https://www.facebook.com/connect/login_success.html" },
        DataPath = "/oauth2redirect",
        AutoVerify = true)]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            AuthenticationState.Authenticator.OnPageLoading(uri);

            Finish();
        }
    }
}