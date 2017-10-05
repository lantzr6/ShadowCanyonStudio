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

        }
    }
}
