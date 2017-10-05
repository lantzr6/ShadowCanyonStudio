using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TriviaTraverse.Helpers;
using Xamarin.Forms;

namespace TriviaTraverse.Models
{
    public enum GameStatus
    {
        Waiting,
        InProgress,
        Complete
    }

    public class UserData
    {
        private bool _isLoginActive;
        public bool IsLoginActive
        {
            get { return _isLoginActive; }
            set
            {
                if (_isLoginActive != value)
                {
                    _isLoginActive = value;
                    Settings.UserDataStorage = this;
                }
            }
        }

        private int _campaignStageLevel;
        public int CampaignStageLevel
        {
            get { return _campaignStageLevel; }
            set
            {
                if (_campaignStageLevel != value)
                {
                    _campaignStageLevel = value;
                    Settings.UserDataStorage = this;
                }
            }
        }


    }



    public class Player
    {
        public int PlayerId { get; set; }
        public string UserName { get; set; }
        public string EmailAddr { get; set; }
        public string Password { get; set; }
        public int PlayerLevel { get; set; }
        public int TutorialInfoLevel { get; set; }
        public int CurrentSteps { get; set; }
        public int StepBank { get; set; }
        public int Coins { get; set; }
        public int Stars { get; set; }
        public int Points { get; set; }
    }


    public class Dashboard
    {
        public List<Game> ActiveGames { get; set; }
    }

    public class SelectableItemWrapper<T>
    {
        public bool IsSelected { get; set; }
        public T Item { get; set; }
    }

    //public class User : INotifyPropertyChanged
    //{
    //    string fullname;
    //    public string Fullname
    //    {
    //        get { return fullname; }

    //        set
    //        {
    //            fullname = value;
    //            OnPropertyChanged();
    //        }
    //    }
    //    string password;
    //    public string Password
    //    {
    //        get { return password; }

    //        set
    //        {
    //            password = value;
    //            OnPropertyChanged();
    //        }
    //    }
    //    string email;
    //    public string Email
    //    {
    //        get { return email; }

    //        set
    //        {
    //            email = value;
    //            OnPropertyChanged();
    //        }
    //    }

    //    #region INotifyPropertyChanged implementation

    //    /// <summary>
    //    /// Occurs when property changed.
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    /// <summary>
    //    /// Raises the property changed event.
    //    /// </summary>
    //    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null)
    //        {
    //            handler(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion
    //}

    public class CurrentQuestion : INotifyPropertyChanged
    {
        public Question BaseSectionCategoryQuestion { get; set; }
        public string CurrentCategoryName { get; set; }
        public int PointValue
        {
            get
            {
                return BaseSectionCategoryQuestion.QuestionLevel * 100;
            }
        }
        public List<string> Answers { get; set; }  //randomized
        public int CorrectAnswerIdx { get; set; }
        public int WrongAnswerIdx { get; set; }
        public int SelectedAnswerIdx { get; set; }
        public string SelectedAnswerValue
        {
            get
            {
                if (SelectedAnswerIdx > -1 && (SelectedAnswerIdx < Answers.Count - 1))
                {
                    return Answers[SelectedAnswerIdx];
                }
                else
                {
                    return "";
                }
            }
        }
        public CurrentQuestion()
        {
            SelectedAnswerIdx = -1;
            CorrectAnswerIdx = -1;
            WrongAnswerIdx = -1;
        }
        public bool IsLocked { get; set; }
        bool _questionModeActive = false;
        public bool QuestionModeActive
        {
            get
            {
                return _questionModeActive;
            }
            set
            {
                _questionModeActive = value;
                if (value)
                {
                    QuestionModeCorrect = false;
                    QuestionModeWrong = false;
                }
                RaisePropertyChanged(nameof(QuestionModeActive));
            }
        }
        bool _questionModeCorrect = false;
        public bool QuestionModeCorrect
        {
            get
            {
                return _questionModeCorrect;
            }
            set
            {
                _questionModeCorrect = value;
                if (value)
                {
                    QuestionModeActive = false;
                    QuestionModeWrong = false;
                }
                RaisePropertyChanged(nameof(QuestionModeCorrect));
            }
        }
        bool _questionModeWrong = false;
        public bool QuestionModeWrong
        {
            get
            {
                return _questionModeWrong;
            }
            set
            {
                _questionModeWrong = value;
                if (value)
                {
                    QuestionModeActive = false;
                    QuestionModeCorrect = false;
                }
                RaisePropertyChanged(nameof(QuestionModeWrong));
            }
        }

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class CampaignSection : INotifyPropertyChanged
    {
        public int CampaignSectionId { get; set; }
        public List<Question> Questions { get; set; }
        CurrentQuestion _activeQuestion;
        public CurrentQuestion ActiveQuestion
        {
            get
            {
                return _activeQuestion;
            }
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged(nameof(ActiveQuestion));
            }
        }
        public bool IsComplete
        {
            get
            {
                return (NumberAnswered >= NumberOfQuestions);  //should never by greater than, but just incase
            }
        }

