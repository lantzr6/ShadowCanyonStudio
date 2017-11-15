using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TriviaTraverse.Models;
using Newtonsoft.Json;

namespace TriviaTraverse.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        //private static Lazy<Settings> SettingsInstance = new Lazy<Settings>(() => new Settings());

        //public static Settings Current => SettingsInstance.Value;

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        //#region INotifyPropertyChanged

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        //{
        //    if (!string.IsNullOrWhiteSpace(propertyName))
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //#endregion INotifyPropertyChanged

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion

        //private static UserData _userDataStorage = null;
        //public static UserData UserDataStorage
        //{
        //    get
        //    {
        //        if (_userDataStorage is null)
        //        {
        //            string data = AppSettings.GetValueOrDefault(nameof(UserDataStorage), SettingsDefault);
        //            if (data == SettingsDefault)
        //            {
        //                _userDataStorage = new UserData();
        //            }
        //            else
        //            {
        //                _userDataStorage = JsonConvert.DeserializeObject<UserData>(data);
        //            }
        //        }
        //        return _userDataStorage;

        //    }
        //    set
        //    {
        //        string data = JsonConvert.SerializeObject(value);
        //        AppSettings.AddOrUpdateValue(nameof(UserDataStorage), data);
        //    }

        //}

        private static Player _currentPlayer = null;
        public static Player CurrentPlayer
        {
            get
            {
                if (_currentPlayer is null)
                {
                    string data = AppSettings.GetValueOrDefault(nameof(CurrentPlayer), SettingsDefault);
                    if (data == SettingsDefault)
                    {
                        _currentPlayer = new Player();
                    }
                    else
                    {
                        _currentPlayer = JsonConvert.DeserializeObject<Player>(data);
                    }
                }
                return _currentPlayer;
            }
            set
            {
                _currentPlayer = value;
                string data = null;
                if (value != null)
                {
                    data = JsonConvert.SerializeObject(value);
                }
                AppSettings.AddOrUpdateValue(nameof(CurrentPlayer), data);
            }

        }
        
        private static Campaign _userCampaign = null;
        public static Campaign UserCampaign
        {
            get
            {
                if (_userCampaign is null)
                {
                    string data = AppSettings.GetValueOrDefault(nameof(UserCampaign), SettingsDefault);
                    if (data == SettingsDefault)
                    {
                        _userCampaign = null;
                    }
                    else
                    {
                        _userCampaign = JsonConvert.DeserializeObject<Campaign>(data);
                    }
                }
                return _userCampaign;

            }
            set
            {
                _userCampaign = value;
                string data = null;
                if (value != null)
                {
                    data = JsonConvert.SerializeObject(value);
                }
                AppSettings.AddOrUpdateValue(nameof(UserCampaign), data);
            }

        }

        private static TutorialMessagesStatus _userTutorial = null;
        public static TutorialMessagesStatus UserTutorial
        {
            get
            {
                if (_userTutorial is null)
                {
                    string data = AppSettings.GetValueOrDefault(nameof(UserTutorial), SettingsDefault);
                    if (data == SettingsDefault)
                    {
                        _userTutorial = new TutorialMessagesStatus();
                    }
                    else
                    {
                        _userTutorial = JsonConvert.DeserializeObject<TutorialMessagesStatus>(data);
                    }
                }
                return _userTutorial;
            }
            set
            {
                _userTutorial = value;
                string data = "";
                if (value != null)
                {
                    data = JsonConvert.SerializeObject(value);
                }  
                AppSettings.AddOrUpdateValue(nameof(UserTutorial), data);
            }
        }


        //private static PlayerLocal _userPlayer = null;
        //public static PlayerLocal UserPlayer
        //{
        //    get
        //    {
        //        if (_userPlayer == null)
        //        {
        //            //build player from settings
        //            PlayerLocal localPlayer = new PlayerLocal();
        //            localPlayer.PlayerId = Settings.PlayerId;
        //            localPlayer.UserName = Settings.UserName;
        //            localPlayer.EmailAddr = Settings.EmailAddr;
        //            localPlayer.Password = Settings.Password;
        //            localPlayer.PlayerLevel = Settings.PlayerLevel;
        //            localPlayer.TutorialInfoLevel = Settings.TutorialInfoLevel;
        //            localPlayer.CurrentSteps = Settings.CurrentSteps;
        //            localPlayer.StepBank = Settings.StepBank;
        //            localPlayer.Coins = Settings.Coins;
        //            localPlayer.Stars = Settings.Stars;
        //            localPlayer.Points = Settings.Points;

        //            _userPlayer = localPlayer;
        //            return _userPlayer;
        //        }
        //        else
        //        {
        //            return _userPlayer;
        //        }
        //    }
        //}




        public static string AuthToken
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthToken), SettingsDefault);
            set => AppSettings.AddOrUpdateValue(nameof(AuthToken), value);
        }

        public static DateTime AuthTokenExpire
        {
            get => AppSettings.GetValueOrDefault(nameof(AuthTokenExpire), DateTime.MinValue);
            set => AppSettings.AddOrUpdateValue(nameof(AuthTokenExpire), value);
        }



        //private static int PlayerId
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(PlayerId), -1);
        //    set => AppSettings.AddOrUpdateValue(nameof(PlayerId), value);
        //}
        //private static string UserName
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(UserName), SettingsDefault);
        //    set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        //}
        //private static string EmailAddr
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(EmailAddr), SettingsDefault);
        //    set => AppSettings.AddOrUpdateValue(nameof(EmailAddr), value);
        //}
        //private static string Password
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(Password), SettingsDefault);
        //    set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        //}
        //private static int PlayerLevel
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(PlayerLevel), 0);
        //    set => AppSettings.AddOrUpdateValue(nameof(PlayerLevel), value);
        //}
        //private static int TutorialInfoLevel
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(TutorialInfoLevel), 0);
        //    set => AppSettings.AddOrUpdateValue(nameof(TutorialInfoLevel), value);
        //}
        //private static int CurrentSteps
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(CurrentSteps), 0);
        //    set => AppSettings.AddOrUpdateValue(nameof(CurrentSteps), value);
        //}
        //private static int StepBank
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(StepBank), 0);
        //    set => AppSettings.AddOrUpdateValue(nameof(StepBank), value);
        //}
        //private static int Coins
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(Coins), 0);
        //    set => AppSettings.AddOrUpdateValue(nameof(Coins), value);
        //}
        //private static int Stars
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(Stars), 0);
        //    set => AppSettings.AddOrUpdateValue(nameof(Stars), value);
        //}
        //private static int Points
        //{
        //    get => AppSettings.GetValueOrDefault(nameof(Points), 0);
        //    set => AppSettings.AddOrUpdateValue(nameof(Points), value);
        //}

        //public class PlayerLocal : IBindableBase
        //{
        //    private int _playerId = -1;
        //    public int PlayerId
        //    {
        //        get { return _playerId; }
        //        set
        //        {
        //            if (Settings.PlayerId != value)
        //            {
        //                Settings.PlayerId = value;
        //            }
        //            if (_playerId != value)
        //            {
        //                _playerId = value;
        //                OnPropertyChanged(nameof(PlayerId));
        //            }
        //        }
        //    }
        //    private string _userName;
        //    public string UserName
        //    {
        //        get { return _userName; }
        //        set
        //        {
        //            if (Settings.UserName != value)
        //            {
        //                Settings.UserName = value;
        //            }
        //            if (_userName != value)
        //            {
        //                _userName = value;
        //                OnPropertyChanged(nameof(UserName));
        //            }
        //        }
        //    }
        //    private string _emailAddr;
        //    public string EmailAddr
        //    {
        //        get { return _emailAddr; }
        //        set
        //        {
        //            if (Settings.EmailAddr != value)
        //            {
        //                Settings.EmailAddr = value;
        //            }
        //            if (_emailAddr != value)
        //            {
        //                _emailAddr = value;
        //                OnPropertyChanged(nameof(EmailAddr));
        //            }
        //        }
        //    }
        //    private string _password;
        //    public string Password
        //    {
        //        get { return _password; }
        //        set
        //        {
        //            if (Settings.Password != value)
        //            {
        //                Settings.Password = value;
        //            }
        //            if (_password != value)
        //            {
        //                _password = value;
        //                OnPropertyChanged(nameof(Password));
        //            }
        //        }
        //    }
        //    private int _playerLevel;
        //    public int PlayerLevel
        //    {
        //        get { return _playerLevel; }
        //        set
        //        {
        //            if (Settings.PlayerLevel != value)
        //            {
        //                Settings.PlayerLevel = value;
        //            }
        //            if (_playerLevel != value)
        //            {
        //                _playerLevel = value;
        //                OnPropertyChanged(nameof(PlayerLevel));
        //            }
        //        }
        //    }
        //    private int _tutorialInfoLevel;
        //    public int TutorialInfoLevel
        //    {
        //        get { return _tutorialInfoLevel; }
        //        set
        //        {
        //            if (Settings.TutorialInfoLevel != value)
        //            {
        //                Settings.TutorialInfoLevel = value;
        //            }
        //            if (_tutorialInfoLevel != value)
        //            {
        //                _tutorialInfoLevel = value;
        //                OnPropertyChanged(nameof(TutorialInfoLevel));
        //            }
        //        }
        //    }
        //    private int _currentSteps;
        //    public int CurrentSteps
        //    {
        //        get { return _currentSteps; }
        //        set
        //        {
        //            if (Settings.CurrentSteps != value)
        //            {
        //                Settings.CurrentSteps = value;
        //            }
        //            if (_currentSteps != value)
        //            {
        //                _currentSteps = value;
        //                OnPropertyChanged(nameof(CurrentSteps));
        //            }
        //        }
        //    }
        //    private int _stepBank;
        //    public int StepBank
        //    {
        //        get { return _stepBank; }
        //        set
        //        {
        //            if (Settings.StepBank != value)
        //            {
        //                Settings.StepBank = value;
        //            }
        //            if (_stepBank != value)
        //            {
        //                _stepBank = value;
        //                OnPropertyChanged(nameof(StepBank));
        //                OnPropertyChanged(nameof(StepsNeeded));
        //                OnPropertyChanged(nameof(QuestionReady));
        //            }
        //        }
        //    }
        //    private int _coins;
        //    public int Coins
        //    {
        //        get { return _coins; }
        //        set
        //        {
        //            if (Settings.Coins != value)
        //            {
        //                Settings.Coins = value;
        //            }
        //            if (_coins != value)
        //            {
        //                _coins = value;
        //                OnPropertyChanged(nameof(Coins));
        //            }
        //        }
        //    }
        //    private int _stars;
        //    public int Stars
        //    {
        //        get { return _stars; }
        //        set
        //        {
        //            if (Settings.Stars != value)
        //            {
        //                Settings.Stars = value;
        //            }
        //            if (_stars != value)
        //            {
        //                _stars = value;
        //                OnPropertyChanged(nameof(Stars));
        //            }
        //        }
        //    }
        //    private int _points;
        //    public int Points
        //    {
        //        get { return _points; }
        //        set
        //        {
        //            if (Settings.Points != value)
        //            {
        //                Settings.Points = value;
        //            }
        //            if (_points != value)
        //            {
        //                _points = value;
        //                OnPropertyChanged(nameof(Points));
        //            }
        //        }
        //    }

        //    public bool IsLoginActive
        //    {
        //        get { return (PlayerId > -1 && PlayerLevel > 0); }
        //    }
        //    public bool QuestionReady
        //    {
        //        get { return (StepsNeeded <= 0); }
        //    }
        //    public int StepsNeeded
        //    {
        //        get
        //        {
        //            return 1000 - StepBank;
        //        }
        //    }
        //}


    }

}