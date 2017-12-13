using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Facebook.Models;
using TriviaTraverse.Facebook.Objects;
using TriviaTraverse.Facebook.Services;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Services;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class SignUpPageViewModel : ViewModelBase
    {

        #region "Properties"

        public SignUpPageMode PageMode { get; set; }

        public bool IsLoginAvailable
        {
            get { return PageMode == SignUpPageMode.SignUpOnly ? false : true; }
        }

        private string[] permissions = new string[] {
            "public_profile",
            "email",
            "user_friends"
        };
        private Dictionary<string, string> parameters = new Dictionary<string, string>();


        public Player PlayerObj
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


        #endregion

        #region "Commands"

        private ICommand _signinCommand;
        public ICommand SignInCommand =>
            _signinCommand ?? (_signinCommand = new Command(SignInCommandExecuted, SignInCommandCanExecute));

        private bool SignInCommandCanExecute()
        {
           return true;
        }

        private void SignInCommandExecuted()
        {
            Navigation.PushModalAsync(new SignInDataPage());
        }


        private ICommand _signupCommand;
        public ICommand SignUpCommand =>
            _signupCommand ?? (_signupCommand = new Command(SignUpCommandExecuted, SignUpCommandCanExecute));

        private bool SignUpCommandCanExecute()
        {
            return true;
        }

        private void SignUpCommandExecuted()
        {
            Navigation.PushModalAsync(new SignUpDataPage());
        }

        private ICommand _loginFacebookCommand;
        public ICommand LoginFacebookCommand =>
            _loginFacebookCommand ?? (_loginFacebookCommand = new Command(OnLoginFacebook, CanLoginFacebook));

        private bool CanLoginFacebook()
        {
            return true;
        }

        private async void OnLoginFacebook()
        {
            FbAccessToken token;
            try
            {
                token = await DependencyService.Get<IFacebookLogin>()
                    .LogIn(permissions);

                IsBusy = true;

                if (token != null)
                {
                    // save token
                    await token.Save();

                    FBGraph(token);

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("token was null");
                }

            }
            catch (TaskCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine("The task was canceled. " + e.Message);
                //						throw e;
            }


            IsBusy = false;

        }


        #endregion

        public SignUpPageViewModel(INavigation _navigation, SignUpPageMode _pageMode)
        {
            Navigation = _navigation;
            PageMode = _pageMode;

            // try to check if token is available
            var token = FbAccessToken.Current;
            if (token != null && token.Token != null)
            {
                System.Diagnostics.Debug.WriteLine("Token available");
                FBGraph(token);
            }

        }

        #region "Functions"

        private async void UpdateAccount()
        {
            IsBusy = true;

            PlayerObj.UserName = NewUserName;
            PlayerObj.EmailAddr = NewEmailAddr;
            PlayerObj.Password = NewPassword;
            PlayerObj.PlayerLevel = 1;

            await App._authenticationService.UpdateAccountAsync();

            IsBusy = false;

            ((MasterPage)(App.Current.MainPage)).ShowHome();
        }


        private async void FBGraph(FbAccessToken token)
        {
            parameters.Add("fields", "first_name, last_name, name, email");
            IGraphRequest request = DependencyService.Get<IGraphRequest>()
                .NewRequest(token, "/me", parameters);

            IGraphResponse response = await request.ExecuteAsync();
            System.Diagnostics.Debug.WriteLine(response.RawResponse);

            if (response != null && response.RawResponse != null)
            {
                Dictionary<string, string> deserialized = JsonConvert
                .DeserializeObject<Dictionary<string, string>>(response.RawResponse);
                FacebookProfile profile = new FacebookProfile(deserialized["first_name"], deserialized["last_name"], deserialized["name"], deserialized["email"], deserialized["id"]);

                NewUserName = profile.FirstName + (profile.LastName.Length > 0 ? profile.LastName.First().ToString() : "");
                NewEmailAddr = profile.Email;
                NewPassword = "facebook";

                PlayerObj.FbLogin = true;

                Player data = new Player();

                data = await WebApi.Instance.GetAccountAsync(NewEmailAddr, NewPassword);

                if (data != null)
                {
                    PlayerObj.PlayerId = data.PlayerId;
                    PlayerObj.UserName = data.UserName;
                    PlayerObj.EmailAddr = data.EmailAddr;
                    PlayerObj.Password = data.Password;
                    PlayerObj.PlayerLevel = data.PlayerLevel;
                    PlayerObj.CurrentSteps = data.CurrentSteps;
                    PlayerObj.StepBank = data.StepBank;
                    PlayerObj.Coins = data.Coins;
                    PlayerObj.Stars = data.Stars;
                    PlayerObj.Points = data.Points;

                    ((MasterPage)(App.Current.MainPage)).ShowHome();

                    (((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).RootPage as DashboardPage).LoadDashboard();
                }
                else
                {
                    //App.CampaignObj = null;
                    //App.TutorialObj = null;

                    //Player player = await WebApi.Instance.GetNewTempPlayerAsync();
                    //PlayerObj.PlayerId = player.PlayerId;
                    //PlayerObj.UserName = player.UserName;
                    //PlayerObj.EmailAddr = player.EmailAddr;
                    //PlayerObj.Password = player.Password;
                    //PlayerObj.PlayerLevel = player.PlayerLevel;

                    UpdateAccount();
                }

                
            }
            else
            {
                await token.Clear();
                parameters.Clear();
            }
        }
        #endregion
    }

    public enum SignUpPageMode
    {
        Login,
        SignUpOnly
    }
}