        string _sectionName = "";
        public string SectionName
        {
            get
            {
                return _sectionName;
            }
            set
            {
                _sectionName = value;
                RaisePropertyChanged(nameof(SectionName));
            }
        }

        public string QuestionProgress
        {
            get
            {
                return string.Format("{0} of {1} questions", NumberAnswered, NumberOfQuestions);
            }
        }
        int _numberOfQuestions;
        public int NumberOfQuestions
        {
            get
            {
                return _numberOfQuestions;
            }
            set
            {
                _numberOfQuestions = value;
                RaisePropertyChanged(nameof(NumberOfQuestions));
                RaisePropertyChanged(nameof(QuestionProgress));
            }
        }
        int _numberAnswered;
        public int NumberAnswered
        {
            get
            {
                return _numberAnswered;
            }
            set
            {
                _numberAnswered = value;
                RaisePropertyChanged(nameof(NumberAnswered));
                RaisePropertyChanged(nameof(QuestionProgress));
            }
        }
        int _numberCorrect;
        public int NumberCorrect
        {
            get
            {
                return _numberCorrect;
            }
            set
            {
                _numberCorrect = value;
                RaisePropertyChanged(nameof(NumberCorrect));
            }
        }
        int _earnedStars;
        public int EarnedStars
        {
            get
            {
                return _earnedStars;
            }
            set
            {
                _earnedStars = value;
                RaisePropertyChanged(nameof(EarnedStars));
            }
        }
        int _earnedPoints;
        public int EarnedPoints
        {
            get
            {
                return _earnedPoints;
            }
            set
            {
                _earnedPoints = value;
                RaisePropertyChanged(nameof(EarnedPoints));
            }
        }
        int _earnedCoins;
        public int EarnedCoins
        {
            get
            {
                return _earnedCoins;
            }
            set
            {
                _earnedCoins = value;
                RaisePropertyChanged(nameof(EarnedCoins));
            }
        }

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    //public class CampaignSection
    //{
    //    public int CampaignSectionId { get; set; }
    //    public string SectionName { get; set; }
    //    public IList<SectionQuestion> SectionQuestions { get; set; }
    //    public int NumberOfQuestions { get; set; }
    //    public int NumberAnswered { get; set; }
    //    public int EarnedStars { get; set; }
    //    public int EarnedPoints { get; set; }
    //}

    //public class SectionQuestion
    //{
    //    public string CategoryName { get; set; }
    //    public SectionCategoryQuestion SectionCategoryQuestion { get; set; }
    //}

    public class Campaign
    {
        public int PlayerCampaignId { get; set; }
        public IList<CampaignStageCategory> Stages { get; set; }
        public IList<CampaignCategory> CategoryQueue { get; set; }

