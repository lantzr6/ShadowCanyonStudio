using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampaignPage : ContentPage, IContentPage
    {
        private CampaignPageViewModel vm;

        public string Name
        {
            get
            {
                return "Campaign";
            }
        }

        public CampaignPage()
        {
            InitializeComponent();

            vm = new CampaignPageViewModel(Navigation);
            this.BindingContext = vm;
        }
    }
}
