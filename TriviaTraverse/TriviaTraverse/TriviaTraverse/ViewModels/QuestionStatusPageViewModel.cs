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
    public class QuestionStatusPageViewModel : ViewModelBase
    {

        #region "Properties"

        public bool UserHeaderVisible { get; set; }
        public bool VGameHeaderVisible { get; set; }

        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }
        public VGame VGameObj
        {
            get { return App.VGameObj; }
        }

        public TutorialMessagesStatus TutorialObj
        {
            get { return App.TutorialObj; }
            set
            {
                App.TutorialObj = value;
                RaisePropertyChanged(nameof(TutorialOneIsVisable));
                RaisePropertyChanged(nameof(TutorialTwoIsVisable));
            }
        }

        public bool TutorialOneIsVisable
        {
            get { return TutorialObj.QuestionStatusOne == false; }
        }

        public bool TutorialTwoIsVisable
        {
            get { return TutorialObj.QuestionStatusOne == true && TutorialObj.QuestionStatusTwo == false && ActiveSection.SectionType == GameSectionType.CampaignTutorial; }
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

        public int StepsNeeded
        {
            get
            {
                int stepsNeeded = 0;
                switch (ActiveSection.SectionType)
                {
                    case GameSectionType.Campaign:
                        stepsNeeded = 1000 - PlayerObj.StepBank;
                        break;
                    case GameSectionType.CampaignTutorial:
                        stepsNeeded = 1000 - PlayerObj.StepBank;
                        break;
                    case GameSectionType.VersusRegular:
                        stepsNeeded = 1000 - VGameObj.PlayerGameSteps;
                        break;
                    case GameSectionType.VersusFirst:
                        stepsNeeded = 0;
                        break;
                }
                return stepsNeeded;
            }
        }
        public bool QuestionReady
        {
            get { return (StepsNeeded <= 0); }
        }

        private bool _isSectionComplete = false;
        public bool IsSectionComplete
        {
            get { return _isSectionComplete; }
            set { if (_isSectionComplete != value)
                {
                    _isSectionComplete = value;
                    RaisePropertyChanged(nameof(IsSectionComplete));
                }
            }
        }

        private bool _isReadyForSignUp = false;
        public bool IsReadyForSignUp
        {
            get { return _isReadyForSignUp; }
            set
            {
                if (_isReadyForSignUp != value)
                {
                    _isReadyForSignUp = value;
                    RaisePropertyChanged(nameof(IsReadyForSignUp));
                }
            }
        }

        public FormattedString StepsNeededFormattedText
        {
            get
            {
                return new FormattedString
                {
                    Spans = {
                        new Span { Text = StepsNeeded.ToString(), FontAttributes=FontAttributes.Bold, FontSize=28 },
                        new Span { Text = " steps before your next question is unlocked", FontSize=18 } }
                };
            }
        }

        #endregion

        #region "Commands"
        private ICommand _closeCommand;
        public ICommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new Command(OnClose, CanClose));
        private bool CanClose()
        {
            return true;
        }
        private async void OnClose()
        {
            await Navigation.PopAsync();
        }
        private ICommand _closeTutorialOneCommand;
        public ICommand CloseTutorialOneCommand =>
            _closeTutorialOneCommand ?? (_closeTutorialOneCommand = new Command(CloseTutorialOne, CanCloseTutorialOne));
        private bool CanCloseTutorialOne()
        {
            return true;
        }
        private void CloseTutorialOne()
        {
            TutorialObj.QuestionStatusOne = true;
            RaisePropertyChanged(nameof(TutorialOneIsVisable));
            RaisePropertyChanged(nameof(TutorialTwoIsVisable));
            App.UpdateTutorialData();
        }

        private ICommand _closeTutorialTwoCommand;
        public ICommand CloseTutorialTwoCommand =>
            _closeTutorialTwoCommand ?? (_closeTutorialTwoCommand = new Command(CloseTutorialTwo, CanCloseTutorialTwo));
        private bool CanCloseTutorialTwo()
        {
            return true;
        }
        private void CloseTutorialTwo()
        {
            PlayerObj.StepBank += 5000;
            TutorialObj.QuestionStatusTwo = true;
            RaisePropertyChanged(nameof(TutorialTwoIsVisable));
            RaisePropertyChanged(nameof(QuestionReady));
            RaisePropertyChanged(nameof(StepsNeeded));
            App.UpdatePlayerData();
            App.UpdateTutorialData();
        }

        private ICommand _startQuestionCommand;
        public ICommand StartQuestionCommand =>
            _startQuestionCommand ?? (_startQuestionCommand = new Command<object>(StartQuestion, CanStartQuestion));

        private bool CanStartQuestion(object value)
        {
            return QuestionReady;
        }

        private async void StartQuestion(object value)
        {
            BuildCurrentQuestion(int.Parse(value.ToString()));
            ActiveSection.ActiveQuestion.QuestionModeActive = true;
            switch (ActiveSection.SectionType)
            {
                case GameSectionType.Campaign:
                case GameSectionType.CampaignTutorial:
                    PlayerObj.StepBank -= 1000;
                    App.UpdatePlayerData();
                    break;
                case GameSectionType.VersusRegular:
                    VGameObj.PlayerGameSteps -= 1000;
                    VGamePlayerUpdate inObj = new VGamePlayerUpdate() { VGameId = VGameObj.VGameId, PlayerId = PlayerObj.PlayerId, GameSteps = VGameObj.PlayerGameSteps, LastStepUpdate = VGameObj.PlayerLastStepQuery };
                    App.VGameObj = await WebApi.Instance.UpdateVGamePlayerAsync(inObj);
                    break;
            }

            await Navigation.PushModalAsync(new QuestionPage());
        }

        private ICommand _continueCommand;
        public ICommand ContinueCommand =>
            _continueCommand ?? (_continueCommand = new Command(OnContinue, CanContinue));
        private bool CanContinue()
        {
            return true;
        }
        private void OnContinue()
        {
            if (PlayerObj.PlayerLevel > 0)  //logged in
            {
                ActiveSection.NewlyComplete = true;
                Navigation.PopModalAsync();
            }
            else
            {
                //PlayerObj.TutorialInfoLevel = 3;
                Navigation.PushModalAsync(new LoginPage());
            }
        }

        #endregion

        public QuestionStatusPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            PageSetup();
        }

        public void PageSetup()
        {
            if (App.GameMode == GameMode.VGame)
            {
                UserHeaderVisible = false;
                VGameHeaderVisible = true;
            }
            else
            {
                UserHeaderVisible = true;
                VGameHeaderVisible = false;
            }

            if (ActiveSection.IsComplete)
            {
                IsSectionComplete = true;
                if (PlayerObj.PlayerLevel > 0)  //logged in
                {
                    IsReadyForSignUp = false;
                }
                else
                {
                    IsReadyForSignUp = true;
                }
            }
            else
            {
                IsSectionComplete = false;
                IsReadyForSignUp = false;
            }
            RaisePropertyChanged(nameof(StepsNeeded));
            RaisePropertyChanged(nameof(QuestionReady));
            RaisePropertyChanged(nameof(StepsNeededFormattedText));
            RaisePropertyChanged(nameof(ActiveSection));
        }


        public void BuildCurrentQuestion(int questionId)
        {
            CurrentQuestion CQ = ActiveSection.ActiveQuestion;
            CQ = new CurrentQuestion();

            Question SQ = null;
            SQ = ActiveSection.Questions.Where(o => o.QuestionId == questionId).FirstOrDefault();
            CQ.CurrentCategoryName = SQ.CategoryName;

            CQ.BaseSectionCategoryQuestion = SQ;
            List<string> answers = new List<string>();
            answers.Add(SQ.AnswerCorrect);
            answers.Add(SQ.AnswerWrong1);
            if (SQ.IsMultiChoice)
            {
                answers.Add(SQ.AnswerWrong2);
                answers.Add(SQ.AnswerWrong3);
            }
            answers.Shuffle();
            CQ.Answers = answers;
            CQ.CorrectAnswerIdx = answers.IndexOf(SQ.AnswerCorrect);

            CQ.IsLocked = (!QuestionReady);
            ActiveSection.ActiveQuestion = CQ;
        }

        //public async void GetSteps()
        //{
        //    if (!bPermission) { GetHealthPermissionsAsync(); };

        //    if (bPermission)
        //    {
        //        Double stepsCnt = await DependencyService.Get<IDeviceSteps>().GetCumlativeSteps();
        //        UserCurrentSteps = Convert.ToInt32(stepsCnt);
        //    }
        //}
    }
}
