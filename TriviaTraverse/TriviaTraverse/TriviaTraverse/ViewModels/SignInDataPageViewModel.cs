using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class SignInDataPageViewModel : ViewModelBase
    {
        
        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private string _EmailAddr;
        public string EmailAddr
        {
            get { return _EmailAddr; }
            set
            {
                if (_EmailAddr != value)
                {
                    _EmailAddr = value.Trim();
                    RaisePropertyChanged(nameof(EmailAddr));
                    ((Command)SignInCommand).ChangeCanExecute();
                }
            }
        }
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    RaisePropertyChanged(nameof(Password));
                    ((Command)SignInCommand).ChangeCanExecute();
                }
            }
        }

        private bool _loginFailed = false;
        public bool LoginFailed
        {
            get { return _loginFailed; }
            set
            {
                if (_loginFailed != value)
                {
                    _loginFailed = value;
                    RaisePropertyChanged(nameof(LoginFailed));
                }
            }
        }

        private ICommand _signinCommand;
        public ICommand SignInCommand =>
            _signinCommand ?? (_signinCommand = new Command(SignInCommandExecuted, SignInCommandCanExecute));

        private bool SignInCommandCanExecute()
        {
            if (EMailIsValid(EmailAddr) && (!string.IsNullOrEmpty(Password) && Password.Length >= 6))
            {
                return true;
            }
            else { return false; }
        }

        private void SignInCommandExecuted()
        {
           Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;

                LoginFailed = false;

                Player data = new Player();

                data = await WebApi.Instance.GetAccountAsync(EmailAddr, Password);

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
                }
                else
                {
                    LoginFailed = true;
                }

                IsBusy = false;
            });
        }

        public bool EMailIsValid(string emailaddress)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\.\-]+)((\.(\w){2,3})+)$");

                if (!string.IsNullOrEmpty(emailaddress) && regex.IsMatch(emailaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }



        public SignInDataPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

        }



    }
}
