using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using TriviaTraverse.Controls;
using TriviaTraverse.Droid.Utils;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace TriviaTraverse.Droid.Utils
{
    public class FacebookLoginButtonRenderer : ButtonRenderer
    {
        public Xamarin.Forms.Button formsButton;

        public FacebookLoginButtonRenderer()
            : base()
        { 
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                formsButton = e.NewElement;
            }
            if (this.Control != null)
            {
                global::Android.Widget.Button button = this.Control;
                button.Click += button_Click;
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            
            formsButton.Navigation.PopModalAsync();
        }
    }
}