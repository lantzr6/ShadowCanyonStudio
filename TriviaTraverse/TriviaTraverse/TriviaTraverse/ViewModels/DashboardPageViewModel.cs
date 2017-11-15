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

        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private Dashboard _dashboardObj = new Dashboard();
        public Dashboard DashboardObj
        {
            get { return _dashboardObj; } 
            set
            {
                _dashboardObj = value;
                RaisePropertyChanged(nameof(DashboardObj));
            }
        }

        public GameSection ActiveSection
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



        private ICommand _joinGameCommand;
        public ICommand JoinGameCommand =>
            _joinGameCommand ?? (_joinGameCommand = new Command(JoinGame, CanJoinGame));

        private bool CanJoinGame()
        {
            return true;
        }

        private async void JoinGame()
        {
            VGame vGame = await WebApi.Instance.JoinNewGame(PlayerObj.PlayerId);
            LoadDashboardGames();
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new Command(OnRefresh, CanRefresh));

        private bool CanRefresh()
        {
            return true;
        }

        private void OnRefresh()
        {
            LoadDashboardGames();
        }


        #endregion



        public DashboardPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;


            //Navigation.PushModalAsync(new SignUpPage());

            if (PlayerObj.PlayerLevel < 1)  //not logged in and tutorial not complete
            {
                Navigation.PushModalAsync(new StartPage());
            }
            else
            {
                LoadDashboardGames();
            }
            //else if (!PlayerObj.IsLoginActive)
            //{
            //    Navigation.PushModalAsync(new LoginPage());
            //}            
        }

        public void PlayCampaign(object value)
        {
            Campaign obj = (Campaign)value;
            if (obj != null)
            {
                App.GameMode = GameMode.Campaign;
                Navigation.PushAsync(new CampaignBoardPage());
            }
        }

        public void PlayVGame(object value)
        {
            VGame obj = (VGame)value;
            if (obj != null)
            {
                Navigation.PushAsync(new GameBoardPage(obj.VGameId));
            }
        }

        public void LoadDashboardGames()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;

                DashboardObj = await WebApi.Instance.GetDashboard(PlayerObj.PlayerId);

                IsBusy = false;
            });
        }

        public void ClearDashboard()
        {
            DashboardObj = new Dashboard();
        }

    }
}
