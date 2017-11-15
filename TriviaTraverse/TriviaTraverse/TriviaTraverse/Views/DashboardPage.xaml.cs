using System;
using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage, IContentPage
    {
        private DashboardPageViewModel vm;

        public string Name
        {
            get
            {
                return "Dashboard";
            }
        }

        public DashboardPage()
        {
            InitializeComponent();

            vm = new DashboardPageViewModel(Navigation);
            this.BindingContext = vm;

            lstCampaigns.ItemSelected += CampaignSelected;
            lstVGames.ItemSelected += VGameSelected;

        }

        private void VGameSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null) { vm.PlayVGame(e.SelectedItem); }
            lstVGames.SelectedItem = null;
        }

        private void CampaignSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null) { vm.PlayCampaign(e.SelectedItem); }
            lstCampaigns.SelectedItem = null;
        }

        public void LoadDashboard()
        {
            lstCampaigns.SelectedItem = null;
            lstVGames.SelectedItem = null;

            vm.LoadDashboardGames();
        }

        public void Clear()
        {
            vm.ClearDashboard();
        }
    }
}
