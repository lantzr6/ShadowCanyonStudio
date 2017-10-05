using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TriviaTraverse.Helpers;
using TriviaTraverse.Services;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public PlayerLocal PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private ICommand _resetCommand;
        public ICommand ResetCommand =>
            _resetCommand ?? (_resetCommand = new Command(OnReset, CanReset));

        private bool CanReset(object arg)
        {
            return true;
        }

        private async void OnReset(object obj)
        {
            AuthenticationService _authenticationService = new AuthenticationService(Navigation);
            _authenticationService.Logout();

            Settings.AuthToken = "";
            Settings.AuthTokenExpire = DateTime.MinValue;

            App.ActiveSection = null;

            await Navigation.PopToRootAsync();
        }

        private ICommand _addStepsCommand;
        public ICommand AddStepsCommand =>
            _addStepsCommand ?? (_addStepsCommand = new Command(OnAddSteps, OnCanAddSteps));

        private bool OnCanAddSteps()
        {
            return true;
        }

        private void OnAddSteps()
        {
            PlayerObj.StepBank += 1000;
        }

        public MainPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Title = "Home Page";

        }



    }
}