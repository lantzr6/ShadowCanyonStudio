using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaTraverse.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Reflection;
using System.IO;
using TriviaTraverse.Models;
using System.Diagnostics;

namespace TriviaTraverse.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampaignBoardPage : ContentPage, IContentPage
    {
        private CampaignBoardPageViewModel vm;

        public string Name
        {
            get
            {
                return "Campaign";
            }
        }




        public CampaignBoardPage()
        {
            InitializeComponent();

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream2 = assembly.GetManifestResourceStream("TriviaTraverse.images.unlocked_lefthalf.png");

            //var rrr = assembly.GetManifestResourceNames();

            List<SKPoint> starPoints = CalculateStarPoints(0, 0, 5, 20, 10);
            starPath.MoveTo(starPoints[0]);
            for (int i = 1; i < starPoints.Count - 1; i++)
            {
                starPath.LineTo(starPoints[i]);
            }
            starPath.LineTo(starPoints.Last());
            starPath.Close();


            // Create Stage Next UnLocked Bitmap
            using (Stream stream = assembly.GetManifestResourceStream("TriviaTraverse.images.unlocked_lefthalf.png"))
            using (SKManagedStream skStream = new SKManagedStream(stream))
            {
                stageNextUnlocked = SKBitmap.Decode(skStream);
            }
            // Create Stage Next Locked Bitmap
            using (Stream stream = assembly.GetManifestResourceStream("TriviaTraverse.images.locked_lefthalf.png"))
            using (SKManagedStream skStream = new SKManagedStream(stream))
            {
                stageNextLocked = SKBitmap.Decode(skStream);
            }
            // Create Stage Previous UnLocked Bitmap
            using (Stream stream = assembly.GetManifestResourceStream("TriviaTraverse.images.unlocked_righthalf.png"))
            using (SKManagedStream skStream = new SKManagedStream(stream))
            {
                stagePreviousUnlocked = SKBitmap.Decode(skStream);
            }
            //// Create Stage Previous Locked Bitmap
            //using (Stream stream = assembly.GetManifestResourceStream("TriviaTraverse.images.locked_righthalf.png"))
            //using (SKManagedStream skStream = new SKManagedStream(stream))
            //{
            //    stagePreviousLocked = SKBitmap.Decode(skStream);
            //}



            vm = new CampaignBoardPageViewModel(Navigation);
            this.BindingContext = vm;


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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (vm.TutorialObj.CampaignSectionNewlyComplete == false && vm.ActiveStage != null && (from ee in vm.ActiveStage.Sections where ee.NewlyComplete == true select ee).Any())
            {
                vm.TutorialSectionNewlyCompleteIsVisible = true;
            }
            if (vm.TutorialObj.CampaignStageNewlyUnlocked == false && vm.ActiveStage != null && vm.ActiveStage.IsUnLocked)
            {
                vm.TutorialStageNewlyUnlockedIsVisible = true;
            }
            if (vm.TutorialObj.CampaignStageBonus == false && vm.ActiveStage != null && (from ee in vm.ActiveStage.Sections where ee.IsComplete select ee).Count() > 1)
            {
                vm.TutorialStageBonusIsVisible = true;
            }
            UpdateBoard = true;
            newAnimationCountdown = 30; //1 second animation
            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                if (UpdateBoard)
                {
                    canvasView.InvalidateSurface();
                    if (newAnimationCountdown > 0) { newAnimationCountdown -= 1; Debug.WriteLine(newAnimationCountdown.ToString()); }
                    //Debug.WriteLine("Campaign timer");
                }
                return true;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            UpdateBoard = false;
        }



        SKPaint blackThinStrokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Black, StrokeWidth = 1, IsAntialias = true };
        SKPaint blackStrokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Black, StrokeWidth = 6, IsAntialias = true };
        SKPaint inactiveStrokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.Gray, StrokeWidth = 6, IsAntialias = true };
        SKPaint highlightStrokePaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.CornflowerBlue, StrokeWidth = 8, IsAntialias = true };

        SKPaint blackFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black };
        SKPaint completeFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.ForestGreen };
        SKPaint whiteFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.White };




        #region "Properties"
        private int newAnimationCountdown;

        private bool UpdateBoard = true;

        SKBitmap stageNextUnlocked = new SKBitmap { };
        SKBitmap stageNextLocked = new SKBitmap { };
        SKBitmap stagePreviousUnlocked = new SKBitmap { };
        SKBitmap stagePreviousLocked = new SKBitmap { };
        SKBitmap starImage = new SKBitmap { };

        bool ObjectsCreated = false;
        int screenWidth = 0;
        int screenHeight = 0;
        float margin2 = 0;
        float margin4 = 0;
        //float margin6 = 0;
        float margin8 = 0;
        float margin10 = 0;
        float riseX = 0;
        float riseY = 0;
        float fallX = 0;
        float fallY = 0;
        float sectionRiseY = 0;
        float mntWidth = 0;
        float mntHeight = 0;

        SKPaint blackSmallTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 46 };
        SKPaint inactiveSmallTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gray, TextSize = 46 };
        SKPaint blackLargeTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 80 };
        SKPaint playerNameTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 60, TextAlign = SKTextAlign.Center };

        SKPaint highlightFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.CornflowerBlue };
        SKPaint stepFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Brown.WithAlpha(127) };
        SKPaint questionFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Fuchsia.WithAlpha(127) };
        SKPaint inactiveFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gray };
        SKPaint yellowFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gold };
        SKPaint paintGraph = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.LightGreen, StrokeWidth = 8, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round };
        SKPaint paintLines = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.LightGray, StrokeWidth = 6, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round };
        SKPaint paintMntOutline = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.DarkGray, StrokeWidth = 4, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round };
        SKPath GameNamePath; SKPath ScorePath; SKPath starPath;
        SKRect statsRect; SKRect backRect;
        SKPath graphPath; SKPath graphLinePath;
        #endregion

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
                //margin6 = margin2 * 3;
                margin8 = margin2 * 4;
                margin10 = margin2 * 5;

                blackSmallTextPaint.TextSize = screenHeight * .0221f;
                inactiveSmallTextPaint.TextSize = blackSmallTextPaint.TextSize;
                blackLargeTextPaint.TextSize = screenHeight * .0385f;

            }
            
            clickAreas.Clear();

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (vm.CampaignObj != null && vm.ActiveStage != null)
            {
                // Set transforms
                canvas.Translate(screenWidth / 2, screenHeight / 2);
                //canvas.Scale(Math.Min(width / 210f, height / 520f));

                int trueZeroX = -(screenWidth / 2);
                int trueZeroY = -(screenHeight / 2);
                int trueEndX = (screenWidth / 2);
                int trueEndY = (screenHeight / 2);
                int bottomBorder = 12;

                using (SKPath path = new SKPath())
                {
                    path.MoveTo(trueZeroX, trueZeroY+120);
                    path.LineTo(trueEndX, trueZeroY+120);

                    //canvas.DrawPath(path, highlightStrokePaint);
                    canvas.DrawTextOnPath("Stage " + vm.ActiveStage.StageLevel.ToString(), path, 0, 0, blackLargeTextPaint);
                }

                //draw stage nav
                //set next stage button image
                bool IsMoreStages = vm.CampaignObj.CategoryQueue.Any();
                //set next button
                SKBitmap nextButton = null;
                bool nextStageLocked = true;
                if (vm.ActiveStage.IsUnLocked)
                {
                    nextButton = stageNextUnlocked;
                    nextStageLocked = false;
                }
                else
                {
                    nextButton = stageNextLocked;
                }
                //line previous to middle
                canvas.DrawLine(trueZeroX, trueEndY - bottomBorder - nextButton.Height / 2, 0, trueEndY - bottomBorder - nextButton.Height / 2, highlightStrokePaint);
                //line middle to section 0
                canvas.DrawLine(0, trueEndY - bottomBorder - nextButton.Height / 2, 0, 460, highlightStrokePaint);
                if (IsMoreStages || vm.ActiveStage != vm.CampaignObj.Stages.Last())
                {
                    //line middle to next
                    if (nextStageLocked)
                    {
                        canvas.DrawLine(0, trueEndY - bottomBorder - nextButton.Height / 2, trueEndX, trueEndY - bottomBorder - nextButton.Height / 2, inactiveStrokePaint);
                    }
                    else
                    {
                        canvas.DrawLine(0, trueEndY - bottomBorder - nextButton.Height / 2, trueEndX, trueEndY - bottomBorder - nextButton.Height / 2, highlightStrokePaint);
                    }
                    //draw next stage button
                    SKRect rectNext = new SKRect(trueEndX - nextButton.Width, trueEndY - bottomBorder - nextButton.Height, trueEndX, trueEndY - bottomBorder);
                    canvas.DrawBitmap(nextButton, rectNext, whiteFillPaint);
                    RegisterClickArea("next", rectNext);
                }
                //draw previous stage button
                if (vm.ActiveStage.StageLevel > 1)
                {
                    SKRect rectPrevious = new SKRect(trueZeroX, trueEndY - bottomBorder - stagePreviousUnlocked.Height, trueZeroX + stagePreviousUnlocked.Width, trueEndY - bottomBorder);
                    canvas.DrawBitmap(stagePreviousUnlocked, rectPrevious, whiteFillPaint);
                    RegisterClickArea("previous", rectPrevious);
                }

                //draw connector lines
                bool section0Complete = vm.ActiveStage.Sections[0].IsComplete;
                bool section1Complete = vm.ActiveStage.Sections[1].IsComplete;
                bool section2Complete = vm.ActiveStage.Sections[2].IsComplete;
                bool section3Complete = vm.ActiveStage.Sections[3].IsComplete;
                bool section4Complete = vm.ActiveStage.Sections[4].IsComplete;
                bool section5Perfect = vm.ActiveStage.Sections[5].IsComplete && vm.ActiveStage.Sections[5].NumberCorrect == 5;
                // from 0 - 1
                canvas.DrawLine(0, 460, -290, 230, section0Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 0 - 2
                canvas.DrawLine(0, 460, 290, 230, section0Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 1 - 3
                canvas.DrawLine(-290, 230, 0, 0, section1Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 2 - 3
                canvas.DrawLine(290, 230, 0, 0, section2Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 3 - 4
                canvas.DrawLine(0, 0, -290, -230, section3Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 3 - 5
                canvas.DrawLine(0, 0, 290, -230, section3Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 5 - 6
                canvas.DrawLine(290, -230, 0, -460, section5Perfect ? highlightStrokePaint : inactiveStrokePaint);

                //draw section circles
                SKPoint center = new SKPoint();
                // section 0
                center.X = 0; center.Y = 460;
                DrawSectionCircle(canvas, 0, true, center);
                // section 1
                center.X = -290; center.Y = 230;
                DrawSectionCircle(canvas, 1, section0Complete, center);
                // section 2
                center.X = 290; center.Y = 230;
                DrawSectionCircle(canvas, 2, section0Complete, center);
                // section 3
                center.X = 0; center.Y = 0;
                DrawSectionCircle(canvas, 3, (section1Complete || section2Complete), center);
                // section 4
                center.X = -290; center.Y = -230;
                DrawSectionCircle(canvas, 4, section3Complete, center);
                // section 5
                center.X = 290; center.Y = -230;
                DrawSectionCircle(canvas, 5, section3Complete, center);
                // section 6 - Bonus
                center.X = 0; center.Y = -460;
                DrawSectionCircle(canvas, 6, section5Perfect, center);
            }
        }

        private List<CampaignBoardClickArea> clickAreas = new List<CampaignBoardClickArea>();

        private void RegisterClickArea(string name, SKRect area, string value = null)
        {
            int adjWidth = screenWidth / 2;
            int adjHeight = screenHeight / 2;
            SKRect newArea = new SKRect(area.Left + adjWidth, area.Top + adjHeight, area.Right + adjWidth, area.Bottom + adjHeight);
            clickAreas.Add(new CampaignBoardClickArea() { Name = name, Area = newArea, Value = value });
        }

        private void canvasView_Touch(object sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                SKPoint touchPoint = e.Location;
                for (int i = 0; i < clickAreas.Count; i++)
                {
                    if (clickAreas[i].Area.Contains(touchPoint))
                    {
                        //Debug.WriteLine("Clicked: " + clickAreas[i].Name);
                        switch (clickAreas[i].Name)
                        {
                            case "Section":
                                vm.SelectSection(clickAreas[i].Value);
                                break;
                            case "SectionRetry":
                                vm.RetrySection(clickAreas[i].Value);
                                break;
                            case "previous":
                                vm.SelectPreviousStage();
                                break;
                            case "next":
                                vm.SelectNextStage();
                                break;
                            //case "TutorialSectionNewlyComplete":
                            //    vm.CloseTutorialSectionNewlyComplete();
                            //    break;
                        }
                        break;
                    }
                }
            }
            e.Handled = true;
        }

        private void DrawSectionCircle(SKCanvas canvas, int sectionIdx, bool IsActive, SKPoint center)
        {
            //Debug.WriteLine("Draw section " + sectionIdx.ToString());
            GameSection section = null;
            SKRect rect = new SKRect();
            float radius = 87;
            float startAngle = 0;
            float sweepAngleBase = 360f / 5;
            float sweepAngle = 0;

            section = vm.ActiveStage.Sections[sectionIdx];
            if (IsActive)
            {
                if (section.IsComplete)
                {
                    //draw complete circle
                    float sectionRadius = radius;
                    if (section.NewlyComplete) { sectionRadius -= (newAnimationCountdown *2); Debug.WriteLine(newAnimationCountdown.ToString() + "   " + sectionRadius.ToString()); }

                    rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);
                    canvas.DrawCircle(center.X, center.Y, sectionRadius, completeFillPaint);

                    RegisterClickArea("SectionRetry", rect, sectionIdx.ToString());


                    if (section.NewlyComplete && newAnimationCountdown == 0) { section.NewlyComplete = false; }
                }
                else
                {
                    //draw pie wedges
                    rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);
                    canvas.DrawCircle(center.X, center.Y, radius, blackFillPaint);
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
                    RegisterClickArea("Section", rect, sectionIdx.ToString() );
                }
            }
            else
            {
                //draw inactive circle
                canvas.DrawCircle(center.X, center.Y, radius, whiteFillPaint);
                canvas.DrawCircle(center.X, center.Y, radius, inactiveStrokePaint);
            }

            //draw text
            SKPoint textPoint = new SKPoint();
            SKPaint newPaint = inactiveSmallTextPaint.Clone();
            if (IsActive) { newPaint = blackSmallTextPaint.Clone(); }
            switch (sectionIdx)
            {
                case 0:
                    textPoint.X = center.X + radius - 8;
                    textPoint.Y = center.Y + (radius / 2) + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 3:
                    textPoint.X = center.X + radius + 12;
                    textPoint.Y = center.Y - 7 + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 1:
                case 4:
                    textPoint.X = center.X - radius - radius;
                    textPoint.Y = center.Y + radius + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 2:
                case 5:
                    textPoint.X = center.X - radius;
                    textPoint.Y = center.Y + radius + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
                case 6:
                    textPoint.X = center.X + radius + 8; ;
                    textPoint.Y = center.Y - radius + newPaint.TextSize;
                    newPaint.TextAlign = SKTextAlign.Left;
                    break;
            }


            using (SKPath path = new SKPath())
            {
                path.MoveTo(textPoint.X, textPoint.Y);
                path.LineTo(textPoint.X + 400, textPoint.Y);

                canvas.DrawTextOnPath(section.SectionName, path, 0, 0, newPaint);
            }
            SKPoint starPoint = new SKPoint(textPoint.X, textPoint.Y + 8);
            for (int i = 1; i <= 5; i++)
            {
                canvas.Save();
                canvas.Translate(starPoint.X + 20, starPoint.Y + 20);
                canvas.DrawPath(starPath, section.NumberCorrect >= i ? yellowFillPaint : inactiveFillPaint);
                canvas.Restore();
                starPoint.X += starPath.TightBounds.Width + 8;
            }
        }
    }


    public class CampaignBoardClickArea
    {
        public string Name { get; set; }
        public SKRect Area { get; set; }
        public string Value { get; set; }

    }
}