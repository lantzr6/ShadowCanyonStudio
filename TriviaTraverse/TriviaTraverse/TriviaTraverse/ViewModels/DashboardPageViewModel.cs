using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private ICommand _newGameCommand;
        public ICommand NewGameCommand =>
            _newGameCommand ?? (_newGameCommand = new Command(OnNewGame, CanNewGame));
        private bool CanNewGame()
        {
            return true;
        }
        private async void OnNewGame()
        {
            await Navigation.PushModalAsync(new GameJoinPage());
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
            GetSteps();
        }

        private ICommand _playCampaignCommand;
        public ICommand PlayCampaignCommand =>
            _playCampaignCommand ?? (_playCampaignCommand = new Command<object>(OnPlayCampaign, CanPlayCampaign));
        private bool CanPlayCampaign(object value)
        {
            return true;
        }
        private void OnPlayCampaign(object value)
        {
            PlayCampaign(value);
        }


        #endregion



        public DashboardPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;


            //Navigation.PushModalAsync(new SignUpPage());

            if (PlayerObj.PlayerLevel < 1)  //not logged in and tutorial not complete
            {
                Navigation.PushModalAsync(new TutorialPage());
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

        private void PlayCampaign(object value)
        {
            Campaign obj = DashboardObj.Campaigns.Where(o=>o.PlayerCampaignId==(int)value).FirstOrDefault();
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

                //GetSteps();

                IsBusy = false;
            });
        }

        public void ClearDashboard()
        {
            DashboardObj = new Dashboard();
        }

        public void GetSteps()
        {
            List<StepData> stepQueries = new List<StepData>();

            DateTime nowStepQuery = DateTime.Now;
            if (PlayerObj.LastStepUpdateLocal < DateTime.Today) { PlayerObj.LastStepUpdate = DateTime.Today; }
            stepQueries.Add(new StepData() { VGameId = -1, StartTime = PlayerObj.LastStepUpdateLocal });
            Debug.WriteLine("Fetching Steps Player " + PlayerObj.LastStepUpdateLocal.ToString());

            foreach (VGame vg in DashboardObj.VGames.Where(o=>o.IsActive))
            {
                if (vg.PlayerLastStepQueryLocal < vg.StartTimeLocal) { vg.PlayerLastStepQuery = vg.StartTime; }
                stepQueries.Add(new StepData() { VGameId = vg.VGameId, StartTime = vg.PlayerLastStepQueryLocal });
                Debug.WriteLine("Fetching Steps VGame " + vg.VGameId.ToString() + " " + PlayerObj.LastStepUpdateLocal.ToString());
            }
            FetchHealthData(stepQueries);
        }

        public async void UpdateSteps(int vgID, int steps, DateTime endDate)
        {
            Console.WriteLine("Fecth Returned: " + steps.ToString());
            if (steps > 0)
            {
                if (vgID == -1)
                {
                    PlayerObj.StepBank += steps;
                    PlayerObj.LastStepUpdate = endDate.ToUniversalTime();
                    App.UpdatePlayerData();
                    Console.WriteLine("Player StepBank Updated " + steps.ToString());
                }
                else
                {
                    VGame vg = DashboardObj.VGames.Where(o => o.VGameId == vgID).FirstOrDefault();
                    vg.PlayerGameSteps += steps;
                    vg.PlayerLastStepQuery = endDate.ToUniversalTime();
                    VGamePlayerUpdate inObj = new VGamePlayerUpdate() { VGameId = vg.VGameId, PlayerId = PlayerObj.PlayerId, GameSteps = vg.PlayerGameSteps, LastStepUpdate = vg.PlayerLastStepQuery };
                    vg = await WebApi.Instance.UpdateVGamePlayerAsync(inObj);
                    Console.WriteLine("VGame Player Steps Updated " + vg.VGameId.ToString() + steps.ToString());
                }
            }
        }

        void FetchHealthData(List<StepData> StepQueries)
        {
            DependencyService.Get<IHealthData>().GetHealthPermissionAsync((result) =>
            {
                var a = result;
                if (result)
                { 
                    DependencyService.Get<IHealthData>().FetchSteps((resultsList) =>
                    {
                        foreach (StepData item in resultsList)
                        {
                            UpdateSteps(item.VGameId, item.Steps, item.EndTime);
                        }
                    }, StepQueries);
 
                    // wait for them all to finish
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    //this.Label1.Text = "Total steps today: " + Math.Floor(steps).ToString() + " Meters Walked " + Math.Floor(meters).ToString() + " Active minutes " + Math.Floor(minutes).ToString();
                    //});
                }
            });

        }

    }
}