        public CampaignStageCategory CurrentStage
        {
            get
            {
                //return Stages.Where(o => o.StageLevel > 0).OrderByDescending(o => o.StageLevel).FirstOrDefault();
                return Stages.OrderByDescending(o => o.StageLevel).FirstOrDefault();
            }
        }
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public string CategoryName { get; set; }
        public string Text { get; set; }
        public int QuestionLevel { get; set; }
        public string QuestionType { get; set; }
        public bool IsMultiChoice
        {
            get
            {
                return (QuestionType.Trim() == "MultipleChoice");
            }
        }
        public bool HasImage { get; set; }
        public string ImageUrl { get; set; }
        public string AnswerCorrect { get; set; }
        public string AnswerWrong1 { get; set; }
        public string AnswerWrong2 { get; set; }
        public string AnswerWrong3 { get; set; }
        public string PlayerAnswer { get; set; }
        public int PointsRewarded { get; set; }
    }

    public class PlayerQuestionResult
    {
        public int PlayerQuestionResultId { get; set; }
        public int PlayerId { get; set; }
        public int QuestionId { get; set; }
        public string PlayerAnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsRewarded { get; set; }
    }

    public class CampaignStageCategory
    {
        public int CampaignStageCategoryId { get; set; }
        public int StageLevel { get; set; }
        public int StarsRequired { get; set; }
        public IList<CampaignSection> Sections { get; set; }
        public bool IsUnLocked
        {
            get
            {
                return (App.PlayerObj.Stars >= StarsRequired);
            }
        }
        

    }

    public class NewCampaignStageInfo
    {
        public int PlayerCampaignId { get; set; }
        public int CampaignCategoryId { get; set; }
        public int StageLevel { get; set; }
    }
    public class NewCampaignStageReturn
    {
        public CampaignStageCategory NewStage { get; set; }
        public IList<CampaignCategory> CategoryQueue { get; set; }
    }

    public class CampaignCategory
    {
        public int CampaignCategoryId { get; set; }
        public string CategoryName { get; set; }
    }








    public class Game
    {
        public int GameId { get; set; }
        public string StartedAt { get; set; }
        public GameStatus Status { get; set; }
        public bool IsPlayReady
        {
            get
            {
                return (this.Status == GameStatus.InProgress);
            }
        }
        public int Winner { get; set; }
        public GamePlayer SelfPlayer { get; set; }
        public List<GamePlayer> OpponentPlayers { get; set; }
        public List<GameCategoryBasic> GameCategories { get; set; }
    }



    public class GamePlayer
    {
        public int GamePlayerId { get; set; }
        public int PlayerId { get; set; }
        public GameCategory GameCategory1 { get; set; }
        public GameCategory GameCategory2 { get; set; }
        public GameCategory GameCategory3 { get; set; }
        public int CurrentCategoryNumber { get; set; }
        public int CurrentQuestionLevel { get; set; }
        public CurrentQuestion ActiveQuestion { get; set; }
        public int TotalScore { get; set; }
        public bool IsComplete { get; set; }
    }

    public class GameCategoryQuestion
    {
        public int GameCategoryQuestionId { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int QuestionLevel { get; set; }
        public string QuestionType { get; set; }
        public bool IsMultiChoice
        {
            get
            {
                return (QuestionType.Trim() == "MultipleChoice");
            }
        }
        public bool HasImage { get; set; }
        public string ImageUrl { get; set; }
        public string AnswerCorrect { get; set; }
        public string AnswerWrong1 { get; set; }
        public string AnswerWrong2 { get; set; }
        public string AnswerWrong3 { get; set; }
        public string PlayerAnswer { get; set; }
        public int PointsRewarded { get; set; }
    }


    public class GamePlayerQuestionResult
    {
        public int GamePlayerId { get; set; }
        public int CurrentCategoryNum { get; set; }
        public int CurrentQuestionLevel { get; set; }
        public bool IsComplete { get; set; }
        public int TotalScore { get; set; }
        public int GameCategoryQuestionId { get; set; }
        public string PlayerAnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsRewards { get; set; }
    }

    public class GameCategory
    {
        public int GameCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<GameCategoryQuestion> GameCategoryQuestions { get; set; }
    }
    public class GameCategoryBasic
    {
        public int GameCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

}
