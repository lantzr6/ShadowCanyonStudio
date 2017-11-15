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
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    class CampaignBoardPageViewModel : ViewModelBase
    {
        public Task Initialization { get; private set; }

        public CampaignBoardPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Initialization = InitializeAsync();

        }

        private async Task InitializeAsync()
        {
            // Asynchronously initialize this instance.
            if (CampaignObj == null)
            {
                IsBusy = true;
                CampaignObj = await WebApi.Instance.GetCampaign(PlayerObj.PlayerId);
                //GetSteps();

                IsBusy = false;
            }
            if (CampaignObj.CurrentStage.StageLevel == 0)
            {
                SelectCategoryVisible = true;
            }
            else
            {
                ActiveStage = CampaignObj.CurrentStage;
                SelectCategoryVisible = false;
            }
        }

        #region "Properties"

        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }
        public TutorialMessagesStatus TutorialObj
        {
            get { return App.TutorialObj; }
            set
            {
                App.TutorialObj = value;
            }
        }

        private bool _tutorialSectionNewlyCompleteIsVisible;
        public bool TutorialSectionNewlyCompleteIsVisible
        {
            get { return _tutorialSectionNewlyCompleteIsVisible; }
            set
            {
                if (_tutorialSectionNewlyCompleteIsVisible != value)
                {
                    _tutorialSectionNewlyCompleteIsVisible = value;
                    TutorialObj.CampaignSectionNewlyComplete = !value;
                    App.UpdateTutorialData();
                    RaisePropertyChanged(nameof(TutorialSectionNewlyCompleteIsVisible));
                }
            }
        }

        private bool _tutorialStageNewlyUnlockedIsVisible;
        public bool TutorialStageNewlyUnlockedIsVisible
        {
            get { return _tutorialStageNewlyUnlockedIsVisible; }
            set
            {
                if (_tutorialStageNewlyUnlockedIsVisible != value)
                {
                    _tutorialStageNewlyUnlockedIsVisible = value;
                    TutorialObj.CampaignStageNewlyUnlocked = !value;
                    App.UpdateTutorialData();
                    RaisePropertyChanged(nameof(TutorialStageNewlyUnlockedIsVisible));
                }
            }
        }

        private bool _tutorialStageBonusIsVisible;
        public bool TutorialStageBonusIsVisible
        {
            get { return _tutorialStageBonusIsVisible; }
            set
            {
                if (_tutorialStageBonusIsVisible != value)
                {
                    _tutorialStageBonusIsVisible = value;
                    TutorialObj.CampaignStageBonus = !value;
                    App.UpdateTutorialData();
                    RaisePropertyChanged(nameof(TutorialStageBonusIsVisible));
                }
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

        private int _selectedStageLevel;
        public int SelectedStageLevel
        {
            get { return _selectedStageLevel; }
            set
            {
                if (_selectedStageLevel != value)
                {
                    _selectedStageLevel = value;
                    RaisePropertyChanged(nameof(SelectedStageLevel));
                }
            }
        }

        private bool _selectCategoryVisible = false;
        public bool SelectCategoryVisible
        {
            get { return _selectCategoryVisible; }
            set
            {
                if (_selectCategoryVisible != value)
                {
                    _selectCategoryVisible = value;
                    RaisePropertyChanged(nameof(SelectCategoryVisible));
                    RaisePropertyChanged(nameof(SelectCategoryNotVisible));
                }
            }
        }
        public bool SelectCategoryNotVisible
        {
            get { return !SelectCategoryVisible; }
        }

        private string _campaignState;
        public string CampaignState
        {
            get { return _campaignState; }
            set
            {
                if (_campaignState != value)
                {
                    _campaignState = value;
                    RaisePropertyChanged(nameof(CampaignState));
                }
            }
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

        #region "Commands"

        private ICommand _selectCategoryCommand;
        public ICommand SelectCategoryCommand =>
            _selectCategoryCommand ?? (_selectCategoryCommand = new Command<object>(OnSelectCategory, CanSelectCategory));

        private bool CanSelectCategory(object value)
        {
            return true;
        }

        private async void OnSelectCategory(object value)
        {
            IsBusy = true;
            CampaignCategory obj = (CampaignCategory)value;

            int nextStageLevel = CampaignObj.Stages.Select(o => o.StageLevel).Max() + 1;
            NewCampaignStageReturn returnObj = await WebApi.Instance.PostNextCampaignCategory(new NewCampaignStageInfo() { PlayerCampaignId = CampaignObj.PlayerCampaignId, CampaignCategoryId = obj.CampaignCategoryId, StageLevel = nextStageLevel });
            ActiveStage = returnObj.NewStage;
            CampaignObj.Stages.Add(ActiveStage);
            CampaignObj.CategoryQueue = returnObj.CategoryQueue;

            App.CampaignObj = CampaignObj; //local save of Campaign changes
            RaisePropertyChanged(nameof(CampaignObj));  //needed to refresh category list

            SelectCategoryVisible = false;
            IsBusy = false;
        }

        private ICommand _closeTutorialSectionNewlyCompleteCommand;
        public ICommand CloseTutorialSectionNewlyCompleteCommand =>
            _closeTutorialSectionNewlyCompleteCommand ?? (_closeTutorialSectionNewlyCompleteCommand = new Command<object>(OnCloseTutorialSectionNewlyComplete, CanCloseTutorialSectionNewlyComplete));

        private bool CanCloseTutorialSectionNewlyComplete(object value)
        {
            return true;
        }

        private void OnCloseTutorialSectionNewlyComplete(object value)
        {
            TutorialSectionNewlyCompleteIsVisible = false;
        }


        private ICommand _closeTutorialStageNewlyUnlockedCommand;
        public ICommand CloseTutorialStageNewlyUnlockedCommand =>
            _closeTutorialStageNewlyUnlockedCommand ?? (_closeTutorialStageNewlyUnlockedCommand = new Command<object>(OnCloseTutorialStageNewlyUnlocked, CanCloseTutorialStageNewlyUnlocked));

        private bool CanCloseTutorialStageNewlyUnlocked(object value)
        {
            return true;
        }

        private void OnCloseTutorialStageNewlyUnlocked(object value)
        {
            TutorialStageNewlyUnlockedIsVisible = false;
        }

        private ICommand _closeTutorialStageBonusCommand;
        public ICommand CloseTutorialStageBonusCommand =>
            _closeTutorialStageBonusCommand ?? (_closeTutorialStageBonusCommand = new Command<object>(OnCloseTutorialStageBonus, CanCloseTutorialStageBonus));

        private bool CanCloseTutorialStageBonus(object value)
        {
            return true;
        }

        private void OnCloseTutorialStageBonus(object value)
        {
            TutorialStageBonusIsVisible = false;
        }


        #endregion

        public async void SelectSection(string parm)
        {
            IsBusy = true;
            int obj = int.Parse(parm);

            App.ActiveSection = ActiveStage.Sections[obj];
            if (!App.ActiveSection.IsComplete)
            {
                int sectionId = ActiveStage.Sections[obj].CampaignSectionId;
                int campaignId = CampaignObj.PlayerCampaignId;

                GameSection returnSection = await (GetSection(sectionId,PlayerObj.PlayerId,campaignId,false));
                App.ActiveSection.Questions = returnSection.Questions;
                App.ActiveSection.NumberOfQuestions = returnSection.Questions.Count();
                await Navigation.PushModalAsync(new QuestionStatusPage());
            }
            IsBusy = false;
        }

        public async void RetrySection(string parm)
        {
            IsBusy = true;
            int obj = int.Parse(parm);

            App.ActiveSection = ActiveStage.Sections[obj];

            PlayerObj.Coins -= App.ActiveSection.EarnedCoins;
            PlayerObj.Stars -= App.ActiveSection.EarnedStars;
            PlayerObj.Points -= App.ActiveSection.EarnedPoints;

            int sectionId = ActiveStage.Sections[obj].CampaignSectionId;
            int campaignId = CampaignObj.PlayerCampaignId;

            GameSection returnSection = await (GetSection(sectionId, PlayerObj.PlayerId, campaignId, true));
            App.ActiveSection.Questions = returnSection.Questions;
            App.ActiveSection.NumberOfQuestions = returnSection.Questions.Count();
            App.ActiveSection.NumberAnswered = 0;
            App.ActiveSection.NumberCorrect = 0;
            App.ActiveSection.EarnedCoins = 0;
            App.ActiveSection.EarnedPoints = 0;
            App.ActiveSection.EarnedStars = 0;
            await Navigation.PushModalAsync(new QuestionStatusPage());

            IsBusy = false;
        }

        public async Task<GameSection> GetSection(int sectionId, int playerId, int playerCampaignId, bool retry = false)
        {
            GameSection section;
            section = await WebApi.Instance.GetSectionAsync(sectionId, playerId, playerCampaignId, retry);
            return section;
        }

        public void SelectPreviousStage()
        {
            int stageLevel = ActiveStage.StageLevel;

            ActiveStage = CampaignObj.Stages.Where(o => o.StageLevel == (stageLevel - 1)).FirstOrDefault();
        }

        public async void SelectNextStage()
        {
            int stageLevel = ActiveStage.StageLevel;
            int starsNeeded = ActiveStage.StarsRequired;

            if (PlayerObj.Stars >= starsNeeded)
            {
                if (CampaignObj.Stages.Where(o => o.StageLevel == (stageLevel + 1)).Any())
                {
                    ActiveStage = CampaignObj.Stages.Where(o => o.StageLevel == (stageLevel + 1)).FirstOrDefault();
                }
                else
                {
                    SelectCategoryVisible = true;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Not enough stars", "You need " + (starsNeeded - PlayerObj.Stars).ToString() + " more stars", "OK");
            }


        }


    }
}
