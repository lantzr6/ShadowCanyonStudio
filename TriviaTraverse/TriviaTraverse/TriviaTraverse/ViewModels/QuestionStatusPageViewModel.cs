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
                        new Span { Text = PlayerObj.StepsNeeded.ToString(), FontAttributes=FontAttributes.Bold, FontSize=28 },
                        new Span { Text = " steps before your next question is unlocked", FontSize=18 } }
                };
            }
        }

        #endregion

        #region "Commands"
        private ICommand _closeTutorialStepCommandCommand;
        public ICommand CloseTutorialStepCommand =>
            _closeTutorialStepCommandCommand ?? (_closeTutorialStepCommandCommand = new Command(CloseTutorialStep, CanCloseTutorialStep));
        private bool CanCloseTutorialStep()
        {
            return true;
        }
        private void CloseTutorialStep()
        {
            switch (PlayerObj.TutorialInfoLevel)
            {
                case 1:
                    PlayerObj.TutorialInfoLevel++;
                    break;
                case 2:
                    PlayerObj.StepBank += 5000;
                    PlayerObj.TutorialInfoLevel++;
                    break;
            }
        }

        private ICommand _startQuestionCommand;
        public ICommand StartQuestionCommand =>
            _startQuestionCommand ?? (_startQuestionCommand = new Command(StartQuestion, CanStartQuestion));

        private bool CanStartQuestion()
        {
            return PlayerObj.QuestionReady;
        }

        private async void StartQuestion()
        {
            BuildCurrentQuestion();
            ActiveSection.ActiveQuestion.QuestionModeActive = true;
            PlayerObj.StepBank -= 1000;
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
                Navigation.PopAsync();
            }
            else
            {
                PlayerObj.TutorialInfoLevel = 3;
                Navigation.PushModalAsync(new SignUpPage());
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
            RaisePropertyChanged("StepsNeededFormattedText");
        }


        public void BuildCurrentQuestion()
        {
            CurrentQuestion CQ = ActiveSection.ActiveQuestion;
            CQ = new CurrentQuestion();

            Question SQ = null;
            SQ = ActiveSection.Questions.Where(o => o.QuestionLevel == (ActiveSection.NumberAnswered + 1)).FirstOrDefault();
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

            CQ.IsLocked = (PlayerObj.StepBank < 1000);
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
