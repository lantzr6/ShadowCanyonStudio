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

    public enum GameSectionType
    {
        Campaign,
        CampaignTutorial,
        VersusFirst,
        VersusRegular
    }

    public enum GameMode
    {
        Campaign,
        VGame,
        Tutorial
    }

    //public class UserData
    //{

    //    private bool _isLoginActive;
    //    public bool IsLoginActive
    //    {
    //        get { return _isLoginActive; }
    //        set
    //        {
    //            if (_isLoginActive != value)
    //            {
    //                _isLoginActive = value;
    //                Settings.UserDataStorage = this;
    //            }
    //        }
    //    }

    //    private int _campaignStageLevel;
    //    public int CampaignStageLevel
    //    {
    //        get { return _campaignStageLevel; }
    //        set
    //        {
    //            if (_campaignStageLevel != value)
    //            {
    //                _campaignStageLevel = value;
    //                Settings.UserDataStorage = this;
    //            }
    //        }
    //    }


    //}



    public class Player : INotifyPropertyChanged
    {
        public Player()
        {
            LastStepUpdate = DateTime.Today;
        }
        public int PlayerId { get; set; }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    RaisePropertyChanged(nameof(UserName));
                }
            }
        }
        private string _emailAddr;
        public string EmailAddr
        {
            get { return _emailAddr; }
            set
            {
                if (_emailAddr != value)
                {
                    _emailAddr = value;
                    RaisePropertyChanged(nameof(EmailAddr));
                }
            }
        }
        private bool _fbLogin;
        public bool FbLogin
        {
            get { return _fbLogin; }
            set
            {
                if (_fbLogin != value)
                {
                    _fbLogin = value;
                    RaisePropertyChanged(nameof(FbLogin));
                }
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged(nameof(Password));
                }
            }
        }
        private int _playerLevel;
        public int PlayerLevel
        {
            get { return _playerLevel; }
            set
            {
                if (_playerLevel != value)
                {
                    _playerLevel = value;
                    RaisePropertyChanged(nameof(PlayerLevel));
                }
            }
        }
        private int _currentSteps;
        public int CurrentSteps
        {
            get { return _currentSteps; }
            set
            {
                if (_currentSteps != value)
                {
                    _currentSteps = value;
                    RaisePropertyChanged(nameof(CurrentSteps));
                }
            }
        }
        private int _stepBank;
        public int StepBank
        {
            get { return _stepBank; }
            set
            {
                if (_stepBank != value)
                {
                    _stepBank = value;
                    RaisePropertyChanged(nameof(StepBank));
                }
            }
        }
        private DateTime _lastStepUpdate;
        public DateTime LastStepUpdate
        {
            get { return _lastStepUpdate; }
            set
            {
                if (_lastStepUpdate != value)
                {
                    _lastStepUpdate = value;
                    RaisePropertyChanged(nameof(LastStepUpdate));
                }
            }
        }
        private int _coins;
        public int Coins
        {
            get { return _coins; }
            set
            {
                if (_coins != value)
                {
                    _coins = value;
                    RaisePropertyChanged(nameof(Coins));
                }
            }
        }
        private int _stars;
        public int Stars
        {
            get { return _stars; }
            set
            {
                if (_stars != value)
                {
                    _stars = value;
                    RaisePropertyChanged(nameof(Stars));
                }
            }
        }
        private int _points;
        public int Points
        {
            get { return _points; }
            set
            {
                if (_points != value)
                {
                    _points = value;
                    RaisePropertyChanged(nameof(Points));
                }
            }
        }

        public DateTime LastStepUpdateLocal
        {
            get { return this.LastStepUpdate.ToLocalTime(); }
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

    public class TutorialMessagesStatus : INotifyPropertyChanged
    {
        public bool QuestionStatusOne { get; set; }
        public bool QuestionStatusTwo { get; set; }

        public bool CampaignSectionNewlyComplete { get; set; }
        public bool CampaignStageNewlyUnlocked { get; set; }
        public bool CampaignStageBonus { get; set; }

        public TutorialMessagesStatus()
        {
            QuestionStatusOne = false;
            QuestionStatusTwo = false;
            CampaignSectionNewlyComplete = false;
            CampaignStageNewlyUnlocked = false;
            CampaignStageBonus = false;
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

    public class Dashboard : INotifyPropertyChanged
    {
        private List<Campaign> _campaigns;
        public List<Campaign> Campaigns
        {
            get { return _campaigns; }
            set
            {
                _campaigns = value;
                RaisePropertyChanged(nameof(Campaign));
            }
        }
        private List<VGame> _vGames;
        public List<VGame> VGames
        {
            get { return _vGames; }
            set
            {
                _vGames = value;
                RaisePropertyChanged(nameof(VGames));
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
        public int? SelectedAnswerIdx { get; set; }
        public string SelectedAnswerValue
        {
            get
            {
                if (SelectedAnswerIdx > -1 && (SelectedAnswerIdx < Answers.Count - 1))
                {
                    return Answers[SelectedAnswerIdx.Value];
                }
                else
                {
                    return "";
                }
            }
        }
        public CurrentQuestion()
        {
            //SelectedAnswerIdx = -1;
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

    public class GameSection : INotifyPropertyChanged
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
                return (NumberAnswered == 5);
            }
        }
        private bool _newlyComplete = false;
        public bool NewlyComplete
        {
            get
            {
                return _newlyComplete;
            }
            set
            {
                if (_newlyComplete != value)
                {
                    _newlyComplete = value;
                    RaisePropertyChanged(nameof(NewlyComplete));
                }
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
        GameSectionType _sectionType;
        public GameSectionType SectionType
        {
            get
            {
                return _sectionType;
            }
            set
            {
                _sectionType = value;
                RaisePropertyChanged(nameof(SectionType));
            }
        }

        public GameSection() { }
        public GameSection(VGameCategory vcat)
        {
            this.SectionType = GameSectionType.VersusRegular;
            this.Questions = vcat.Questions;
            this.SectionName = vcat.CategoryName;
            this.NumberOfQuestions = 5;
            this.NumberAnswered = 0;
            this.NumberCorrect = 0;
            this.EarnedStars = 0;
            this.EarnedPoints = 0;
            this.EarnedCoins = 0;
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
                return (QuestionType.Trim() == "Multi 4");
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
        public bool IsUnanswered { get; set; }
        //private bool _isUnanswered = true;
        //public bool IsUnanswered
        //    {
        //    get { return _isUnanswered; }
        //    set
        //    {
        //        if (value != _isUnanswered)
        //        {
        //            _isUnanswered = value; RaisePropertyChanged(nameof(IsUnanswered));
        //        }
        //    }
        //    }
        public bool PlayerCorrect
        {
            get
            {
                return (PlayerAnswer == AnswerCorrect);
            }
        }
        public int PointValue
        {
            get { return QuestionLevel * 100; }
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

    public class PlayerQuestionResult
    {
        public int PlayerQuestionResultId { get; set; }
        public int PlayerId { get; set; }
        public int QuestionId { get; set; }
        public string PlayerAnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsRewarded { get; set; }
    }
    public class PlayerVGameQuestionResult
    {
        public int PlayerQuestionResultId { get; set; }
        public int PlayerId { get; set; }
        public int QuestionId { get; set; }
        public string PlayerAnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsRewarded { get; set; }
        public int VGameId { get; set; }
    }
    public class VGamePlayerUpdate
    {
        public int VGameId { get; set; }
        public int PlayerId { get; set; }
        public int GameSteps { get; set; }
        public DateTime LastStepUpdate { get; set; }
    }

    public class CampaignStageCategory
    {
        public int CampaignStageCategoryId { get; set; }
        public int StageLevel { get; set; }
        public int StarsRequired { get; set; }
        public IList<GameSection> Sections { get; set; }
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

    public class VGame : INotifyPropertyChanged
    {
        public int VGameId { get; set; }
        public string GameName { get; set; }
        public string GameTypeName { get; set; }
        public bool IsPrivate { get; set; }
        public int GameStepCap { get; set; }
        public int GameLength { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PlayerMax { get; set; }
        public int PlayerGameSteps { get; set; }
        public int PlayerScore { get; set; }
        public DateTime PlayerLastStepQuery { get; set; }

        public DateTime StartTimeLocal { get { return this.StartTime.ToLocalTime(); } }
        public DateTime EndTimeLocal { get { return this.EndTime.ToLocalTime(); } }
        public DateTime PlayerLastStepQueryLocal { get { return this.PlayerLastStepQuery.ToLocalTime(); } }
        public int TotalPlayers { get { return VGamePlayers.Count; } }

        public bool IsActive
        {
            get
            {
                DateTime now = DateTime.Now;
                return (now >= StartTimeLocal && now < EndTimeLocal);
            }
        }

        public IList<VGamePlayer> VGamePlayers { get; set; }
        public IList<GameSection> Sections { get; set; }
        public IList<VGameCategory> CategoryQueue { get; set; }

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

    public class VGameSelectedCategories
    {
        public int PlayerId { get; set; }
        public int VGameId { get; set; }
        public IList<VGameCategory> SelectedCategories { get; set; }

        public VGameSelectedCategories()
        {
            this.SelectedCategories = new List<VGameCategory>();
        }
    }

    public class VGamePlayer
    {
        public int VGamePlayerId { get; set; }
        public string UserName { get; set; }
        public int GameSteps { get; set; }
        public int Score { get; set; }
        public int Stage { get; set; }
        public int QuestionsAnswered { get; set; }
    }

    public class VGameCategory
    {
        public int VGameCategoryId { get; set; }
        public int Stage { get; set; }
        public string CategoryName { get; set; }
        public bool IsUsed { get; set; }
        public bool IsNotUsed
        {
            get { return !IsUsed; }
        }
        public List<Question> Questions { get; set; }  //holds questions to build stage on device
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

    public class StepData
    {
        public int VGameId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Steps { get; set; }
        public bool Complete { get; set; }

        public StepData()
        {
            Steps = 0;
            EndTime = DateTime.MinValue;
            Complete = false;
        }
    }

}
