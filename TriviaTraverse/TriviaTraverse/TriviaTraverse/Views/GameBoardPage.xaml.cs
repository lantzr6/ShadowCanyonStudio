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

        private List<GameBoardClickArea> clickAreas = new List<GameBoardClickArea>();

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

        SKPaint blackSmallTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 46 };
        SKPaint graphLabelTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 46, TextAlign = SKTextAlign.Right };
        SKPaint inactiveSmallTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gray, TextSize = 46 };
        SKPaint blackLargeTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black, TextSize = 80 };
        SKPaint playerNameTextPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.White, TextSize = 46, TextAlign = SKTextAlign.Center };

        SKPaint highlightFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.CornflowerBlue };
        SKPaint stepFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Brown.WithAlpha(127) };
        SKPaint questionFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Fuchsia.WithAlpha(127) };
        SKPaint inactiveFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gray };
        SKPaint yellowFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Gold };


        SKPaint optionStepsFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.DarkSalmon };
        SKPaint optionStepsFreeFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Salmon };
        SKPaint optionProgressFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.DarkOrchid };
        SKPaint optionScoreFillPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Pink };

        SKPaint paintGraph = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.LightGreen, StrokeWidth = 8, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round };
        SKPaint paintLines = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.GreenYellow, StrokeWidth = 6, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round };
        SKPaint paintMntOutline = new SKPaint { Style = SKPaintStyle.Stroke, Color = SKColors.DarkGray, StrokeWidth = 4, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round };
        SKPath GameNamePath; SKPath StartTimePath; SKPath ScorePath;
        #endregion

        public GameBoardPage(int vGameId)
        {
            InitializeComponent();

            DisplayOption = "steps";

            lstCategory.ItemTemplate = new DataTemplate(typeof(WrappedItemSelectionTemplate));
            
            vm = new GameBoardPageViewModel(Navigation, vGameId);
            this.BindingContext = vm;
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
            VGame _vgame = vm.VGameObj;
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
                graphLabelTextPaint.TextSize = screenHeight * .0221f;
                inactiveSmallTextPaint.TextSize = blackSmallTextPaint.TextSize;
                blackLargeTextPaint.TextSize = screenHeight * .0385f;
                playerNameTextPaint.TextSize = screenHeight * .0221f;

                paintGraph.StrokeWidth = screenWidth * .0056f;
                paintLines.StrokeWidth = screenWidth * .0042f;
                paintMntOutline.StrokeWidth = screenWidth * .0028f;

                GameNamePath = new SKPath();
                GameNamePath.MoveTo(0, screenHeight * .0577f);
                GameNamePath.RLineTo(screenWidth, 0);
                ScorePath = new SKPath();
                ScorePath.MoveTo(screenWidth * .6573f, screenHeight * .65f);
                ScorePath.RLineTo(screenWidth, 0);
                StartTimePath = new SKPath();
                StartTimePath.MoveTo(screenWidth / 2, screenHeight * .0577f);
                StartTimePath.RLineTo(screenWidth, 0);

                ObjectsCreated = true;
            }
            clickAreas.Clear();

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (_vgame != null)
            {
                canvas.DrawTextOnPath(_vgame.GameName, GameNamePath, 0, 0, blackLargeTextPaint);
                canvas.DrawTextOnPath(_vgame.StartTimeLocal.ToString(), StartTimePath, 0, blackLargeTextPaint.FontSpacing, blackLargeTextPaint);
                canvas.DrawTextOnPath(_vgame.EndTimeLocal.ToString(), StartTimePath, 0, (blackLargeTextPaint.FontSpacing *2) + 4, blackLargeTextPaint);

                if (vm.ViewState == "Self")
                {
                    ////draw stats button
                    //canvas.DrawRect(statsRect, highlightFillPaint);
                    //RegisterClickArea("ShowStats", statsRect, null);

                    //draw Score
                    canvas.DrawTextOnPath("Score", ScorePath, 0, 0, blackLargeTextPaint);
                    canvas.DrawTextOnPath(_vgame.PlayerScore.ToString(), ScorePath, 0, blackLargeTextPaint.TextSize + margin10, blackSmallTextPaint);

                    bool nextButton = false;
                    int numberMnts = 1;
                    switch (_vgame.GameStepCap)
                    {
                        case 15000:
                            numberMnts = 4;
                            break;
                        case 10000:
                            numberMnts = 3;
                            break;
                        case 8000:
                            numberMnts = 3;
                            break;
                        case 5000:
                            numberMnts = 3;
                            break;
                    }
                    nextButton = (_vgame.Sections.Count() == 1 && _vgame.Sections[0].IsComplete);
                    //draw mountains
                    //draw section mountains
                    int fieldIdx = numberMnts -2;
                    for (int i = numberMnts; i >= 1; i--)
                    {
                        int sectionType = 2; // field
                        bool isActive = false;
                        if (i == 1)
                        { sectionType = 1; isActive = true; }
                        else
                        {
                            if (i == 1) { isActive = true; }
                            else if (i <= _vgame.Sections.Count())
                            {
                                isActive = _vgame.Sections[i - 2].IsComplete;
                            }
                            if (i == numberMnts) { sectionType = 3; }
                            //if (numberMnts == 5 && fieldIdx == 3) { --fieldIdx; } //skip position 3 for 20k game
                        }
                        DrawMountain(canvas, i - 1, sectionType, fieldIdx, isActive);
                        if (i < numberMnts) { --fieldIdx; }
                    }

                    float statsTopY = screenHeight * .8f;
                    float statsHeight = screenHeight - statsTopY;
                    float userAreaX = screenWidth * .75f;
                    float graphX = screenWidth * .06f;

                    SKRect statBack = new SKRect(0, statsTopY, screenWidth, screenHeight);
                    canvas.DrawRect(statBack, inactiveFillPaint);

                    SKRect usersBack = new SKRect(userAreaX, statsTopY, screenWidth, screenHeight);
                    canvas.DrawRect(usersBack, highlightFillPaint);

                    SKRect optionsBack = new SKRect(0, statsTopY, graphX, screenHeight);
                    canvas.DrawRect(optionsBack, yellowFillPaint);

                    //option buttons
                    //SKRect optionsSteps = new SKRect(0, statsTopY, graphX, screenHeight - (statsHeight * .6666f));
                    //canvas.DrawRect(optionsSteps, optionStepsFillPaint);
                    //RegisterClickArea("Option", optionsSteps, "steps");
                    //SKRect optionsProgress = new SKRect(0, screenHeight - (statsHeight * .6666f), graphX, screenHeight - (statsHeight * .3333f));
                    //canvas.DrawRect(optionsProgress, optionProgressFillPaint);
                    //RegisterClickArea("Option", optionsProgress, "progress");
                    //SKRect optionsScore = new SKRect(0, screenHeight - (statsHeight * .3333f), graphX, screenHeight);
                    //canvas.DrawRect(optionsScore, optionScoreFillPaint);
                    //RegisterClickArea("Option", optionsScore, "score");

                    //lines
                    float sectionWidth;
                    //if (DisplayOption == "steps" || DisplayOption == "progress")
                    //{
                        sectionWidth = (userAreaX - graphX) / numberMnts;
                        for (int i = 1; i <= numberMnts; i++)
                        {
                            canvas.DrawLine(userAreaX - (sectionWidth * i), statsTopY, userAreaX - (sectionWidth * i), screenHeight, paintLines);
                        }
                    //}
                    //else
                    //{
                    //    int numberSections = maxPoints / 500;
                    //    sectionWidth = (userAreaX - graphX) / numberSections;
                    //    for (int i = 1; i <= numberSections; i++)
                    //    {
                    //        canvas.DrawLine(userAreaX - (sectionWidth * i), statsTopY, userAreaX - (sectionWidth * i), screenHeight, paintLines);
                    //    }
                    //}

                    int cnt = _vgame.VGamePlayers.Count();
                    float playerHeight = statsHeight / cnt;
                    float graphBarWidth = screenWidth * .04167f;

                    int iPlayer = 0;
                    foreach (VGamePlayer vgp in _vgame.VGamePlayers)
                    {
                        string playerUserName = vgp.UserName;
                        int steps = vgp.GameSteps;
                        int score = vgp.Score;
                        int QuestionsAnswered = vgp.QuestionsAnswered;

                        float playerStartX = userAreaX;
                        float playerStartY = statsTopY + ((playerHeight) * iPlayer - 1) + (playerHeight / 2);

                        using (SKPath path = new SKPath())
                        {
                            path.MoveTo(playerStartX, playerStartY);
                            path.LineTo(screenWidth, playerStartY);

                            canvas.DrawTextOnPath(string.Format("{0}", playerUserName), path, 0, playerNameTextPaint.TextSize / 2, playerNameTextPaint);
                            canvas.DrawTextOnPath(string.Format("{0}", vgp.Score), path, 0, playerNameTextPaint.TextSize / 2 + playerNameTextPaint.TextSize + 2, playerNameTextPaint);
                        }

                        //make this better
                        int statValue = 0;
                        float playerBarCenterY = playerStartY;
                        SKRect qRect;
                        //switch (DisplayOption)
                        //{
                        //    case "steps":
                                statValue = vgp.GameSteps;
                                qRect = new SKRect(userAreaX - sectionWidth, playerBarCenterY - (graphBarWidth / 2), userAreaX, playerBarCenterY + (graphBarWidth / 2));
                                canvas.DrawRect(qRect, optionStepsFreeFillPaint);
                                qRect = new SKRect(userAreaX - sectionWidth - (sectionWidth * (statValue / 5000f)), playerBarCenterY - (graphBarWidth / 2), userAreaX - sectionWidth, playerBarCenterY + (graphBarWidth / 2));
                                canvas.DrawRect(qRect, optionStepsFillPaint);
                                //break;
                            //case "progress":
                                statValue = vgp.QuestionsAnswered;
                                qRect = new SKRect(userAreaX - ((sectionWidth / 5) * statValue), playerBarCenterY - (graphBarWidth / 2), userAreaX, playerBarCenterY + (graphBarWidth / 2));
                                canvas.DrawRect(qRect, optionProgressFillPaint);
                                //break;
                            //case "score":
                            //    statValue = vgp.Score;
                            //    qRect = new SKRect(userAreaX - (sectionWidth * (statValue / 500f)), playerBarCenterY - (graphBarWidth / 2), userAreaX, playerBarCenterY + (graphBarWidth / 2));
                            //    canvas.DrawRect(qRect, optionScoreFillPaint);
                            //    break;
                        //}
                        using (SKPath path = new SKPath())
                        {
                            path.MoveTo(graphX, playerBarCenterY + (graphBarWidth / 2));
                            path.LineTo(userAreaX - margin4, playerBarCenterY + (graphBarWidth / 2));

                            canvas.DrawTextOnPath(statValue.ToString(), path, 0, -margin2, graphLabelTextPaint);
                        }

                        //SKRect qSteps = new SKRect(playerBarCenterX + (playerStatsMargin / 2), playerStartY - (riseX * (vgp.GameSteps / 5000f)), playerBarCenterX + (playerStatsMargin / 2) + graphBarWidth, playerStartY);
                        //canvas.DrawRect(qSteps, stepFillPaint);
                        //using (SKPath path = new SKPath())
                        //{
                        //    path.MoveTo(playerBarCenterX + (playerStatsMargin / 2) + graphBarWidth, playerStartY - (playerStatsMargin / 2));
                        //    path.RLineTo(0, -graphBarTextLength);

                        //    canvas.DrawTextOnPath("STEPS", path, 0, 0, blackSmallTextPaint);
                        //}

                        iPlayer++;
                    }

                    if (nextButton)
                    {
                        //draw next button
                        SKRect nextBtn = new SKRect(screenWidth * .2f, (screenHeight / 2) - (screenHeight * .05f), screenWidth * .8f, (screenHeight / 2) + (screenHeight * .05f));
                        canvas.DrawRect(nextBtn, highlightFillPaint);

                        RegisterClickArea("Next", nextBtn);
                    }

                }
            }
        }

        private void DrawMountain(SKCanvas canvas, int sectionIdx, int sectionType, int fieldIdx, bool IsActive)  //sectionType: 1 = first, 2 = field, 3 = last
        {
            GameSection section = ((sectionIdx < vm.VGameObj.Sections.Count()) ? vm.VGameObj.Sections[sectionIdx] : null);

            SKPoint topleft = new SKPoint();

            float mntWidth = 0;
            float mntHeight = 0;
            switch (sectionType)
            {
                case 1:
                    mntWidth = screenWidth * .5f;
                    mntHeight = screenHeight * .16f;
                    topleft.X = screenWidth * .01f;
                    topleft.Y = screenHeight * .6f;
                    break;
                case 3:
                    mntWidth = screenWidth * .75f;
                    mntHeight = screenHeight * .25f;
                    topleft.X = screenWidth * .06f;
                    topleft.Y = screenHeight * .06f;
                    break;
                default:
                    mntWidth = screenWidth * .3f;
                    mntHeight = screenHeight * .10f;
                    switch (fieldIdx)
                    {
                        case 1:
                            topleft.X = screenWidth * .5f;
                            topleft.Y = screenHeight * .5f;
                            break;
                        case 2:
                            topleft.X = screenWidth * .35f;
                            topleft.Y = screenHeight * .45f;
                            break;
                        case 3:
                            topleft.X = screenWidth * .65f;
                            topleft.Y = screenHeight * .43f;
                            break;
                        case 4:
                            topleft.X = screenWidth * .05f;
                            topleft.Y = screenHeight * .30f;
                            break;
                        case 5:
                            topleft.X = screenWidth * .2f;
                            topleft.Y = screenHeight * .27f;
                            break;
                    }
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

                textPoint.X = blX + ((fieldIdx == 3 || fieldIdx == 5 || sectionType == 3) ? mntWidth / 2 : 0);
                textPoint.Y = blY + newPaint.TextSize + margin2;
                newPaint.TextAlign = SKTextAlign.Left;

                using (SKPath path = new SKPath())
                {
                    path.MoveTo(textPoint.X, textPoint.Y);
                    path.LineTo(textPoint.X + (screenWidth * .2778f), textPoint.Y);

                    canvas.DrawTextOnPath(section.SectionName, path, 0, 0, newPaint);
                }
                ////stars
                //SKPoint starPoint = new SKPoint(textPoint.X, textPoint.Y + margin8);
                //for (int i = 1; i <= 5; i++)
                //{

                //    canvas.Save();
                //    canvas.Translate(starPoint.X + (margin10*2), starPoint.Y + (margin10 * 2));
                //    canvas.DrawPath(starPath, section.NumberCorrect >= i ? yellowFillPaint : inactiveFillPaint);
                //    canvas.Restore();
                //    starPoint.X += starPath.TightBounds.Width + margin8;
                //}
            }
            //draw text - TEMP
            SKPoint textPointT = new SKPoint();
            SKPaint newPaintT = inactiveSmallTextPaint.Clone();
            if (IsActive) { newPaintT = blackSmallTextPaint.Clone(); }

            textPointT.X = topleft.X + (mntWidth / 2);
            textPointT.Y = topleft.Y + newPaintT.TextSize + margin2;
            newPaintT.TextAlign = SKTextAlign.Left;

            using (SKPath path = new SKPath())
            {
                path.MoveTo(textPointT.X, textPointT.Y);
                path.LineTo(textPointT.X + (screenWidth * .2778f), textPointT.Y);

                canvas.DrawTextOnPath(sectionIdx.ToString(), path, 0, 0, newPaintT);
            }

        }


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

        public string DisplayOption { get; set; }

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
                            case "Option":
                                DisplayOption = clickAreas[i].Value;
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