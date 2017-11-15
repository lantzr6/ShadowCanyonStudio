using TriviaTraverse.ViewModels;
using Xamarin.Forms;

namespace TriviaTraverse.Views
{
    public partial class MasterPage : MasterDetailPage
    {

        private MasterPageViewModel vm;

        public MasterPage()
        {
            InitializeComponent();

            vm = new MasterPageViewModel(Navigation);
            this.BindingContext = vm;
        }

        public void ShowHome()
        {
            vm.ShowHome();
        }

    }
}