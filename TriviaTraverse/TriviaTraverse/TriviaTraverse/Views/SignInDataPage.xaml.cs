using TriviaTraverse.ViewModels;
using Xamarin.Forms;

namespace TriviaTraverse.Views
{
    public partial class SignInDataPage : ContentPage
    {
        private SignInDataPageViewModel vm;


        public SignInDataPage()
        {
            InitializeComponent();

            vm = new SignInDataPageViewModel(Navigation);
            this.BindingContext = vm;
        }

    }
}

