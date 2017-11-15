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
using TriviaTraverse.Controls;

namespace TriviaTraverse.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameBoardPage : ContentPage, IContentPage
    {
        private GameBoardPageViewModel vm;

        public string Name
        {
            get
            {
                return "Game";
            }
        }

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

        public GameBoardPage(int vGameId)
        {
            InitializeComponent();

            lstCategory.ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate));
            
            vm = new GameBoardPageViewModel(Navigation, vGameId);
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

            lstCategory.SelectedItem = null;
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
                //margin6 = margin2 * 3;
                margin8 = margin2 * 4;
                margin10 = margin2 * 5;

                blackSmallTextPaint.TextSize = screenHeight * .0221f;
                inactiveSmallTextPaint.TextSize = blackSmallTextPaint.TextSize;
                blackLargeTextPaint.TextSize = screenHeight * .0385f;
                playerNameTextPaint.TextSize = screenHeight * .0289f;

                paintGraph.StrokeWidth = screenWidth * .0056f;
                paintLines.StrokeWidth = screenWidth * .0042f;
                paintMntOutline.StrokeWidth = screenWidth * .0028f;

                GameNamePath = new SKPath();
                GameNamePath.MoveTo(0, screenHeight * .0577f);
                GameNamePath.RLineTo(screenWidth, 0);
                ScorePath = new SKPath();
                ScorePath.MoveTo(screenWidth * .6573f, screenHeight * .8075f);
                ScorePath.RLineTo(screenWidth, 0);
                statsRect = new SKRect(screenWidth * .722f, screenHeight * .9038f, screenWidth, screenHeight * .9519f);
                backRect = new SKRect(0, screenHeight * .9038f, screenWidth * .2778f, screenHeight * .9519f);

                List<SKPoint> starPoints = CalculateStarPoints(0, 0, 5, screenHeight * .01f, screenHeight * .005f);
                starPath = new SKPath();
                starPath.MoveTo(starPoints[0]);
                for (int i = 1; i < starPoints.Count - 1; i++)
                {
                    starPath.LineTo(starPoints[i]);
                }
                starPath.LineTo(starPoints.Last());
                starPath.Close();

                riseX = screenWidth * .1f;
                riseY = -(screenHeight * .086f);
                fallX = screenWidth * .02f;
                fallY = screenHeight * .014f;
                sectionRiseY = riseY + fallY;

                mntWidth = screenWidth * .5f;
                mntHeight = screenHeight * .16f;

                graphPath = new SKPath();
                graphPath.MoveTo((screenWidth * .08f) - fallX, (screenHeight * .72f) - fallY);
                graphPath.RLineTo(riseX - fallX, sectionRiseY);
                graphPath.RLineTo(fallX, fallY);
                graphPath.RLineTo(riseX, riseY);
                graphPath.RLineTo(fallX, fallY);
                graphPath.RLineTo(riseX, riseY);
                graphPath.RLineTo(fallX, fallY);
                graphPath.RLineTo(riseX, riseY);
                graphPath.RLineTo(fallX, fallY);
                graphPath.RLineTo(riseX, riseY);
                graphPath.RLineTo(fallX, fallY);
                graphPath.RLineTo(riseX, riseY);
                graphPath.RLineTo(fallX, fallY);
                graphPath.RLineTo(riseX, riseY);
                graphPath.RLineTo(fallX, fallY);

                graphLinePath = new SKPath();
                float widthX = ((riseX + fallX) * 6) + (riseX - fallX);
                graphLinePath.MoveTo(screenWidth * .08f - fallX, screenHeight * .72f - fallY);
                graphLinePath.RLineTo(widthX, 0);
                graphLinePath.RMoveTo(-widthX, sectionRiseY);
                graphLinePath.RLineTo(widthX, 0);
                graphLinePath.RMoveTo(-widthX, sectionRiseY);
                graphLinePath.RLineTo(widthX, 0);
                graphLinePath.RMoveTo(-widthX, sectionRiseY);
                graphLinePath.RLineTo(widthX, 0);
                graphLinePath.RMoveTo(-widthX, sectionRiseY);
                graphLinePath.RLineTo(widthX, 0);
                graphLinePath.RMoveTo(-widthX, sectionRiseY);
                graphLinePath.RLineTo(widthX, 0);
                graphLinePath.RMoveTo(-widthX, sectionRiseY);
                graphLinePath.RLineTo(widthX, 0);
                graphLinePath.RMoveTo(-widthX, sectionRiseY);
                graphLinePath.RLineTo(widthX, 0);

                ObjectsCreated = true;
            }
            clickAreas.Clear();

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (vm.VGameObj != null)
            {
                canvas.DrawTextOnPath(vm.VGameObj.GameName, GameNamePath, 0, 0, blackLargeTextPaint);
                
                if (vm.ViewState == "Self")
                {
                    //draw stats button
                    canvas.DrawRect(statsRect, highlightFillPaint);
                    RegisterClickArea("ShowStats", statsRect, null);

                    //draw Score
                    canvas.DrawTextOnPath("Score", ScorePath, 0, 0, blackLargeTextPaint);
                    canvas.DrawTextOnPath(vm.VGameObj.PlayerScore.ToString(), ScorePath, 0, blackLargeTextPaint.TextSize + margin10, blackSmallTextPaint);

                    //draw section mountains
                    for (int i = 6; i >= 1; i--)
                    {
                        bool isActive = false;
                        if (i <= vm.VGameObj.Sections.Count() - 1)
                        {
                            isActive = vm.VGameObj.Sections[i - 1].IsComplete;
                        }
                        DrawMountain(canvas, i, isActive);
                    }

                    // section 0
                    DrawMountain(canvas, 0, true);
                }
                else
                {
                    float graphBarWidth = screenWidth * .04167f;
                    float graphBarTextLength = screenHeight * .5f;
                    float graphMargin = screenWidth * .0139f;
                    float playerStatsMargin = screenWidth * .0069f;

                    canvas.DrawPath(graphLinePath, paintLines);
                    canvas.DrawPath(graphPath, paintGraph);

                    //draw back button
                    canvas.DrawRect(backRect, highlightFillPaint);
                    RegisterClickArea("Back", backRect, null);

                    int cnt = vm.VGameObj.VGamePlayers.Count();
                    float playerWidth = (screenWidth - (graphMargin*2) - (playerStatsMargin * cnt)) / cnt;

                    int i = 0;
                    foreach (VGamePlayer vgp in vm.VGameObj.VGamePlayers)
                    {
                        string playerUserName = vgp.UserName;
                        int steps = vgp.GameSteps;
                        int score = vgp.Score;
                        int QuestionsAnswered = vgp.QuestionsAnswered;

                        float playerStartX = graphMargin + ((playerWidth + playerStatsMargin) * i);
                        float playerStartY = (screenHeight * .72f) - fallY;

                        using (SKPath path = new SKPath())
                        {
                            path.MoveTo(playerStartX, playerStartY);
                            path.RLineTo(playerWidth, 0);

                            canvas.DrawTextOnPath(string.Format("{0}", playerUserName), path, 0, playerNameTextPaint.TextSize + margin4, playerNameTextPaint);
                        }

                        //make this better
                        float playerBarCenterX = playerStartX + (playerWidth/2);
                        SKRect qRect = new SKRect(playerBarCenterX - (playerStatsMargin / 2) - graphBarWidth, playerStartY + ((sectionRiseY / 5) * vgp.QuestionsAnswered), playerBarCenterX - (playerStatsMargin / 2), playerStartY);
                        canvas.DrawRect(qRect, questionFillPaint);
                        using (SKPath path = new SKPath())
                        {
                            path.MoveTo(playerBarCenterX - (playerStatsMargin/2), playerStartY - (playerStatsMargin / 2));
                            path.RLineTo(0, -graphBarTextLength);

                            canvas.DrawTextOnPath("PROGRESS", path, 0, 0, blackSmallTextPaint);
                        }

                        SKRect qSteps = new SKRect(playerBarCenterX + (playerStatsMargin / 2), playerStartY - (riseX * (vgp.GameSteps/5000f)), playerBarCenterX + (playerStatsMargin / 2) + graphBarWidth, playerStartY);
                        canvas.DrawRect(qSteps, stepFillPaint);
                        using (SKPath path = new SKPath())
                        {
                            path.MoveTo(playerBarCenterX + (playerStatsMargin / 2) + graphBarWidth, playerStartY - (playerStatsMargin / 2));
                            path.RLineTo(0, -graphBarTextLength);

                            canvas.DrawTextOnPath("STEPS", path, 0, 0, blackSmallTextPaint);
                        }

                        i++;
                    }
                }
            }
        }

        private void DrawMountain(SKCanvas canvas, int sectionIdx, bool IsActive)
        {
            GameSection section = ((sectionIdx < vm.VGameObj.Sections.Count()) ? vm.VGameObj.Sections[sectionIdx] : null);

            SKPoint topleft = new SKPoint();
            switch (sectionIdx)
            {
                case 0:
                    topleft.X = screenWidth * .06f;
                    topleft.Y = screenHeight * .74f;
                    break;
                case 1:
                    topleft.X = screenWidth * .32f;
                    topleft.Y = screenHeight * .47f;
                    break;
                case 2:
                    topleft.X = screenWidth * .02f;
                    topleft.Y = screenHeight * .46f;
                    break;
                case 3:
                    topleft.X = screenWidth * .54f;
                    topleft.Y = screenHeight * .36f;
                    break;
                case 4:
                    topleft.X = screenWidth * .16f;
                    topleft.Y = screenHeight * .22f;
                    break;
                case 5:
                    topleft.X = screenWidth * .40f;
                    topleft.Y = screenHeight * .11f;
                    break;
                case 6:
                    topleft.X = screenWidth * .08f;
                    topleft.Y = screenHeight * .08f;
                    break;
            }

            SKPaint paintFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = (IsActive ? ((section != null && section.IsComplete) ? SKColors.Green : SKColors.Black) : SKColors.Silver)
            };

            using (SKPath path = new SKPath())
            {
                path.MoveTo(topleft.X, topleft.Y + mntHeight);
                path.LineTo(topleft.X + (mntWidth / 2), topleft.Y);
                path.LineTo(topleft.X + mntWidth, topleft.Y + mntHeight);

                canvas.DrawPath(path, paintFill);  //fill
                if (IsActive)
                {
                    if (!section.IsComplete) { RegisterClickArea("Section", path, sectionIdx.ToString()); }
                }
                else
                {
                    canvas.DrawPath(path, paintMntOutline); //stroke
                }
            }

            if (IsActive)
            {
                float blX = topleft.X;
                float blY = topleft.Y + mntHeight;
                float brX = topleft.X + mntWidth;
                float brY = blY;

                if (!section.IsComplete)
                {
                    int numberAnswered = section.NumberAnswered;


                    float slX = blX + (numberAnswered * ((mntWidth / 2) / 5));
                    float slY = blY - (numberAnswered * (mntHeight / 5));
                    float srX = brX - (numberAnswered * ((mntWidth / 2) / 5));
                    float srY = slY;
                    using (SKPath path = new SKPath())
                    {
                        path.MoveTo(blX, blY);
                        path.LineTo(slX, slY);
                        path.LineTo(srX, srY);
                        path.LineTo(brX, brY);
                        path.Close();

                        canvas.DrawPath(path, highlightFillPaint);  //fill
                    }
                }
                //draw text
                SKPoint textPoint = new SKPoint();
                SKPaint newPaint = inactiveSmallTextPaint.Clone();
                if (IsActive) { newPaint = blackSmallTextPaint.Clone(); }

                textPoint.X = blX + ((sectionIdx == 3 || sectionIdx == 5) ? mntWidth / 2 : 0);
                textPoint.Y = blY + newPaint.TextSize + margin2;
                newPaint.TextAlign = SKTextAlign.Left;

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
                    canvas.Translate(starPoint.X + (margin10*2), starPoint.Y + (margin10 * 2));
                    canvas.DrawPath(starPath, section.NumberCorrect >= i ? yellowFillPaint : inactiveFillPaint);
                    canvas.Restore();
                    starPoint.X += starPath.TightBounds.Width + margin8;
                }
            }

            if (sectionIdx == 1 && !IsActive && vm.VGameObj.Sections[0].IsComplete)
            {
                //draw next button
                SKRect nextBtn = new SKRect(topleft.X, topleft.Y + (mntHeight / 4), topleft.X + mntWidth, topleft.Y + mntHeight - (mntHeight / 4));
                canvas.DrawRect(nextBtn, highlightFillPaint);

                RegisterClickArea("Next", nextBtn, sectionIdx.ToString());
            }
        }

        private List<GameBoardClickArea> clickAreas = new List<GameBoardClickArea>();

        private void RegisterClickArea(string name, SKPath area, string value = null)
        {
            SKPath newArea = new SKPath(area);
            clickAreas.Insert(0,new GameBoardClickArea() { Name = name, AreaPath = newArea, Value = value });
        }
        private void RegisterClickArea(string name, SKRect area, string value = null)
        {
            SKRect newArea = new SKRect(area.Left,area.Top, area.Right, area.Bottom);
            clickAreas.Insert(0, new GameBoardClickArea() { Name = name, AreaRect = newArea, Value = value });
        }

        private void canvasView_Touch(object sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                //SKPoint touchPoint = e.Location;
                for (int i = 0; i < clickAreas.Count; i++)
                {
                    bool clicked = false;
                    GameBoardClickArea area = clickAreas[i];
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
                            case "Next":
                                vm.SectionNext();
                                break;
                            case "ShowStats":
                                vm.ShowStats();
                                break;
                            case "Back":
                                vm.ShowSelf();
                                break;
                        }
                        break;
                    }
                }
            }
            e.Handled = true;
        }



    }


    public class GameBoardClickArea
    {
        public string Name { get; set; }
        public SKRect AreaRect { get; set; }
        public SKPath AreaPath { get; set; }
        public string Value { get; set; }

    }
}