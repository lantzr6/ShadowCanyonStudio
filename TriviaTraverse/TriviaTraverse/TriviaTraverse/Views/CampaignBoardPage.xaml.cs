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

        #region "Properties"
        private int newAnimationCountdown;

        private bool UpdateBoard = true;

        SKBitmap stageNextUnlocked = new SKBitmap { };
        SKBitmap stageNextLocked = new SKBitmap { };
        SKBitmap stagePreviousUnlocked = new SKBitmap { };
        SKBitmap stagePreviousLocked = new SKBitmap { };

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

        SKPoint section0Center; SKPoint section1Center; SKPoint section2Center; SKPoint section3Center; SKPoint section4Center; SKPoint section5Center; SKPoint section6Center;
        SKPath GameStagePath; SKPath starPath;
        #endregion




        public CampaignBoardPage()
        {
            InitializeComponent();

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream2 = assembly.GetManifestResourceStream("TriviaTraverse.images.unlocked_lefthalf.png");

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

                section3Center = new SKPoint(screenWidth / 2, (screenHeight / 2) - (screenHeight * .05f));
                section0Center = new SKPoint(section3Center.X, section3Center.Y + (screenHeight * .30f));
                section1Center = new SKPoint(section3Center.X - (screenWidth * .30f), section3Center.Y + (screenHeight * .175f));
                section2Center = new SKPoint(section3Center.X + (screenWidth * .30f), section3Center.Y + (screenHeight * .175f));
                section4Center = new SKPoint(section3Center.X - (screenWidth * .30f), section3Center.Y - (screenHeight * .175f));
                section5Center = new SKPoint(section3Center.X + (screenWidth * .30f), section3Center.Y - (screenHeight * .175f));
                section6Center = new SKPoint(section3Center.X, section3Center.Y - (screenHeight * .30f));
            }
            
            clickAreas.Clear();

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (vm.CampaignObj != null && vm.ActiveStage != null)
            {              
                canvas.DrawTextOnPath("Stage " + vm.ActiveStage.StageLevel.ToString(), GameStagePath, 0, 0, blackLargeTextPaint);

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
                //base traverse line Y
                float baseLineY = (screenHeight * .9f);
                //line previous to middle
                canvas.DrawLine(0, baseLineY, section0Center.X, baseLineY, highlightStrokePaint);
                //line middle to section 0
                canvas.DrawLine(screenWidth / 2, baseLineY, section0Center.X, section0Center.Y, highlightStrokePaint);
                if (IsMoreStages || vm.ActiveStage != vm.CampaignObj.Stages.Last())
                {
                    //line middle to next
                    canvas.DrawLine(screenWidth / 2, baseLineY, screenWidth, baseLineY, nextStageLocked?inactiveStrokePaint: highlightStrokePaint);

                    //draw next stage button
                    // Draw a bitmap rescaled
                    float target = screenWidth * .1f;
                    float scaleFactor = target / float.Parse(nextButton.Width.ToString());
                    canvas.SetMatrix(SKMatrix.MakeScale(scaleFactor, scaleFactor));
                    SKPoint pos = new SKPoint(screenWidth - (nextButton.Width * scaleFactor), baseLineY - ((nextButton.Height * scaleFactor) / 2));
                    SKRect rectNext = new SKRect(pos.X, pos.Y, screenWidth, pos.Y + (nextButton.Width * scaleFactor));
                    RegisterClickArea("next", rectNext);
                    canvas.DrawBitmap(nextButton, pos.X / scaleFactor, pos.Y / scaleFactor);
                    canvas.ResetMatrix();
                }
                //draw previous stage button
                if (vm.ActiveStage.StageLevel > 1)
                {
                    // Draw a bitmap rescaled
                    float target = screenWidth * .1f;
                    float scaleFactor = target / float.Parse(stagePreviousUnlocked.Width.ToString());
                    canvas.SetMatrix(SKMatrix.MakeScale(scaleFactor, scaleFactor));
                    SKPoint pos = new SKPoint(0, baseLineY - ((stagePreviousUnlocked.Height * scaleFactor) / 2));
                    SKRect rectPrevious = new SKRect(pos.X, pos.Y, (stagePreviousUnlocked.Width * scaleFactor), pos.Y + (stagePreviousUnlocked.Height * scaleFactor));
                    RegisterClickArea("previous", rectPrevious);
                    canvas.DrawBitmap(stagePreviousUnlocked, pos.X, pos.Y / scaleFactor);
                    canvas.ResetMatrix();
                }

                //draw connector lines
                bool section0Complete = vm.ActiveStage.Sections[0].IsComplete;
                bool section1Complete = vm.ActiveStage.Sections[1].IsComplete;
                bool section2Complete = vm.ActiveStage.Sections[2].IsComplete;
                bool section3Complete = vm.ActiveStage.Sections[3].IsComplete;
                bool section4Complete = vm.ActiveStage.Sections[4].IsComplete;
                bool section5Perfect = vm.ActiveStage.Sections[5].IsComplete && vm.ActiveStage.Sections[5].NumberCorrect == 5;
                // from 0 - 1
                canvas.DrawLine(section0Center.X, section0Center.Y, section1Center.X, section1Center.Y, section0Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 0 - 2
                canvas.DrawLine(section0Center.X, section0Center.Y, section2Center.X, section2Center.Y, section0Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 1 - 3
                canvas.DrawLine(section1Center.X, section1Center.Y, section3Center.X, section3Center.Y, section1Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 2 - 3
                canvas.DrawLine(section2Center.X, section2Center.Y, section3Center.X, section3Center.Y, section2Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 3 - 4
                canvas.DrawLine(section3Center.X, section3Center.Y, section4Center.X, section4Center.Y, section3Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 3 - 5
                canvas.DrawLine(section3Center.X, section3Center.Y, section5Center.X, section5Center.Y, section3Complete ? highlightStrokePaint : inactiveStrokePaint);
                // from 5 - 6
                canvas.DrawLine(section5Center.X, section5Center.Y, section6Center.X, section6Center.Y, section5Perfect ? highlightStrokePaint : inactiveStrokePaint);

                //draw section circles
                // section 0
                DrawSectionCircle(canvas, 0, true, section0Center);
                // section 1
                DrawSectionCircle(canvas, 1, section0Complete, section1Center);
                // section 2
                DrawSectionCircle(canvas, 2, section0Complete, section2Center);
                // section 3
                DrawSectionCircle(canvas, 3, (section1Complete || section2Complete), section3Center);
                // section 4
                DrawSectionCircle(canvas, 4, section3Complete, section4Center);
                // section 5
                DrawSectionCircle(canvas, 5, section3Complete, section5Center);
                // section 6 - Bonus
                DrawSectionCircle(canvas, 6, section5Perfect, section6Center);
            }
        }

        private List<CampaignBoardClickArea> clickAreas = new List<CampaignBoardClickArea>();

        private void RegisterClickArea(string name, SKRect area, string value = null)
        {
            SKRect newArea = new SKRect(area.Left, area.Top, area.Right, area.Bottom);
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
            float startAngle = 0;
            float sweepAngleBase = 360f / 5;
            float sweepAngle = 0;

            section = vm.ActiveStage.Sections[sectionIdx];
            if (IsActive)
            {
                if (section.IsComplete)
                {
                    //draw complete circle
                    float sectionRadiusAni = sectionRadius;  //setup animation
                    if (section.NewlyComplete) { sectionRadiusAni -= (newAnimationCountdown * 2); }
                    //Debug.WriteLine(newAnimationCountdown.ToString() + "   " + sectionRadiusAni.ToString()); }

                    rect = new SKRect(center.X - sectionRadius, center.Y - sectionRadius, center.X + sectionRadius, center.Y + sectionRadius);
                    canvas.DrawCircle(center.X, center.Y, sectionRadiusAni, completeFillPaint);

                    RegisterClickArea("SectionRetry", rect, sectionIdx.ToString());

                    if (section.NewlyComplete && newAnimationCountdown == 0) { section.NewlyComplete = false; }
                }
                else
                {
                    //draw pie wedges
                    rect = new SKRect(center.X - sectionRadius, center.Y - sectionRadius, center.X + sectionRadius, center.Y + sectionRadius);
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
                    RegisterClickArea("Section", rect, sectionIdx.ToString() );
                }
            }
            else
            {
                //draw inactive circle
                canvas.DrawCircle(center.X, center.Y, sectionRadius, whiteFillPaint);
                canvas.DrawCircle(center.X, center.Y, sectionRadius, inactiveStrokePaint);
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


    public class CampaignBoardClickArea
    {
        public string Name { get; set; }
        public SKRect Area { get; set; }
        public string Value { get; set; }

    }
}