using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                return "Tutorial";
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