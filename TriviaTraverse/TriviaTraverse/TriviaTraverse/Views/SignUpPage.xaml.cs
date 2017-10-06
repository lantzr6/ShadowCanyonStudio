using TriviaTraverse.ViewModels;
using Xamarin.Forms;

namespace TriviaTraverse.Views
{
    public partial class SignUpPage : ContentPage
    {
        private SignUpPageViewModel vm;


        public SignUpPage()
        {
            InitializeComponent();

            vm = new SignUpPageViewModel(Navigation);
            this.BindingContext = vm;
        }

    }
}

