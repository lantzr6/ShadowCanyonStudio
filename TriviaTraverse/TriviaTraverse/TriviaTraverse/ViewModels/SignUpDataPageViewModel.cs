using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Services;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static Android.Provider.SyncStateContract;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class SignUpDataPageViewModel : ViewModelBase
    {
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
                    ((Command)SignUpCommand).ChangeCanExecute();
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
                    _newEmailAddr = value.Trim();
                    RaisePropertyChanged(nameof(NewEmailAddr));
                    ((Command)SignUpCommand).ChangeCanExecute();
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
                    ((Command)SignUpCommand).ChangeCanExecute();
                }
            }
        }

        private ICommand _signupCommand;
        public ICommand SignUpCommand =>
            _signupCommand ?? (_signupCommand = new Command(SignUpCommandExecuted, SignUpCommandCanExecute));

        private bool SignUpCommandCanExecute()
        {
            if (!string.IsNullOrEmpty(NewUserName) && EMailIsValid(NewEmailAddr) && (!string.IsNullOrEmpty(NewPassword) && NewPassword.Length >= 6))
            {
                return true;
            }
            else { return false; }
        }

        private void SignUpCommandExecuted()
        {
            UpdateAccount();
        }

        public bool EMailIsValid(string emailaddress)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

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

        private async void UpdateAccount()
        {
            IsBusy = true;

            PlayerObj.UserName = NewUserName;
            PlayerObj.EmailAddr = NewEmailAddr;
            PlayerObj.Password = NewPassword;
            PlayerObj.PlayerLevel = 1;
            PlayerObj.FbLogin = false;

            await App._authenticationService.UpdateAccountAsync();
   
            IsBusy = false;

            ((MasterPage)(App.Current.MainPage)).ShowHome();
        }


 



        public SignUpDataPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

        }



    }
}
