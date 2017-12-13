using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;

namespace TriviaTraverse.ViewModels
{
    class GameJoinPageViewModel : ViewModelBase
    {
        public Task Initialization { get; private set; }

        public GameJoinPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            Initialization = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // Asynchronously initialize this instance.
            IsBusy = true;

            IsBusy = false;
        }

        #region Properties

        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }

        private string _vGameType;
        public string VGameType
        {
            get { return _vGameType; }
            set
            {
                if (_vGameType != value)
                {
                    _vGameType = value;
                    ((Command)StartGameCommand).ChangeCanExecute();
                }
            }
        }

        private string _vGameStepCap;
        public string VGameStepCap
        {
            get { return _vGameStepCap; }
            set
            {
                if (_vGameStepCap != value)
                {
                    _vGameStepCap = value;
                    ((Command)StartGameCommand).ChangeCanExecute();
                }
            }
        }

        private bool _vGameAuto;
        public bool VGameAuto
        {
            get { return _vGameAuto; }
            set
            {
                if (_vGameAuto != value)
                {
                    _vGameAuto = value;
                    ((Command)StartGameCommand).ChangeCanExecute();
                }
            }
        }

        #endregion

        #region Commands
        private ICommand _closeCommand;
        public ICommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new Command(OnClose, CanClose));
        private bool CanClose()
        {
            return true;
        }
        private async void OnClose()
        {
            await Navigation.PopModalAsync();
        }

        private ICommand _startGameCommand;
        public ICommand StartGameCommand =>
            _startGameCommand ?? (_startGameCommand = new Command(OnStartGame, CanStartGame));
        private bool CanStartGame()
        {
            return (VGameType != null && VGameStepCap != null);
        }
        private async void OnStartGame()
        {
            VGame vGame = await WebApi.Instance.JoinNewGame(PlayerObj.PlayerId,VGameType,VGameStepCap,VGameAuto);
            ((MasterPage)(App.Current.MainPage)).ShowHome();
        }

        #endregion

    }

}
