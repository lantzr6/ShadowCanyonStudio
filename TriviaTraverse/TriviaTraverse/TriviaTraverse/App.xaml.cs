using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Services;
using TriviaTraverse.Views;
using Xamarin.Forms;
using System.Threading.Tasks;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }


    public partial class App : Application
    {

        public static PlayerLocal PlayerObj;
        public static Campaign CampaignObj;
        public static CampaignSection ActiveSection;
        public static string GameMode;

        // Track whether the user has authenticated.
        public static bool authenticated = false;

        public App()
        {

            InitializeComponent();

            PlayerObj = Settings.UserPlayer;
            CampaignObj = Settings.UserCampaign;

            // The root page of your application
            MainPage = new DashboardPage();
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
