using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Facebook.Models;
using TriviaTraverse.Facebook.Objects;
using TriviaTraverse.Facebook.Services;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {

        #region "Properties"


        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }

        public Campaign CampaignObj
        {
            get { return App.CampaignObj; }
            set
            {
                App.CampaignObj = value;
                RaisePropertyChanged(nameof(CampaignObj));
            }
        }

        private string[] permissions = new string[] {
            "public_profile",
            "email",
            "user_friends"
        };
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        #endregion

        #region "Commands"

        private ICommand _startTutorialCommand;
        public ICommand StartTutorialCommand =>
            _startTutorialCommand ?? (_startTutorialCommand = new Command(StartTutorial, CanStartTutorial));
        private bool CanStartTutorial()
        {
            return true;
        }
        private void StartTutorial()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;

                if (PlayerObj.PlayerId <= 0)
                {
                    CampaignObj = null;
                    App.TutorialObj = null;

                    //Player player = await WebApi.Instance.GetNewTempPlayerAsync();
                    //PlayerObj.PlayerId = player.PlayerId;
                    //PlayerObj.UserName = player.UserName;
                    //PlayerObj.EmailAddr = player.EmailAddr;
                    //PlayerObj.Password = player.Password;
                    //PlayerObj.PlayerLevel = player.PlayerLevel;
                }

                if (CampaignObj == null)
                {
                    CampaignObj = await WebApi.Instance.GetCampaign(PlayerObj.PlayerId);
                }

                CampaignStageCategory ActiveStage = CampaignObj.Stages.Where(o=>o.StageLevel==0).FirstOrDefault();

                App.ActiveSection = ActiveStage.Sections[0];
                int sectionId = ActiveStage.Sections[0].CampaignSectionId;
                int campaignId = CampaignObj.PlayerCampaignId;

                GameSection returnSection = await (GetSection(sectionId, PlayerObj.PlayerId, campaignId));
                App.ActiveSection.Questions = returnSection.Questions;

                App.GameMode = GameMode.Tutorial;
                await Navigation.PushModalAsync(new QuestionStatusPage());
                IsBusy = false;
            });

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

        public StartPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            // try to check if token is available
            var token = FbAccessToken.Current;
            if (token != null && token.Token != null)
            {
                System.Diagnostics.Debug.WriteLine("Token available");
                FBGraph(token);
            }
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

        private ICommand _loginCommand;
        public ICommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new Command(Login, CanLogin));
        private bool CanLogin()
        {
            return true;
        }
        private void Login()
        {
            Navigation.PushModalAsync(new SignUpPage(SignUpPageMode.Login));
        }

        private async void UpdateAccount(string username, string email, string password)
        {
            IsBusy = true;

            PlayerObj.UserName = username;
            PlayerObj.EmailAddr = email;
            PlayerObj.Password = password;
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

                string NewUserName = profile.FirstName + (profile.LastName.Length > 0 ? profile.LastName.First().ToString() : "");
                string NewEmailAddr = profile.Email;
                string NewPassword = "facebook";

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
                    App.CampaignObj = null;
                    App.TutorialObj = null;

                    //Player player = await WebApi.Instance.GetNewTempPlayerAsync();
                    //PlayerObj.PlayerId = player.PlayerId;
                    //PlayerObj.UserName = player.UserName;
                    //PlayerObj.EmailAddr = player.EmailAddr;
                    //PlayerObj.Password = player.Password;
                    //PlayerObj.PlayerLevel = player.PlayerLevel;

                    UpdateAccount(NewUserName, NewEmailAddr, NewPassword);
                }


            }
            else
            {
                await token.Clear();
                parameters.Clear();
            }
        }


        public async Task<GameSection> GetSection(int sectionId, int playerId,  int playerCampaignId)
        {
            GameSection section;
            section = await WebApi.Instance.GetSectionAsync(sectionId, playerId, playerCampaignId, false);
            return section;
        }
    }
}
