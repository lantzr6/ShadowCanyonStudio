using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionPage : ContentPage, IContentPage
    {
        private QuestionPageViewModel vm;

        public string Name
        {
            get
            {
                return "Question";
            }
        }

        public QuestionPage()
        {
            InitializeComponent();

            vm = new QuestionPageViewModel(Navigation);
            this.BindingContext = vm;
        }
    }
}
