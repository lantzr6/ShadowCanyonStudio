using System;
using System.Collections.Generic;
using TriviaTraverse.Models;
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

            lstVGames.ItemSelected += VGameSelected;

            //MessagingCenter.Subscribe<MasterPage, List<StepData>>(this, "Steps", (sender, arg) => {
            //    Device.BeginInvokeOnMainThread(() => {
            //        //foreach(StepData i in (List<StepData>)arg)
            //        //{
            //            //DisplayAlert("New Steps " + i.VGameId.ToString(), i.Steps.ToString(), "Ok");
                        
            //       // }
            //        vm.UpdateSteps((List<StepData>)arg);
            //    });
            //});

        }

        private void VGameSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null) { vm.PlayVGame(e.SelectedItem); }
            lstVGames.SelectedItem = null;
        }

        public void LoadDashboard()
        {
            //lstCampaigns.SelectedItem = null;
            lstVGames.SelectedItem = null;

            vm.LoadDashboardGames();
        }

        public void Clear()
        {
            vm.ClearDashboard();
        }
    }
}
