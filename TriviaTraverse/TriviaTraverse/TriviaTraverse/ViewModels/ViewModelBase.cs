
using System.ComponentModel;
using TriviaTraverse.Models;
using Xamarin.Forms;

namespace TriviaTraverse.ViewModels
{
    public class ViewModelBase : IBindableBase
    {
        public INavigation Navigation;

        public ViewModelBase()
        {
        }

        public ViewModelBase(INavigation _navigation)
        {
            IsBusy = false;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value != _isBusy)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                    OnPropertyChanged(nameof(IsNotBusy));
                }
            }
        }

        public bool IsNotBusy
        {
            get { return !IsBusy; }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

    }
}
