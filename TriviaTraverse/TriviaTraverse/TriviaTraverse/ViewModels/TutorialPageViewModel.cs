using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;

namespace TriviaTraverse.ViewModels
{
    class TutorialPageViewModel : ViewModelBase
    {
        public Task Initialization { get; private set; }

        public TutorialPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            Initialization = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // Asynchronously initialize this instance.
            IsBusy = true;
            if (PlayerObj.PlayerId > 0)
            {
                await LoadCampaign();
                if (ActiveStage.Sections[0].IsComplete)
                {
                    await Navigation.PushModalAsync(new LoginPage());
                }
            }
            IsBusy = false;
        }

        #region Properties

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

        private CampaignStageCategory _activeStage;
        public CampaignStageCategory ActiveStage
        {
            get { return _activeStage; }
            set
            {
                if (_activeStage != value)
                {
                    _activeStage = value;
                    RaisePropertyChanged(nameof(ActiveStage));
                }
            }
        }


        #endregion

        #region Commands

        private ICommand _skipCommand;
        public ICommand SkipCommand =>
            _skipCommand ?? (_skipCommand = new Command(SkipCommandExecuted, SkipCommandCanExecute));

        private bool SkipCommandCanExecute()
        {
            return true;
        }

        private void SkipCommandExecuted()
        {
            Navigation.PushModalAsync(new LoginPage());
        }

        #endregion

        public void StartTutorial()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                BusyMessage = "loading Tutorial...";

                if (PlayerObj.PlayerId <= 0)
                {
                    CampaignObj = null;
                    App.TutorialObj = null;

                    Player player = await WebApi.Instance.NewAccountAsync(null);
                    PlayerObj.PlayerId = player.PlayerId;
                    PlayerObj.UserName = player.UserName;
                    PlayerObj.EmailAddr = player.EmailAddr;
                    PlayerObj.Password = player.Password;
                    PlayerObj.PlayerLevel = player.PlayerLevel;
                }

                await LoadCampaign();
                await Navigation.PushModalAsync(new QuestionStatusPage());
                IsBusy = false;
            });

        }

        private async Task LoadCampaign()
        {
            if (CampaignObj == null)
            {
                CampaignObj = await WebApi.Instance.GetCampaign(PlayerObj.PlayerId);
            }

            ActiveStage = CampaignObj.Stages.Where(o => o.StageLevel == 0).FirstOrDefault();

            App.ActiveSection = ActiveStage.Sections[0];
            int sectionId = ActiveStage.Sections[0].CampaignSectionId;
            int campaignId = CampaignObj.PlayerCampaignId;

            GameSection returnSection = await (GetSection(sectionId, PlayerObj.PlayerId, campaignId));
            App.ActiveSection.Questions = returnSection.Questions;

            App.GameMode = GameMode.Tutorial;
        }

        public async void SelectSection(string parm)
        {
            int obj = int.Parse(parm);

            App.ActiveSection = ActiveStage.Sections[obj];
            if (!App.ActiveSection.IsComplete)
            {
                int sectionId = ActiveStage.Sections[obj].CampaignSectionId;
                int campaignId = CampaignObj.PlayerCampaignId;

                GameSection returnSection = await (GetSection(sectionId, PlayerObj.PlayerId, campaignId, false));
                App.ActiveSection.Questions = returnSection.Questions;
                App.ActiveSection.NumberOfQuestions = returnSection.Questions.Count();
                await Navigation.PushModalAsync(new QuestionStatusPage());
            }
        }


        public async Task<GameSection> GetSection(int sectionId, int playerId, int playerCampaignId, bool retry = false)
        {
            GameSection section;
            section = await WebApi.Instance.GetSectionAsync(sectionId, playerId, playerCampaignId, retry);
            return section;
        }


    }
}
