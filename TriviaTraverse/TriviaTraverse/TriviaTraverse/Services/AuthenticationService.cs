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
using TriviaTraverse.Facebook.Objects;

namespace TriviaTraverse.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService()
        {
        }

        public async Task<Player> LoginAsync(string email, string password)
        {
            Player data = new Player();
            data.UserName = "";
            data.EmailAddr = email;
            data.Password = password;

            //Player isValid = await WebApi.Instance.GetAccountAsync(data);
            data = await WebApi.Instance.GetAccountAsync(email, password);

            return data;
        }

        public async Task UpdateAccountAsync()
        {
            Player data = new Player();
            data.PlayerId = App.PlayerObj.PlayerId;
            data.UserName = App.PlayerObj.UserName;
            data.EmailAddr = App.PlayerObj.EmailAddr;
            data.FbLogin = App.PlayerObj.FbLogin;
            data.Password = App.PlayerObj.Password;
            data.PlayerLevel = App.PlayerObj.PlayerLevel;
            data.CurrentSteps = App.PlayerObj.CurrentSteps;
            data.StepBank = App.PlayerObj.StepBank;
            data.LastStepUpdate = App.PlayerObj.LastStepUpdate;
            data.Coins = App.PlayerObj.Coins;
            data.Stars = App.PlayerObj.Stars;
            data.Points = App.PlayerObj.Points;

            await WebApi.Instance.UpdateAccountAsync(data);

        }

        public async Task NewAccountAsync()
        {
            Player data = new Player();
            data.PlayerId = App.PlayerObj.PlayerId;
            data.UserName = App.PlayerObj.UserName;
            data.EmailAddr = App.PlayerObj.EmailAddr;
            data.FbLogin = App.PlayerObj.FbLogin;
            data.Password = App.PlayerObj.Password;
            data.PlayerLevel = App.PlayerObj.PlayerLevel;
            data.CurrentSteps = App.PlayerObj.CurrentSteps;
            data.StepBank = App.PlayerObj.StepBank;
            data.LastStepUpdate = App.PlayerObj.LastStepUpdate;
            data.Coins = App.PlayerObj.Coins;
            data.Stars = App.PlayerObj.Stars;
            data.Points = App.PlayerObj.Points;

            await WebApi.Instance.NewAccountAsync(data);

        }

        public async void Logout()
        {
            App.PlayerObj.PlayerId = 0;
            App.PlayerObj.UserName = string.Empty;
            App.PlayerObj.Password = string.Empty;
            App.PlayerObj.EmailAddr = string.Empty;
            App.PlayerObj.Coins = 0;
            App.PlayerObj.CurrentSteps = 0;
            App.PlayerObj.PlayerLevel = -1;
            App.PlayerObj.Points = 0;
            App.PlayerObj.Stars = 0;
            App.PlayerObj.StepBank = 0;
            App.PlayerObj.LastStepUpdate = DateTime.MinValue;

            App.CampaignObj = null;
            App.TutorialObj = null;
            App.VGameObj = null;

            if (FbAccessToken.Current != null)
            {
                await FbAccessToken.Current.Clear();
            }

            Settings.AuthToken = "";
            Settings.AuthTokenExpire = DateTime.MinValue;

            App.ActiveSection = null;

        }
    }
}