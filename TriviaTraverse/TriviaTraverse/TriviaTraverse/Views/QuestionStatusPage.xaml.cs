using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionStatusPage : ContentPage, IContentPage
    {
        private QuestionStatusPageViewModel vm;

        public string Name
        {
            get
            {
                return "Dashboard";
            }
        }

        public QuestionStatusPage()
        {
            InitializeComponent();

            vm = new QuestionStatusPageViewModel(Navigation);
            this.BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            vm.PageSetup();

            base.OnAppearing();
        }

    }
    
}
