using System;
using System.Collections.Generic;
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
    public class StartPageViewModel : ViewModelBase
    {

        #region "Properties"


        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }

        public Campaign CampaignObj
        {
            get { return App.CampaignObj; }
            set
            {
                App.CampaignObj = value;
                RaisePropertyChanged(nameof(CampaignObj));
            }
        }


        #endregion

        #region "Commands"

        private ICommand _startTutorialCommand;
        public ICommand StartTutorialCommand =>
            _startTutorialCommand ?? (_startTutorialCommand = new Command(StartTutorial, CanStartTutorial));

        private bool CanStartTutorial()
        {
            return true;
        }

        private void StartTutorial()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;

                if (PlayerObj.PlayerId <= 0)
                {
                    CampaignObj = null;
                    App.TutorialObj = null;

                    Player player = await WebApi.Instance.GetNewTempPlayerAsync();
                    PlayerObj.PlayerId = player.PlayerId;
                    PlayerObj.UserName = player.UserName;
                    PlayerObj.EmailAddr = player.EmailAddr;
                    PlayerObj.Password = player.Password;
                    PlayerObj.PlayerLevel = player.PlayerLevel;
                }

                if (CampaignObj == null)
                {
                    CampaignObj = await WebApi.Instance.GetCampaign(PlayerObj.PlayerId);
                }

                CampaignStageCategory ActiveStage = CampaignObj.Stages.Where(o=>o.StageLevel==0).FirstOrDefault();

                App.ActiveSection = ActiveStage.Sections[0];
                int sectionId = ActiveStage.Sections[0].CampaignSectionId;
                int campaignId = CampaignObj.PlayerCampaignId;

                GameSection returnSection = await (GetSection(sectionId, PlayerObj.PlayerId, campaignId));
                App.ActiveSection.Questions = returnSection.Questions;

                App.GameMode = GameMode.Tutorial;
                await Navigation.PushModalAsync(new QuestionStatusPage());
                IsBusy = false;
            });

        }
        #endregion

        public StartPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }


        private ICommand _loginCommand;
        public ICommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new Command(Login, CanLogin));

        private bool CanLogin()
        {
            return true;
        }

        private void Login()
        {
            Navigation.PushModalAsync(new SignUpPage(SignUpPageMode.Login));
        }

        public async Task<GameSection> GetSection(int sectionId, int playerId,  int playerCampaignId)
        {
            GameSection section;
            section = await WebApi.Instance.GetSectionAsync(sectionId, playerId, playerCampaignId, false);
            return section;
        }
    }
}
