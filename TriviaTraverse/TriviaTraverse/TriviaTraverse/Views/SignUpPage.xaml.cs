using TriviaTraverse.ViewModels;
using Xamarin.Forms;

namespace TriviaTraverse.Views
{
    public partial class SignUpPage : ContentPage
    {
        private SignUpPageViewModel vm;


        public SignUpPage(SignUpPageMode pageMode)
        {
            InitializeComponent();

            vm = new SignUpPageViewModel(Navigation, pageMode);
            this.BindingContext = vm;
        }

    }
}

