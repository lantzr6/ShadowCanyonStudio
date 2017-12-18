using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TriviaTraverse.Api;
using TriviaTraverse.Controls;
using TriviaTraverse.Helpers;
using TriviaTraverse.Models;
using TriviaTraverse.Views;
using Xamarin.Forms;
using static TriviaTraverse.Helpers.Settings;

namespace TriviaTraverse.ViewModels
{
    class GameBoardPageViewModel : ViewModelBase
    {
        public Task Initialization { get; private set; }

        public GameBoardPageViewModel(INavigation _navigation, int vGameId)
        {
            Navigation = _navigation;

            Initialization = InitializeAsync(vGameId);
        }

        private async Task InitializeAsync(int vGameId)
        {
            // Asynchronously initialize this instance.
            IsBusy = true;
            VGameObj = await LoadVGame(vGameId);
            App.GameMode = GameMode.VGame;
            if (VGameObj.Sections.Count == 0)
            {
                WrappedItems = VGameObj.CategoryQueue.Select(item => new WrappedSelection<object>() { Item = item, IsSelected = false }).ToList();
                foreach (var wi in WrappedItems)
                {
                    wi.PropertyChanged += (sender, e) => { CategoriesSelected(); };
                }
                SelectCategoryVisible = true;
            }
            IsBusy = false;
        }

        private async Task<VGame> LoadVGame(int vGameId)
        {
            VGame vGame = await WebApi.Instance.GetGame(vGameId, PlayerObj.PlayerId);

            return vGame;
        }

        #region "Properties"

        public Player PlayerObj
        {
            get { return App.PlayerObj; }
        }
        public VGame VGameObj
        {
            get { return App.VGameObj; }
            set
            {
                App.VGameObj = value;
                RaisePropertyChanged(nameof(VGameObj));
            }
        }
        public TutorialMessagesStatus TutorialObj
        {
            get { return App.TutorialObj; }
            set
            {
                App.TutorialObj = value;
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
            set
            {
                if (_selectCategoryVisible != value)
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

        //private string _viewState;
        //public string ViewState
        //{
        //    get { return _viewState; }
        //    set
        //    {
        //        if (_viewState != value)
        //        {
        //            _viewState = value;
        //            RaisePropertyChanged(nameof(ViewState));
        //        }
        //    }
        //}

        private List<WrappedSelection<object>> _wrapperItems;
        public List<WrappedSelection<object>> WrappedItems
        {
            get { return _wrapperItems; }
            set
            {
                _wrapperItems = value;
                RaisePropertyChanged(nameof(WrappedItems));
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

        #endregion

        public async void SelectSection(string parm)
        {
            IsBusy = true;
            int obj = int.Parse(parm);

            App.ActiveSection = VGameObj.Sections.Where(o=>o.SectionOrder == obj).FirstOrDefault();
            if (!App.ActiveSection.IsComplete)
            {
                GameSection returnSection = VGameObj.Sections[obj];
                await Navigation.PushModalAsync(new QuestionStatusPage());
            }
            IsBusy = false;
        }

        //public async void SectionNext()
        //{
        //    IsBusy = true;
        //    //if (VGameObj.StartTime >= DateTime.Now)
        //    //{
        //    if (VGameObj.CategoryQueue.Count() == 0)
        //    {
        //        VGameObj = await WebApi.Instance.StartGame(VGameObj.VGameId, PlayerObj.PlayerId);
        //    }

        //    WrappedItems = VGameObj.CategoryQueue.Select(item => new WrappedSelection<object>() { Item = item, IsSelected = false }).ToList();
        //    foreach (var wi in WrappedItems)
        //    {
        //        wi.PropertyChanged += (sender, e) => { CategoriesSelected(); };
        //    }

        //    SelectCategoryVisible = true;
        //    //}
        //    IsBusy = false;
        //}

        public async void CategoriesSelected()
        {
            int selectedCnt = 0;
            foreach (WrappedSelection<object> wi in WrappedItems)
            {
                if (wi.IsSelected)
                {
                    selectedCnt++;
                    wi.PropertyChanged -= (sender, e) => { CategoriesSelected(); };
                }
            }
            if ((VGameObj.GameStepCap == 15000 && selectedCnt == 4) || (VGameObj.GameStepCap < 15000 && selectedCnt == 3))
            {
                WrappedItems.Shuffle();
                IsBusy = true;
                VGameSelectedCategories inObj = new VGameSelectedCategories();
                inObj.PlayerId = PlayerObj.PlayerId;
                inObj.VGameId = VGameObj.VGameId;
                int order = 1;
                for (int i = 1; i <= WrappedItems.Count(); i++)
                {
                    if (WrappedItems[i-1].IsSelected)
                    {
                        VGameCategory vcat = WrappedItems[i-1].Item as VGameCategory;
                        if (vcat != null)
                        {
                            //vcat.IsUsed = true;
                            GameSection gs = new GameSection(vcat, order);
                            VGameObj.Sections.Add(gs);
                        }
                        inObj.SelectedCategories.Add(vcat);
                        ++order;
                    }
                }
                await WebApi.Instance.PostSelectedCategories(inObj);
                SelectCategoryVisible = false;
                IsBusy = false;
            }
            
        }

        public async Task<GameSection> GetSection(int sectionId, int playerId, int playerCampaignId, bool retry = false)
        {
            GameSection section;
            section = await WebApi.Instance.GetSectionAsync(sectionId, playerId, playerCampaignId, retry);
            return section;
        }

        //public async void ShowStats()
        //{
        //    IsBusy = true;
        //    VGameObj = await LoadVGame(VGameObj.VGameId);
        //    ViewState = "Stats";
        //    IsBusy = false;
        //}

        //public async void ShowSelf()
        //{
        //    IsBusy = true;
        //    VGameObj = await LoadVGame(VGameObj.VGameId);
        //    ViewState = "Self";
        //    IsBusy = false;
        //}


    }
}
