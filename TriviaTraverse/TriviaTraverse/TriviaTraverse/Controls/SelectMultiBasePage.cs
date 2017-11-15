using System;

using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace TriviaTraverse.Controls
{
    public class WrappedSelection<T> : INotifyPropertyChanged
        {
            public T Item { get; set; }
            bool isSelected = false;
            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    if (isSelected != value)
                    {
                        isSelected = value;
                        PropertyChanged (this, new PropertyChangedEventArgs (nameof (IsSelected)));
                    }
                }
            }
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
        }
        public class WrappedItemSelectionTemplate : ViewCell
        {
            public WrappedItemSelectionTemplate() : base()
            {
                Label name = new Label();
                name.SetBinding(Label.TextProperty, new Binding("Item.CategoryName"));
                Switch mainSwitch = new Switch();
                mainSwitch.SetBinding(Switch.IsToggledProperty, new Binding("IsSelected"));
                RelativeLayout layout = new RelativeLayout();
                layout.Children.Add(name,
                    Constraint.Constant(5),
                    Constraint.Constant(5),
                    Constraint.RelativeToParent(p => p.Width - 60),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );
                layout.Children.Add(mainSwitch,
                    Constraint.RelativeToParent(p => p.Width - 55),
                    Constraint.Constant(5),
                    Constraint.Constant(50),
                    Constraint.RelativeToParent(p => p.Height - 10)
                );
                View = layout;
            }
        }
 
}
