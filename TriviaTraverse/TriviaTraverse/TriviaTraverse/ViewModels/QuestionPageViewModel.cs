using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    public class QuestionPageViewModel : ViewModelBase
    {

        #region "Properties"

        public PlayerLocal PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private Countdown _countdown;
        public Countdown Countdown
        {
            get
            {
                return _countdown;
            }
            set
            {
                _countdown = value;
                RaisePropertyChanged(nameof(Countdown));
            }
        }
        string _currentCategoryName = "";
        public string CurrentCategoryName
        {
            get
            {
                return _currentCategoryName;
            }
            set
            {

            }
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

        private ICommand _answerQuestionCommand;
        public ICommand AnswerQuestionCommand =>
            _answerQuestionCommand ?? (_answerQuestionCommand = new Command<string>(AnswerQuestion, CanAnswerQuestion));

        private bool CanAnswerQuestion(string parm)
        {
            return true;
        }

        private void AnswerQuestion(string parm)
        {
            AnswerQuestion(int.Parse(parm));
        }

        private ICommand _continueCommand;
        public ICommand ContinueCommand =>
            _continueCommand ?? (_continueCommand = new Command(Continue, CanContinue));

        private bool CanContinue()
        {
            return true;
        }

        private async void Continue()
        {
            Countdown.ResetUpdating();  //reset now so the progress bar resets
            //if (ActiveSection.ActiveQuestion.BaseSectionCategoryQuestion.QuestionLevel == ActiveSection.LastQuestionLevel)
            //{
            //    if (!Settings.TutorialIsComplete) { Settings.TutorialIsComplete = true; }
            //    await _navigationService.NavigateAsync("/SectionCompletePage");
            //}
            //else
            //{
            //if (App.GameMode == "Campaign")
            //{
            //    await _navigationService.NavigateAsync("/Master/Navigation/DashboardPage/CampaignPage/QuestionStatusPage");
            //}
            //else
            //{
            //    await _navigationService.NavigateAsync("/StartPage/QuestionStatusPage",);
            //}
            //}

            await Navigation.PopModalAsync();
        }

        private ICommand _viewStatsCommand;
        public ICommand ViewStatsCommand =>
            _viewStatsCommand ?? (_viewStatsCommand = new Command(ViewStats, CanViewStats));

        private bool CanViewStats()
        {
            return true;
        }

        private void ViewStats()
        {
            /// view stats page
        }

        #endregion

        public QuestionPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Countdown = new Countdown();
            Countdown.StartUpdating(10, .25);
            Countdown.Completed += delegate
            {
                CountdownExpired();
            };
        }

        private void CountdownExpired()
        {
            Countdown.StopUpdating();
            AnswerQuestion(-1);
        }

        public async void AnswerQuestion(int answerIdx)
        {
            if (ActiveSection.ActiveQuestion.SelectedAnswerIdx == -1 || answerIdx == -1) //only run if not answered yet or manually set by timeout
            {
                IsBusy = true;
                Countdown.StopUpdating();
                bool isCorrect = false;
                int pointsRewarded = 0;

                int curQuestionLevel = ActiveSection.ActiveQuestion.BaseSectionCategoryQuestion.QuestionLevel;

                ActiveSection.ActiveQuestion.SelectedAnswerIdx = answerIdx;
                //CurrentQuestion.BaseGameCategoryQuestion.PlayerAnswer = answer;
                //CurrentQuestion.BaseGameCategoryQuestion.PointsRewarded = (curQuestionLevel * 100);

                ActiveSection.NumberAnswered++;
                if (answerIdx == ActiveSection.ActiveQuestion.CorrectAnswerIdx)
                {
                    //await App.MainPage.DisplayAlert("Result", "Correct !!", "OK");
                    ActiveSection.EarnedPoints += (curQuestionLevel * 100);
                    pointsRewarded = (curQuestionLevel * 100);
                    PlayerObj.Points += pointsRewarded;
                    ActiveSection.EarnedStars++;
                    PlayerObj.Stars++;
                    ActiveSection.NumberCorrect++;
                    isCorrect = true;
                }
                else
                {
                    ActiveSection.ActiveQuestion.WrongAnswerIdx = answerIdx;
                }
                //else
                //{
                //    await App.MainPage.DisplayAlert("Result", "Wrong :(", "OK");
                //}
                //CurrentQuestion.BaseGameCategoryQuestion.PointsRewarded = newPoints;

                if (isCorrect)
                {
                    ActiveSection.ActiveQuestion.QuestionModeCorrect = true;
                }
                else
                {
                    ActiveSection.ActiveQuestion.QuestionModeWrong = true;
                }

                //update database

                
                PlayerQuestionResult inObj = new PlayerQuestionResult();
                inObj.QuestionId = ActiveSection.ActiveQuestion.BaseSectionCategoryQuestion.QuestionId;
                inObj.PlayerId = PlayerObj.PlayerId;
                inObj.PlayerAnswerText = ActiveSection.ActiveQuestion.Answers[answerIdx];
                inObj.IsCorrect = isCorrect;
                inObj.PointsRewarded = pointsRewarded;
                var retval = await WebApi.Instance.PostQuestionResults(inObj);
                IsBusy = false;

                //update bindings
                RaisePropertyChanged(nameof(ActiveSection));
            }
        }

    }
}
