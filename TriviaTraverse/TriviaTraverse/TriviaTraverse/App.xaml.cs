using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Services;
using TriviaTraverse.Views;
using Xamarin.Forms;
using System.Threading.Tasks;
using static TriviaTraverse.Helpers.Settings;
using System;
using System.Diagnostics;
using System.ComponentModel;
using TriviaTraverse.Api;

namespace TriviaTraverse
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }


    public partial class App : Application
    {
        public static AuthenticationService _authenticationService = new AuthenticationService();

        public static Player PlayerObj
        {
            get { return Settings.CurrentPlayer; }
            set { Settings.CurrentPlayer = value;  }
        }
        public static TutorialMessagesStatus TutorialObj
        {
            get { return Settings.UserTutorial; }
            set { Settings.UserTutorial = value; }
        }
        public static Campaign CampaignObj
        {
            get { return Settings.UserCampaign; }
            set { Settings.UserCampaign = value; }
        }

        private static VGame _vGame = null;
        public static VGame VGameObj
        {
            get { return _vGame; }
            set { _vGame = value; }
        }

        public static GameSection ActiveSection;
        public static GameMode GameMode;

        // Track whether the user has authenticated.
        public static bool authenticated = false;

        public App()
        {

            InitializeComponent();

            PlayerObj.PropertyChanged += PlayerObjPropertyChanged;
            
            // The root page of your application
            MasterDetailPage MasterPage = new MasterPage();
            MasterPage.Detail = new NavigationPage(new DashboardPage());
            MainPage = MasterPage;
        }

        private void PlayerObjPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PlayerObj = PlayerObj;
        }

        public static async void UpdatePlayerData()
        {
            await _authenticationService.UpdateAccountAsync();
        }

        private void TutorialObjPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TutorialObj = TutorialObj;
        }

        public static async void UpdateTutorialData()
        {
            await WebApi.Instance.UpdateTutorialMessageStatusAsync(TutorialObj);
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

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
    }


}
