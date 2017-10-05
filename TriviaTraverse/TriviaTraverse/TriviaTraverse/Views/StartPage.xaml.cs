using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage, IContentPage
    {
        private StartPageViewModel vm;

        public string Name
        {
            get
            {
                return "Start";
            }
        }


        public StartPage()
        {
            InitializeComponent();

            vm = new StartPageViewModel(Navigation);
            this.BindingContext = vm;

        }
    }
}
