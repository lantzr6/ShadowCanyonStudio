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

        public PlayerLocal PlayerObj
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


        public StartPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }

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
                if (Settings.UserPlayer.PlayerId == -1)
                {
                    Player player = await (GetNewTempPlayer());
                    PlayerObj.PlayerId = player.PlayerId;
                    PlayerObj.UserName = player.UserName;
                    PlayerObj.EmailAddr = player.EmailAddr;
                    PlayerObj.Password = player.Password;
                    PlayerObj.PlayerLevel = player.PlayerLevel;
                    PlayerObj.TutorialInfoLevel = 1;
                }

                if (CampaignObj == null)
                {
                    CampaignObj = await WebApi.Instance.GetCampaign(PlayerObj.PlayerId);
                }

                CampaignStageCategory ActiveStage = CampaignObj.Stages.Where(o=>o.StageLevel==0).FirstOrDefault();

                App.ActiveSection = ActiveStage.Sections[0];
                int sectionId = ActiveStage.Sections[0].CampaignSectionId;

                CampaignSection returnSection = await (GetSection(sectionId));
                App.ActiveSection.Questions = returnSection.Questions;

                App.GameMode = "Tutorial";
                await Navigation.PushModalAsync(new QuestionStatusPage());
                IsBusy = false;
            });

        }

        public async Task<Player> GetNewTempPlayer()
        {
            Player player;
            player = await WebApi.Instance.GetNewTempPlayerAsync();
            return player;
        }
        public async Task<CampaignSection> GetTutorial()
        {
            CampaignSection section;
            section = await WebApi.Instance.GetTutorialAsync();
            return section;
        }

        private ICommand _loginCommand;
        public ICommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new Command(Login, CanLogin));

        private bool CanLogin()
        {
            return true;
        }

        private async void Login()
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        public async Task<CampaignSection> GetSection(int sectionId)
        {
            CampaignSection section;
            section = await WebApi.Instance.GetSectionAsync(sectionId);
            return section;
        }
    }
}
