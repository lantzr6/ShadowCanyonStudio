using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {

        #region "Properties"

        public PlayerLocal PlayerObj
        {
            get { return App.PlayerObj; }
        }

        public CampaignSection ActiveSection
        {
            get
            {
                return App.ActiveSection;
            }
            set
            {
                App.ActiveSection = value;
                RaisePropertyChanged(nameof(ActiveSection));
            }
        }

        #endregion


        #region "Commands"

        private ICommand _playCampaignCommand;
        public ICommand PlayCampaignCommand =>
            _playCampaignCommand ?? (_playCampaignCommand = new Command(PlayCampaign, CanPlayCampaign));

        private bool CanPlayCampaign()
        {
            return true;
        }

        private void PlayCampaign()
        {
            App.GameMode = "Campaign";
            Navigation.PushAsync(new CampaignPage());

        }

        private ICommand _joinGameCommand;
        public ICommand JoinGameCommand =>
            _joinGameCommand ?? (_joinGameCommand = new Command(JoinGame, CanJoinGame));

        private bool CanJoinGame()
        {
            return true;
        }

        private void JoinGame()
        {
            Application.Current.MainPage.DisplayAlert("TO DO", "Not implemented yet.", "OK");
        }

        private ICommand _createGameCommand;
        public ICommand CreateGameCommand =>
            _createGameCommand ?? (_createGameCommand = new Command(CreateGame, CanCreateGame));

        private bool CanCreateGame()
        {
            return true;
        }

        private void CreateGame()
        {
            Application.Current.MainPage.DisplayAlert("TO DO", "Not implemented yet.", "OK");
        }


        #endregion



        public DashboardPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            if (PlayerObj.PlayerLevel < 1)  //not logged in and tutorial not complete
            {
                Navigation.PushModalAsync(new StartPage());
            }
            //else if (!PlayerObj.IsLoginActive)
            //{
            //    Navigation.PushModalAsync(new LoginPage());
            //}            
        }

    }
}
