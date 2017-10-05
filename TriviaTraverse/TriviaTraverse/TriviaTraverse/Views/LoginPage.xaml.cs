using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage, IContentPage
    {
        private LoginPageViewModel vm;

        public string Name
        {
            get
            {
                return "Login";
            }
        }

        public LoginPage()
        {
            InitializeComponent();

            vm = new LoginPageViewModel(Navigation);
            this.BindingContext = vm;
        }
    }
}
