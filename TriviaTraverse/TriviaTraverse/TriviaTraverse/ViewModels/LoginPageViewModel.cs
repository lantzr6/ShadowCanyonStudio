using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Facebook.Models;
using TriviaTraverse.Facebook.Objects;
using TriviaTraverse.Facebook.Services;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            ModeOptions = true;
        }

        #region Properties

        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private string[] permissions = new string[] {
            "public_profile",
            "email",
            "user_friends"
        };
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        private bool _modeOptions = false;
        public bool ModeOptions
        {
            get { return _modeOptions; }
            set
            {
                if (_modeOptions != value)
                {
                    _modeOptions = value;
                    RaisePropertyChanged(nameof(ModeOptions));
                }
            }
        }
        private bool _modeSignUp = false;
        public bool ModeSignUp
        {
            get { return _modeSignUp; }
            set
            {
                if (_modeSignUp != value)
                {
                    _modeSignUp = value;
                    RaisePropertyChanged(nameof(ModeSignUp));
                }
            }
        }
        private bool _modeLogIn = false;
        public bool ModeLogIn
        {
            get { return _modeLogIn; }
            set
            {
                if (_modeLogIn != value)
                {
                    _modeLogIn = value;
                    RaisePropertyChanged(nameof(ModeLogIn));
                }
            }
        }
        private bool _backVisible = false;
        public bool BackVisible
        {
            get { return _backVisible; }
            set
            {
                if (_backVisible != value)
                {
                    _backVisible = value;
                    RaisePropertyChanged(nameof(BackVisible));
                }
            }
        }
        private string _modeTitle = "";
        public string ModeTitle
        {
            get { return _modeTitle; }
            set
            {
                if (_modeTitle != value)
                {
                    _modeTitle = value;
                    RaisePropertyChanged(nameof(ModeTitle));
                }
            }
        }

        private bool _signUpEmail = false;
        public bool SignUpEmail
        {
            get { return _signUpEmail; }
            set
            {
                if (_signUpEmail != value)
                {
                    _signUpEmail = value;
                    RaisePropertyChanged(nameof(SignUpEmail));
                }
            }
        }
        private bool _signUpUserName = false;
        public bool SignUpUserName
        {
            get { return _signUpUserName; }
            set
            {
                if (_signUpUserName != value)
                {
                    _signUpUserName = value;
                    RaisePropertyChanged(nameof(SignUpUserName));
                }
            }
        }
        private bool _signUpFinal = false;
        public bool SignUpFinal
        {
            get { return _signUpFinal; }
            set
            {
                if (_signUpFinal != value)
                {
                    _signUpFinal = value;
                    RaisePropertyChanged(nameof(SignUpFinal));
                }
            }
        }



        private string _EmailAddr;
        public string EmailAddr
        {
            get { return _EmailAddr; }
            set
            {
                if (_EmailAddr != value)
                {
                    _EmailAddr = value.Trim();
                    RaisePropertyChanged(nameof(EmailAddr));
                    ((Command)SignUpCheckEmailCommand).ChangeCanExecute();
                    ((Command)SignInCommand).ChangeCanExecute();
                }
            }
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value.Trim();
                    RaisePropertyChanged(nameof(UserName));
                    ((Command)SignUpCheckUserNameCommand).ChangeCanExecute();
                }
            }
        }
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    RaisePropertyChanged(nameof(Password));
                    ((Command)SignUpCommand).ChangeCanExecute();
                    ((Command)SignInCommand).ChangeCanExecute();
                }
            }
        }

        private bool _loginFailed = false;
        public bool LoginFailed
        {
            get { return _loginFailed; }
            set
            {
                if (_loginFailed != value)
                {
                    _loginFailed = value;
                    RaisePropertyChanged(nameof(LoginFailed));
                }
            }
        }
        private bool _signUpFailed = false;
        public bool SignUpFailed
        {
            get { return _signUpFailed; }
            set
            {
                if (_signUpFailed != value)
                {
                    _signUpFailed = value;
                    RaisePropertyChanged(nameof(SignUpFailed));
                }
            }
        }
        private string _SignUpErrorMessage;
        public string SignUpErrorMessage
        {
            get { return _SignUpErrorMessage; }
            set
            {
                if (_SignUpErrorMessage != value)
                {
                    _SignUpErrorMessage = value;
                    RaisePropertyChanged(nameof(SignUpErrorMessage));
                }
            }
        }

        #endregion

        #region "Commands"

        private ICommand _continueGuestCommand;
        public ICommand ContinueGuestCommand =>
            _continueGuestCommand ?? (_continueGuestCommand = new Command(ContinueGuestCommandExecuted, ContinueGuestCommandCanExecute));
        private bool ContinueGuestCommandCanExecute()
        {
            return true;
        }
        private void ContinueGuestCommandExecuted()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (PlayerObj.PlayerId > 0)
                {
                    //updated username and sets PlayerLevel;
                    await UpdateAccount(PlayerObj.UserName.Replace("Temp", "Guest"), PlayerObj.EmailAddr, PlayerObj.Password);
                }
                else
                {
                    IsBusy = true;
                    BusyMessage = "Creating guest user account";

                    if (PlayerObj.PlayerId <= 0)
                    {
                        await CreateNewAccount("","","",1); // create guest account wiht level 1
                    }
                    IsBusy = false;
                }
                ((MasterPage)(App.Current.MainPage)).ShowHome();
            });
        }

        private async Task CreateNewAccount(string username = "", string email = "", string password = "", int level = 0, bool fbLogin = false)
        {
            App.CampaignObj = null;
            App.TutorialObj = null;

            Player inObj = new Player() { UserName = username, EmailAddr = email, Password = password, PlayerLevel = level, FbLogin = fbLogin };
            Player player = await WebApi.Instance.NewAccountAsync(inObj);
            PlayerObj.PlayerId = player.PlayerId;
            PlayerObj.UserName = player.UserName;
            PlayerObj.EmailAddr = player.EmailAddr;
            PlayerObj.Password = player.Password;
            PlayerObj.PlayerLevel = player.PlayerLevel;
            PlayerObj.LastStepUpdate = player.LastStepUpdate;
        }

        private ICommand _loginFacebookCommand;
        public ICommand LoginFacebookCommand =>
            _loginFacebookCommand ?? (_loginFacebookCommand = new Command(OnLoginFacebook, CanLoginFacebook));
        private bool CanLoginFacebook()
        {
            return true;
        }
        private void OnLoginFacebook()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                FbAccessToken token;
                try
                {
                    token = await DependencyService.Get<IFacebookLogin>()
                        .LogIn(permissions);

                    IsBusy = true;
                    BusyMessage = "Connecting with Facebook";

                    if (token != null)
                    {
                        // save token
                        await token.Save();

                        FBGraph(token);

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("token was null");
                    }
                }
                catch (TaskCanceledException e)
                {
                    System.Diagnostics.Debug.WriteLine("The task was canceled. " + e.Message);
                    //						throw e;
                }
                IsBusy = false;
            });
        }

        private ICommand _startSignupCommand;
        public ICommand StartSignUpCommand =>
            _startSignupCommand ?? (_startSignupCommand = new Command(StartSignUpCommandExecuted, StartSignUpCommandCanExecute));
        private bool StartSignUpCommandCanExecute()
        {
            return true;
        }
        private void StartSignUpCommandExecuted()
        {
            ModeOptions = false;
            ModeLogIn = false;
            ModeSignUp = true;
            BackVisible = true;
            SignUpEmail = true;
            SignUpUserName = false;
            SignUpFinal = false;
            SignUpFailed = false;
            SignUpErrorMessage = "";
            ModeTitle = "Sign Up";
        }

        private ICommand _signUpCheckEmailCommand;
        public ICommand SignUpCheckEmailCommand =>
            _signUpCheckEmailCommand ?? (_signUpCheckEmailCommand = new Command(SignUpCheckEmailCommandExecuted, SignUpCheckEmailCommandCanExecute));
        private bool SignUpCheckEmailCommandCanExecute()
        {
            if (EMailIsValid(EmailAddr))
            {
                return true;
            }
            else { return false; }
        }
        private async void SignUpCheckEmailCommandExecuted()
        {
            SignUpFailed = false;
            SignUpErrorMessage = "";
            //check
            bool result = await WebApi.Instance.CheckEmailExists(EmailAddr);
            if (!result)
            {
                SignUpEmail = false;
                SignUpUserName = true;
                SignUpFinal = false;
            }
            else
            {
                SignUpFailed = true;
                SignUpErrorMessage = "email already in use";
            }
        }

        private ICommand _signUpCheckUserNameCommand;
        public ICommand SignUpCheckUserNameCommand =>
            _signUpCheckUserNameCommand ?? (_signUpCheckUserNameCommand = new Command(SignUpCheckUserNameCommandExecuted, SignUpCheckUserNameCommandCanExecute));
        private bool SignUpCheckUserNameCommandCanExecute()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                return true;
            }
            else { return false; }
        }
        private async void SignUpCheckUserNameCommandExecuted()
        {
            SignUpFailed = false;
            SignUpErrorMessage = "";
            //check
            bool result = await WebApi.Instance.CheckUserNameExists(UserName);
            if (!result)
            {
                SignUpEmail = false;
                SignUpUserName = false;
                SignUpFinal = true;
            }
            else
            {
                SignUpFailed = true;
                SignUpErrorMessage = "username already in use";
            }
        }

        private ICommand _signupCommand;
        public ICommand SignUpCommand =>
            _signupCommand ?? (_signupCommand = new Command(SignUpCommandExecuted, SignUpCommandCanExecute));

        private bool SignUpCommandCanExecute()
        {
            if ((!string.IsNullOrEmpty(Password) && Password.Length >= 6))
            {
                return true;
            }
            else { return false; }
        }

        private async void SignUpCommandExecuted()
        {
            if (PlayerObj.PlayerId > 0)  //if temp account already created - update
            {
                await UpdateAccount(UserName, EmailAddr, Password);
            }
            else
            {
                await CreateNewAccount(UserName, EmailAddr, Password, 1);
            }
            ((MasterPage)(App.Current.MainPage)).ShowHome();
        }


        private ICommand _startLoginCommand;
        public ICommand StartLoginCommand =>
            _startLoginCommand ?? (_startLoginCommand = new Command(StartLogin, StartCanLogin));
        private bool StartCanLogin()
        {
            return true;
        }
        private void StartLogin()
        {
            ModeOptions = false;
            ModeLogIn = true;
            ModeSignUp = false;
            BackVisible = true;
            ModeTitle = "Log In";
        }

        private ICommand _backCommand;
        public ICommand BackCommand =>
            _backCommand ?? (_backCommand = new Command(OnBack, CanBack));
        private bool CanBack()
        {
            return true;
        }
        private void OnBack()
        {
            ModeOptions = true;
            ModeLogIn = false;
            ModeSignUp = false;
            BackVisible = false;
            SignUpEmail = false;
            SignUpUserName = false;
            SignUpFinal = false;
            SignUpFailed = false;
            SignUpErrorMessage = "";
            ModeTitle = "";
        }

        private ICommand _signinCommand;
        public ICommand SignInCommand =>
            _signinCommand ?? (_signinCommand = new Command(SignInCommandExecuted, SignInCommandCanExecute));

        private bool SignInCommandCanExecute()
        {
            if (EMailIsValid(EmailAddr) && (!string.IsNullOrEmpty(Password) && Password.Length >= 6))
            {
                return true;
            }
            else { return false; }
        }

        private void SignInCommandExecuted()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;

                LoginFailed = false;

                Player data = new Player();

                data = await WebApi.Instance.GetAccountAsync(EmailAddr, Password);

                if (data != null && data.PlayerId > 0)
                {
                    PlayerObj.PlayerId = data.PlayerId;
                    PlayerObj.UserName = data.UserName;
                    PlayerObj.EmailAddr = data.EmailAddr;
                    PlayerObj.Password = data.Password;
                    PlayerObj.PlayerLevel = data.PlayerLevel;
                    PlayerObj.CurrentSteps = data.CurrentSteps;
                    PlayerObj.StepBank = data.StepBank;
                    PlayerObj.Coins = data.Coins;
                    PlayerObj.Stars = data.Stars;
                    PlayerObj.Points = data.Points;

                    ((MasterPage)(App.Current.MainPage)).ShowHome();
                }
                else
                {
                    LoginFailed = true;
                }

                IsBusy = false;
            });
        }

        public bool EMailIsValid(string emailaddress)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\.\-]+)((\.(\w){2,3})+)$");

                if (!string.IsNullOrEmpty(emailaddress) && regex.IsMatch(emailaddress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        private async Task UpdateAccount(string username, string email, string password)
        {
            if (PlayerObj.LastStepUpdate == DateTime.MinValue) { PlayerObj.LastStepUpdate = DateTime.Now; }
            PlayerObj.UserName = username;
            PlayerObj.EmailAddr = email;
            PlayerObj.Password = password;
            PlayerObj.PlayerLevel = 1;

            await App._authenticationService.UpdateAccountAsync();
        }

        private async void FBGraph(FbAccessToken token)
        {
            parameters.Add("fields", "first_name, last_name, name, email");
            IGraphRequest request = DependencyService.Get<IGraphRequest>()
                .NewRequest(token, "/me", parameters);

            IGraphResponse response = await request.ExecuteAsync();
            System.Diagnostics.Debug.WriteLine(response.RawResponse);

            if (response != null && response.RawResponse != null)
            {
                Dictionary<string, string> deserialized = JsonConvert
                .DeserializeObject<Dictionary<string, string>>(response.RawResponse);
                FacebookProfile profile = new FacebookProfile(deserialized["first_name"], deserialized["last_name"], deserialized["name"], deserialized["email"], deserialized["id"]);

                string fbUserName = profile.FirstName + (profile.LastName.Length > 0 ? profile.LastName.First().ToString() : "");
                string fbEmailAddr = profile.Email;
                string fbPassword = "facebook";

                //check for existing account linked to FB
                Player fbPlayer = new Player();
                fbPlayer = await WebApi.Instance.GetAccountAsync(fbEmailAddr, fbPassword);
                if (fbPlayer == null)
                {
                    if (PlayerObj.PlayerId > 0)  //if temp account already created - update
                    {
                        PlayerObj.FbLogin = true;
                        await UpdateAccount(fbUserName, fbEmailAddr, fbPassword);
                    }
                    else
                    {
                        await CreateNewAccount(fbUserName, fbEmailAddr, fbPassword, 1, true);
                    }
                }
                else
                {
                    App.PlayerObj = fbPlayer;
                }
                ((MasterPage)(App.Current.MainPage)).ShowHome();
            }
            else
            {
                await token.Clear();
                parameters.Clear();
            }
        }


    }
}
