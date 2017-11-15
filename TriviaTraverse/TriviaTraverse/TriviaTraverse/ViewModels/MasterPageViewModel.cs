using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Services;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class MasterPageViewModel : ViewModelBase
    {
        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }
        public VGame VGameObj
        {
            get { return App.VGameObj; }
        }

        private ICommand _resetCommand;
        public ICommand ResetCommand =>
            _resetCommand ?? (_resetCommand = new Command(OnReset, CanReset));

        private bool CanReset(object arg)
        {
            return true;
        }

        private void OnReset(object obj)
        {
            App._authenticationService.Logout();

            ShowHome();
            
            (((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).RootPage as DashboardPage).Clear();

            Navigation.PushModalAsync(new StartPage());

            ((MasterPage)(App.Current.MainPage)).IsPresented = false;

        }

        private ICommand _addStepsCommand;
        public ICommand AddStepsCommand =>
            _addStepsCommand ?? (_addStepsCommand = new Command(OnAddSteps, CanAddSteps));
        private bool CanAddSteps()
        {
            return true;
        }
        private async void OnAddSteps()
        {
            PlayerObj.StepBank += 1000;
            await App._authenticationService.UpdateAccountAsync();
        }

        private ICommand _addVGameStepsCommand;
        public ICommand AddVGameStepsCommand =>
            _addVGameStepsCommand ?? (_addVGameStepsCommand = new Command(OnAddVGameSteps, CanAddVGameSteps));
        private bool CanAddVGameSteps()
        {
            return true;
        }
        private async void OnAddVGameSteps()
        {
            VGameObj.PlayerGameSteps += 1000;
            VGamePlayerUpdate inObj = new VGamePlayerUpdate() { VGameId = VGameObj.VGameId, PlayerId = PlayerObj.PlayerId, GameSteps = VGameObj.PlayerGameSteps };
            App.VGameObj = await WebApi.Instance.UpdateVGameAsync(inObj);
        }

        private ICommand _addCoinsCommand;
        public ICommand AddCoinsCommand =>
            _addCoinsCommand ?? (_addCoinsCommand = new Command(OnAddCoins, CanAddCoins));
        private bool CanAddCoins()
        {
            return true;
        }
        private async void OnAddCoins()
        {
            PlayerObj.Coins += 100;
            await App._authenticationService.UpdateAccountAsync();
        }

        public MasterPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Title = "Trivia Traverse";

        }

        public void ShowHome()
        {
            foreach (Page m in Navigation.ModalStack)
            {
                Navigation.PopModalAsync();
            }
            foreach (Page m in Navigation.NavigationStack)
            {
                Navigation.PopAsync();
            }

            (((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).RootPage as DashboardPage).LoadDashboard();
        }
    }
}