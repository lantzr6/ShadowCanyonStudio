using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Diagnostics;
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

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });

        }

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (vm.Countdown.TimerActive)
            {
                SKSurface surface = e.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                int screenWidth = e.Info.Width;
                int screenHeight = e.Info.Height;

                var t = vm.Countdown.TotalTime;
                var r = vm.Countdown.RemainTime;
                Debug.WriteLine(r.ToString());

                var p = r / t;

                int r1 = 102;
                int r2 = 134;
                int rd = r2 - r1;
                int g1 = 51;
                int g2 = 0;
                int gd = g2 - g1;
                int b1 = 204;
                int b2 = 0;
                int bd = b2 - b1;

                int R = int.Parse(Math.Round(r2 - (rd * p), 0).ToString());
                int G = int.Parse(Math.Round(g2 - (gd * p), 0).ToString());
                int B = int.Parse(Math.Round(b2 - (bd * p), 0).ToString());

                Color c = Color.FromRgb(R, G, B);
                SKColor color = c.ToSKColor();

                SKRect rect = new SKRect(0, 0, float.Parse(screenWidth.ToString()), float.Parse((screenHeight - (screenHeight * p)).ToString()));
                canvas.DrawRect(rect, new SKPaint() { Style = SKPaintStyle.Fill, Color = color });
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            return true;  //prevent hardware back button
        }

    }
}
