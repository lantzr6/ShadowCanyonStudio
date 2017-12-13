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

        public Player PlayerObj
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

        public bool IsNotAnswered
        {
            get
            {
                return ActiveSection.ActiveQuestion.SelectedAnswerIdx == null;
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


        private ICommand _powerUpMoreTimeCommand;
        public ICommand PowerUpMoreTimeCommand =>
            _powerUpMoreTimeCommand ?? (_powerUpMoreTimeCommand = new Command(OnPowerUpMoreTime, CanPowerUpMoreTime));
        private bool CanPowerUpMoreTime()
        {
            return (PlayerObj.Coins >= 100);
        }
        private void OnPowerUpMoreTime()
        {
            Countdown.ResetSuccess += delegate
            {
                PlayerObj.Coins -= 100;
            };
            Countdown.ResetTime();
        }
        #endregion

        public QuestionPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Countdown = new Countdown();
            Countdown.StartUpdating(10, .01);
            Countdown.Completed += delegate
            {
                CountdownExpired();
            };
        }

        public void CountdownExpired()
        {
            Countdown.StopUpdating();
            AnswerQuestion(-1);
        }

        public async void AnswerQuestion(int answerIdx)
        {
            if (ActiveSection.ActiveQuestion.SelectedAnswerIdx is null || answerIdx == -1) //only run if not answered yet or manually set by timeout
            {
                IsBusy = true;
                Countdown.StopUpdating();
                CurrentQuestion CQ = ActiveSection.ActiveQuestion;
                bool isCorrect = false;
                int pointsRewarded = 0;

                int curQuestionLevel = CQ.BaseSectionCategoryQuestion.QuestionLevel;

                CQ.SelectedAnswerIdx = answerIdx;
                CQ.BaseSectionCategoryQuestion.IsUnanswered = false;

                ActiveSection.NumberAnswered++;
                if (answerIdx == CQ.CorrectAnswerIdx)
                {
                    ActiveSection.EarnedPoints += CQ.PointValue;
                    pointsRewarded = CQ.PointValue;
                    PlayerObj.Points += pointsRewarded;
                    ActiveSection.EarnedStars++;
                    PlayerObj.Stars++;
                    ActiveSection.NumberCorrect++;
                    isCorrect = true;
                }
                else
                {
                    CQ.WrongAnswerIdx = answerIdx;
                }

                if (isCorrect)
                {
                    CQ.QuestionModeCorrect = true;
                }
                else
                {
                    CQ.QuestionModeWrong = true;
                }

                //update bindings
                RaisePropertyChanged(nameof(ActiveSection));
                RaisePropertyChanged(nameof(IsNotAnswered));

                //update database - combine this later - TODO
                if (App.GameMode == GameMode.Campaign || App.GameMode == GameMode.Tutorial)
                {
                    PlayerQuestionResult inObj = new PlayerQuestionResult();
                    inObj.QuestionId = CQ.BaseSectionCategoryQuestion.QuestionId;
                    inObj.PlayerId = PlayerObj.PlayerId;
                    inObj.PlayerAnswerText = (answerIdx > 0 ? CQ.Answers[answerIdx] : "");
                    inObj.IsCorrect = isCorrect;
                    inObj.PointsRewarded = pointsRewarded;
                    var retval = await WebApi.Instance.PostCampaignQuestionResults(inObj);

                    App.CampaignObj = App.CampaignObj; // ?????
                }
                else
                {
                    App.VGameObj.PlayerScore += pointsRewarded;

                    PlayerVGameQuestionResult inObj = new PlayerVGameQuestionResult();
                    inObj.QuestionId = CQ.BaseSectionCategoryQuestion.QuestionId;
                    inObj.PlayerId = PlayerObj.PlayerId;
                    inObj.PlayerAnswerText = (answerIdx > 0 ? CQ.Answers[answerIdx] : "");
                    inObj.IsCorrect = isCorrect;
                    inObj.PointsRewarded = pointsRewarded;
                    inObj.VGameId = App.VGameObj.VGameId; //only difference - combine later
                    var retval = await WebApi.Instance.PostVGameQuestionResults(inObj);
                }
                IsBusy = false;


            }
        }

    }
}
