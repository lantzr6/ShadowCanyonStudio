using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GameJoinPage : ContentPage, IContentPage
    {
        private GameJoinPageViewModel vm;

        public string Name
        {
            get
            {
                return "Game";
            }
        }

        public GameJoinPage ()
        {
            InitializeComponent();
            
            vm = new GameJoinPageViewModel(Navigation);
            this.BindingContext = vm;
        }


        void OnBtnPublicClicked(object sender, EventArgs e)
        {
            btnPublic.BackgroundColor = Color.Aqua;
            btnPrivate.BackgroundColor = Color.Gray;
            vm.VGameType = "Public";
        }
        void OnBtnPrivateClicked(object sender, EventArgs e)
        {
            btnPublic.BackgroundColor = Color.Gray;
            btnPrivate.BackgroundColor = Color.Aqua;
            vm.VGameType = "Private";
        }
        void OnBtn5KClicked(object sender, EventArgs e)
        {
            btn5K.BackgroundColor = Color.Aqua;
            btn8K.BackgroundColor = Color.Gray;
            btn10K.BackgroundColor = Color.Gray;
            btn15K.BackgroundColor = Color.Gray;
            vm.VGameStepCap = "5K";
        }
        void OnBtn8KClicked(object sender, EventArgs e)
        {
            btn5K.BackgroundColor = Color.Gray;
            btn8K.BackgroundColor = Color.Aqua;
            btn10K.BackgroundColor = Color.Gray;
            btn15K.BackgroundColor = Color.Gray;
            vm.VGameStepCap = "8K";
        }
        void OnBtn10KClicked(object sender, EventArgs e)
        {
            btn5K.BackgroundColor = Color.Gray;
            btn8K.BackgroundColor = Color.Gray;
            btn10K.BackgroundColor = Color.Aqua;
            btn15K.BackgroundColor = Color.Gray;
            vm.VGameStepCap = "10K";
        }
        void OnBtn15KClicked(object sender, EventArgs e)
        {
            btn5K.BackgroundColor = Color.Gray;
            btn8K.BackgroundColor = Color.Gray;
            btn10K.BackgroundColor = Color.Gray;
            btn15K.BackgroundColor = Color.Aqua;
            vm.VGameStepCap = "15K";
        }
        void OnSwitchAutoToggled(object sender, ToggledEventArgs e)
        {
            vm.VGameAuto = switchAuto.IsToggled;
        }

    }
}