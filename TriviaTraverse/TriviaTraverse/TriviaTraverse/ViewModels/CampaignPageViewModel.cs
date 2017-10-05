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
    public class CampaignPageViewModel : ViewModelBase
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
            set { if (_selectCategoryVisible != value)
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
                    IsNextStageUnlocked = ActiveStage.IsUnLocked;
                }
            }
        }

        private bool _isNextStageUnlocked = false;
        public bool IsNextStageUnlocked
        {
            get
            {
                return _isNextStageUnlocked;
            }
            set
            {
                if (_isNextStageUnlocked != value)
                {
                    _isNextStageUnlocked = value;
                    RaisePropertyChanged(nameof(IsNextStageUnlocked));
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

            SelectCategoryVisible = false;
            IsBusy = false;
        }

        private ICommand _selectSectionCommand;
        public ICommand SelectSectionCommand =>
            _selectSectionCommand ?? (_selectSectionCommand = new Command<string>(OnSelectSection, CanSelectSection));

        private bool CanSelectSection(string parm)
        {
            return true;
        }

        private async void OnSelectSection(string parm)
        {
            IsBusy = true;
            int obj = int.Parse(parm);

            App.ActiveSection = ActiveStage.Sections[obj];
            if (!App.ActiveSection.IsComplete)
            {
                int sectionId = ActiveStage.Sections[obj].CampaignSectionId;

                CampaignSection returnSection = await (GetSection(sectionId));
                App.ActiveSection.Questions = returnSection.Questions;
                App.ActiveSection.NumberOfQuestions = returnSection.Questions.Count();
                await Navigation.PushModalAsync(new QuestionPage());
            }
            IsBusy = false;
        }

        private ICommand _nextStageCommand;
        public ICommand NextStageCommand =>
            _nextStageCommand ?? (_nextStageCommand = new Command(OnNextStage, CanNextStage));

        private bool CanNextStage()
        {
            return true;
        }

        private async void OnNextStage()
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

        private ICommand _previousStageCommand;
        public ICommand PreviousStageCommand =>
            _previousStageCommand ?? (_previousStageCommand = new Command(OnPreviousStage, CanPreviousStage));

        private bool CanPreviousStage()
        {
            return (ActiveStage.StageLevel > 1);
        }

        private void OnPreviousStage()
        {
            int stageLevel = ActiveStage.StageLevel;

            ActiveStage = CampaignObj.Stages.Where(o => o.StageLevel == (stageLevel - 1)).FirstOrDefault();
        }

        #endregion

        public CampaignPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Device.BeginInvokeOnMainThread(async () =>
            {
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
            });
            IsNextStageUnlocked = ActiveStage.IsUnLocked;
        }


        public async Task<CampaignSection> GetSection(int sectionId)
        {
            CampaignSection section;
            section = await WebApi.Instance.GetSectionAsync(sectionId);
            return section;
        }
    }
}

namespace TriviaTraverse.Converters
{
    public class SectionStatusImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Xamarin.Forms.ImageSource))
                throw new InvalidOperationException("The target must be a integer");

            string retval = "sectionunlocked.png";

            switch (int.Parse(value.ToString()))
            {
                case 0:
                    retval = "sectionunlocked.png";
                    break;
                case 1:
                    retval = "sectionpie1.png";
                    break;
                case 2:
                    retval = "sectionpie2.png";
                    break;
                case 3:
                    retval = "sectionpie3.png";
                    break;
                case 4:
                    retval = "sectionpie4.png";
                    break;
                case 5:
                    retval = "sectioncomplete.png";
                    break;
            }

            return retval;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
