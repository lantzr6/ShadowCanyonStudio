using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaTraverse.Api;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        INavigation Navigation;

        public AuthenticationService(INavigation _navigation)
        {
            Navigation = _navigation;
        }

        public bool Login(string email, string password)
        {
            if (password.Equals("trivia", StringComparison.OrdinalIgnoreCase))
            {
                App.PlayerObj.EmailAddr = email;
                //Settings.IsLoginActive = true;
                return true;
            }

            return false;
        }

        public async void UpdateAccountAsync()
        {
            Player data = new Player();
            data.PlayerId = App.PlayerObj.PlayerId;
            data.UserName = App.PlayerObj.UserName;
            data.EmailAddr = App.PlayerObj.EmailAddr;
            data.Password = App.PlayerObj.Password;
            data.PlayerLevel = App.PlayerObj.PlayerLevel;
            data.TutorialInfoLevel = App.PlayerObj.TutorialInfoLevel;
            data.CurrentSteps = App.PlayerObj.CurrentSteps;
            data.StepBank = App.PlayerObj.StepBank;
            data.Coins = App.PlayerObj.Coins;
            data.Stars = App.PlayerObj.Stars;
            data.Points = App.PlayerObj.Points;

            await WebApi.Instance.UpdateAccountAsync(data);

        }

        public void Logout()
        {
            PlayerLocal data = App.PlayerObj;
            data.PlayerId = -1;
            data.UserName = string.Empty;
            data.Password = string.Empty;
            data.EmailAddr = string.Empty;
            data.Coins = 0;
            data.CurrentSteps = 0;
            data.PlayerLevel = -1;
            data.Points = 0;
            data.Stars = 0;
            data.StepBank = 0;
            data.TutorialInfoLevel = -1;

            App.CampaignObj = null;

            
            Navigation.PushModalAsync(new LoginPage());
        }
    }
}