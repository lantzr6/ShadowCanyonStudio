using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TriviaTraverse.Services;
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

