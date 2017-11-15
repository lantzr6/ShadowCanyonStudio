using TriviaTraverse.ViewModels;
using Xamarin.Forms;

namespace TriviaTraverse.Views
{
    public partial class SignUpDataPage : ContentPage
    {
        private SignUpDataPageViewModel vm;


        public SignUpDataPage()
        {
            InitializeComponent();

            vm = new SignUpDataPageViewModel(Navigation);
            this.BindingContext = vm;
        }

    }
}

