using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Services;
using Xamarin.Auth;
using Xamarin.Forms;
using static Android.Provider.SyncStateContract;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {
        IAuthenticationService _authenticationService { get; }

        public PlayerLocal PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private string _newUserName;
        public string NewUserName
        {
            get { return _newUserName; }
            set
            {
                if (_newUserName != value)
                {
                    _newUserName = value;
                    RaisePropertyChanged(nameof(NewUserName));
                }
            }
        }
        private string _newEmailAddr;
        public string NewEmailAddr
        {
            get { return _newEmailAddr; }
            set
            {
                if (_newEmailAddr != value)
                {
                    _newEmailAddr = value;
                    RaisePropertyChanged(nameof(NewEmailAddr));
                }
            }
        }
        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    RaisePropertyChanged(nameof(NewPassword));
                }
            }
        }

        private ICommand _signupCommand;
        public ICommand SignUpCommand =>
            _signupCommand ?? (_signupCommand = new Command(SignUpCommandExecuted, SignUpCommandCanExecute));

        private bool SignUpCommandCanExecute()
        {
            if (!string.IsNullOrEmpty(NewUserName) && !string.IsNullOrEmpty(NewEmailAddr) && !string.IsNullOrEmpty(NewPassword))
            {
                return true;
            }
            else { return false; }
        }

        private async void SignUpCommandExecuted()
        {
            IsBusy = true;

            PlayerObj.UserName = NewUserName;
            PlayerObj.EmailAddr = NewEmailAddr;
            PlayerObj.Password = NewPassword;
            PlayerObj.PlayerLevel = 1;

            _authenticationService.UpdateAccountAsync();

            await Navigation.PopToRootAsync();

            IsBusy = false;
        }


        private ICommand _loginFacebookCommand;
        public ICommand LoginFacebookCommand =>
            _loginFacebookCommand ?? (_loginFacebookCommand = new Command(OnLoginFacebook, CanLoginFacebook));

        private bool CanLoginFacebook()
        {
            return true;
        }

        private void OnLoginFacebook()
        {
            store = AccountStore.Create();
            account = store.FindAccountsForService(FacebookConstants.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                FacebookConstants.iAppId,
                null,
                FacebookConstants.Scope,
                new Uri(FacebookConstants.AuthorizeUrl),
                new Uri(FacebookConstants.RedirectUrl),
                new Uri(FacebookConstants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthFacebookCompleted;
            authenticator.Error += OnAuthFacebookError;

            AuthenticationState.Authenticator = authenticator;


            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        private ICommand _loginGoogleCommand;
        public ICommand LoginGoogleCommand =>
            _loginGoogleCommand ?? (_loginGoogleCommand = new Command(OnLoginGoogle, CanLoginGoogle));

        private bool CanLoginGoogle()
        {
            return true;
        }



        Account account;
        AccountStore store;


        private void OnLoginGoogle()
        {
            store = AccountStore.Create();
            account = store.FindAccountsForService(GoogleConstants.AppName).FirstOrDefault();

            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = GoogleConstants.iOSClientId;
                    redirectUri = GoogleConstants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = GoogleConstants.AndroidClientId;
                    redirectUri = GoogleConstants.AndroidRedirectUrl;
                    break;
            }

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                GoogleConstants.Scope,
                new Uri(GoogleConstants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(GoogleConstants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthGoogleCompleted;
            authenticator.Error += OnAuthGoogleError;

            AuthenticationState.Authenticator = authenticator;


            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        async void OnAuthGoogleCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthGoogleCompleted;
                authenticator.Error -= OnAuthGoogleError;
            }

            User user = null;
            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Google
                var request = new OAuth2Request("GET", new Uri(GoogleConstants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);
                }

                if (account != null)
                {
                    store.Delete(account, GoogleConstants.AppName);
                }

                await store.SaveAsync(account = e.Account, GoogleConstants.AppName);
                await App.Current.MainPage.DisplayAlert("G+ Email address", user.Email, "OK");
            }
        }

        void OnAuthGoogleError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthGoogleCompleted;
                authenticator.Error -= OnAuthGoogleError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }

        async void OnAuthFacebookCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthFacebookCompleted;
                authenticator.Error -= OnAuthFacebookError;
            }

            User user = null;
            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Facebook
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=id,name,email"), null, e.Account);
                var response = await request.GetResponseAsync();
                var fbUser = JObject.Parse(response.GetResponseText());

                var id = fbUser["id"].ToString().Replace("\"", "");
                var name = fbUser["name"].ToString().Replace("\"", "");
                var email = fbUser["email"].ToString().Replace("\"", "");

                if (account != null)
                {
                    store.Delete(account, FacebookConstants.AppName);
                }

                await store.SaveAsync(account = e.Account, FacebookConstants.AppName);
                await App.Current.MainPage.DisplayAlert("FB Email address", user.Email, "OK");
            }
        }

        void OnAuthFacebookError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthFacebookCompleted;
                authenticator.Error -= OnAuthFacebookError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }



        public SignUpPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            _authenticationService = new AuthenticationService(Navigation);

        }


    }
}
