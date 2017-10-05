using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TriviaTraverse.Services;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {

        public PlayerLocal PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private ICommand _loginCommand;
        public ICommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new Command(OnLoginCommandExecuted, CanLoginCommandExecute));

        private bool CanLoginCommandExecute()
        {
            if (!string.IsNullOrEmpty(PlayerObj.UserName) && !string.IsNullOrEmpty(PlayerObj.Password) && !IsBusy)
            {
                return true;
            }
            else { return false; }
        }

        private async void OnLoginCommandExecuted()
        {
            IsBusy = true;
            //if (_authenticationService.Login(PlayerObj.UserName, PlayerObj.Password))
            //{
            //    await _navigationService.NavigateAsync("/Master/Navigation/DashboardPage?title=Login%20Success");
            //}
            //else
            //{
            //    await _pageDialogService.DisplayAlertAsync("Seriously???", "trye trivia", "Try Again");
            //}


            App.authenticated = await App.Authenticator.Authenticate();

            IsBusy = false;
        }

        public LoginPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Title = "Login";
        }

    }
}
