using System;
using System.Collections.Generic;
using System.Text;

namespace TriviaTraverse
{
    public static class GoogleConstants
    {
        public static string AppName = "TriviaTraverse";

        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string iOSClientId = "<insert IOS client ID here>";
        public static string AndroidClientId = "758281653246-64puivlpijhsekmqh1bnrebtmqp0l680.apps.googleusercontent.com";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl = "<insert IOS redirect URL here>:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.758281653246-64puivlpijhsekmqh1bnrebtmqp0l680:/oauth2redirect";
    }

    public static class FacebookConstants
    {
        public static string AppName = "TriviaTraverse";

        // OAuth
        // For Facebook login, configure at https://developers.facebook.com/apps
        public static string iAppId = "1988371428042023";

        // These values do not need changing
        public static string Scope = "email";
        public static string AuthorizeUrl = "https://m.facebook.com/dialog/oauth";
        public static string AccessTokenUrl = "https://m.facebook.com/dialog/oauth/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";


        public static string RedirectUrl = "https://www.facebook.com/connect/login_success.html";
    }
}
