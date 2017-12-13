using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using TriviaTraverse.Models;
using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TriviaTraverse.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TutorialPage : ContentPage, IContentPage
    {
        private TutorialPageViewModel vm;

        public string Name
        {
            get
            {
                return "Tutorial";
            }
        }

        public TutorialPage ()
		{
			InitializeComponent ();

            vm = new TutorialPageViewModel(Navigation);
            this.BindingContext = vm;
        }

        #region "Properties"
        private int newAnimationCountdown;

        private bool UpdateBoard = true;

        private List<TutorialBoardClickArea> clickAreas = new List<TutorialBoardClickArea>();

        bool ObjectsCreated = false;
        int screenWidth = 0;
        int screenHeight = 0;
        float margin2 = 0;
        float margin4 = 0;
        float margin6 = 0;
        float margin8 = 0;
        float margin10 = 0;
        float margin12 = 0;

        float sectionRadius = 0;

        SKPaint blackSmallTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 46 };
        SKPaint inactiveSmallTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gray, TextSize = 46 };
        SKPaint blackLargeTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 80 };

        SKPaint inactiveStrokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Gray, StrokeWidth = 6, IsAntialias = true };
        SKPaint highlightStrokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.CornflowerBlue, StrokeWidth = 8, IsAntialias = true };

        SKPaint blackFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black };
        SKPaint completeFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.ForestGreen };
        SKPaint whiteFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.White };
        SKPaint highlightFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.CornflowerBlue };
        SKPaint inactiveFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gray };
        SKPaint yellowFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gold };

        SKPoint sectionTCenter;
        SKPath GameStagePath; SKPath starPath;

        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateBoard = true;
            newAnimationCountdown = 30; //1 second animation
            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                if (UpdateBoard)
                {
                    canvasView.InvalidateSurface();
                    if (newAnimationCountdown > 0) { newAnimationCountdown -= 1; }
                }
                return true;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            UpdateBoard = false;
        }

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            //setup global objects
            if (!ObjectsCreated)
            {
                screenWidth = e.Info.Width;
                screenHeight = e.Info.Height;

                //one pixel = .0005f;
                margin2 = screenHeight * .001f;
                margin4 = margin2 * 2;
                margin6 = margin2 * 3;
                margin8 = margin2 * 4;
                margin10 = margin2 * 5;
                margin12 = margin2 * 6;
                
                sectionRadius = screenHeight * .05f;

                inactiveStrokePaint.TextSize = margin4;
                highlightStrokePaint.TextSize = margin6;

                blackSmallTextPaint.TextSize = screenHeight * .0221f;
                inactiveSmallTextPaint.TextSize = blackSmallTextPaint.TextSize;
                blackLargeTextPaint.TextSize = screenHeight * .0385f;

                GameStagePath = new SKPath();
                GameStagePath.MoveTo(0, screenHeight * .0577f);
                GameStagePath.RLineTo(screenWidth, 0);

                List<SKPoint> starPoints = CalculateStarPoints(0, 0, 5, screenHeight * .01f, screenHeight * .005f);
                starPath = new SKPath();
                starPath.MoveTo(starPoints[0]);
                for (int i = 1; i < starPoints.Count - 1; i++)
                {
                    starPath.LineTo(starPoints[i]);
                }
                starPath.LineTo(starPoints.Last());
                starPath.Close();

                sectionTCenter = new SKPoint(screenWidth * .7f, screenHeight * .7f);

                ObjectsCreated = true;
            }
            clickAreas.Clear();

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            canvas.DrawTextOnPath("Tutorial", GameStagePath, 0, 0, blackLargeTextPaint);

            //draw section circles
            // section Tutorial
            DrawSectionCircle(canvas, 0, vm.ActiveStage==null?false:true, sectionTCenter);
        }

        private List<SKPoint> CalculateStarPoints(float centerX, float centerY, float arms, float outerRadius, float innerRadius)
        {
            List<SKPoint> results = new List<SKPoint>();

            var angle = Math.PI / arms;

            for (var i = 0; i < 2 * arms; i++)
            {
                // Use outer or inner radius depending on what iteration we are in.
                var r = (i & 1) == 0 ? outerRadius : innerRadius;

                var currX = centerX + Math.Cos(i * angle) * r;
                var currY = centerY + Math.Sin(i * angle) * r;

                results.Add(new SKPoint(float.Parse(currX.ToString()), float.Parse(currY.ToString())));
            }

            return results;
        }


        private void DrawSectionCircle(SKCanvas canvas, int sectionIdx, bool IsActive, SKPoint center)
        {
            //Debug.WriteLine("Draw section " + sectionIdx.ToString());
            GameSection section = null;
            float startAngle = 0;
            float sweepAngleBase = 360f / 5;
            float sweepAngle = 0;


            SKRect rect = new SKRect(center.X - sectionRadius, center.Y - sectionRadius, center.X + sectionRadius, center.Y + sectionRadius);
            if (IsActive)
            {
                section = vm.ActiveStage.Sections[sectionIdx];
                if (section.IsComplete)
                {
                    //draw complete circle
                    float sectionRadiusAni = sectionRadius;  //setup animation
                    if (section.NewlyComplete) { sectionRadiusAni -= (newAnimationCountdown * 2); }
                    //Debug.WriteLine(newAnimationCountdown.ToString() + "   " + sectionRadiusAni.ToString()); }

                    canvas.DrawCircle(center.X, center.Y, sectionRadiusAni, completeFillPaint);

                    //RegisterClickArea("SectionRetry", rect, sectionIdx.ToString());

                    if (section.NewlyComplete && newAnimationCountdown == 0) { section.NewlyComplete = false; }
                }
                else
                {
                    //draw pie wedges
                    canvas.DrawCircle(center.X, center.Y, sectionRadius, blackFillPaint);
                    if (section.NumberAnswered > 0)
                    {
                        sweepAngle = section.NumberAnswered * sweepAngleBase;
                        using (SKPath path = new SKPath())
                        {
                            path.MoveTo(center.X, center.Y);
                            path.ArcTo(rect, startAngle, sweepAngle, false);
                            path.Close();

                            canvas.DrawPath(path, highlightFillPaint);
                        }
                    }
                    RegisterClickArea("Section", rect, sectionIdx.ToString());
                }
            }
            else
            {
                //draw inactive circle
                canvas.DrawCircle(center.X, center.Y, sectionRadius, whiteFillPaint);
                canvas.DrawCircle(center.X, center.Y, sectionRadius, inactiveStrokePaint);

                RegisterClickArea("Start", rect, sectionIdx.ToString());
            }

            //draw text
            SKPoint textPoint = new SKPoint();
            SKPaint newPaint = inactiveSmallTextPaint.Clone();
            if (IsActive) { newPaint = blackSmallTextPaint.Clone(); }
            switch (sectionIdx)
            {
                case 0:
                    textPoint.X = center.X + sectionRadius - margin8;
                    textPoint.Y = center.Y + (sectionRadius / 2) + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 3:
                    textPoint.X = center.X + sectionRadius + margin12;
                    textPoint.Y = center.Y - margin8 + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 1:
                case 4:
                    textPoint.X = center.X - (sectionRadius * 2);
                    textPoint.Y = center.Y + sectionRadius + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 2:
                case 5:
                    textPoint.X = center.X - sectionRadius;
                    textPoint.Y = center.Y + sectionRadius + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 6:
                    textPoint.X = center.X + sectionRadius + margin8; ;
                    textPoint.Y = center.Y - sectionRadius + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
            }

            if (section != null)
            {
                //draw text
                using (SKPath path = new SKPath())
                {
                    path.MoveTo(textPoint.X, textPoint.Y);
                    path.LineTo(textPoint.X + (screenWidth * .2778f), textPoint.Y);

                    canvas.DrawTextOnPath(section.SectionName, path, 0, 0, newPaint);
                }
                //stars
                SKPoint starPoint = new SKPoint(textPoint.X, textPoint.Y + margin8);
                for (int i = 1; i <= 5; i++)
                {

                    canvas.Save();
                    canvas.Translate(starPoint.X + (margin10 * 2), starPoint.Y + (margin10 * 2));
                    canvas.DrawPath(starPath, section.NumberCorrect >= i ? yellowFillPaint : inactiveFillPaint);
                    canvas.Restore();
                    starPoint.X += starPath.TightBounds.Width + margin8;
                }
            }
        }

        private void RegisterClickArea(string name, SKPath area, string value = null)
        {
            SKPath newArea = new SKPath(area);
            clickAreas.Insert(0, new TutorialBoardClickArea() { Name = name, AreaPath = newArea, Value = value });
        }
        private void RegisterClickArea(string name, SKRect area, string value = null)
        {
            SKRect newArea = new SKRect(area.Left, area.Top, area.Right, area.Bottom);
            clickAreas.Insert(0, new TutorialBoardClickArea() { Name = name, AreaRect = newArea, Value = value });
        }

        private void canvasView_Touch(object sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                //SKPoint touchPoint = e.Location;
                for (int i = 0; i < clickAreas.Count; i++)
                {
                    bool clicked = false;
                    TutorialBoardClickArea area = clickAreas[i];
                    if (area.AreaPath != null)
                    {
                        clicked = clickAreas[i].AreaPath.Contains(e.Location.X, e.Location.Y);
                    }
                    else
                    {
                        clicked = clickAreas[i].AreaRect.Contains(e.Location.X, e.Location.Y);
                    }

                    if (clicked)
                    {
                        //Debug.WriteLine("Clicked: " + clickAreas[i].Name);
                        switch (clickAreas[i].Name)
                        {
                            case "Section":
                                vm.SelectSection(clickAreas[i].Value);
                                break;
                            case "Start":
                                vm.StartTutorial();
                                break;
                        }
                        break;
                    }
                }
            }
            e.Handled = true;
        }

    }

    public class TutorialBoardClickArea
    {
        public string Name { get; set; }
        public SKRect AreaRect { get; set; }
        public SKPath AreaPath { get; set; }
        public string Value { get; set; }

    }
}