using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaTraverseApi.Models
{
    public class MobileVGame
    {
        public int VGameId { get; set; }
        public string GameName { get; set; }
        public string GameTypeName { get; set; }
        public bool IsPrivate { get; set; }
        public int GameStepCap { get; set; }
        public int GameLength { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PlayerMax { get; set; }
        public int PlayerGameSteps { get; set; }
        public int PlayerGameStepBank { get; set; }
        public int PlayerScore { get; set; }
        public DateTime PlayerLastStepQuery { get; set; }

        public IList<MobileVGamePlayer> VGamePlayers { get; set; }

        public IList<MobileVGameSection> Sections { get; set; }
        public IList<MobileVGameCategory> CategoryQueue { get; set; }

        public MobileVGame() { }
        public MobileVGame(VGame vg, int playerid)
        {
            this.VGameId = vg.VGameId;
            this.GameName = vg.GameName;
            this.GameTypeName = vg.GameType.Description;
            this.IsPrivate = vg.IsPrivate;
            this.GameStepCap = vg.StepCap;
            this.GameLength = vg.GameLength;
            this.StartTime = vg.StartTime;
            this.EndTime = vg.EndTime;
            this.PlayerMax = vg.PlayerMax;
            this.Sections = new List<MobileVGameSection>();
            //this.Sections.Add(new MobileVGameSection(vg, playerid));
            List<VGameCategory> l = vg.VGameCategories.Where(o => o.VGamePlayerCategories.Where(p => p.PlayerId == playerid).Any()).ToList();
            foreach (VGameCategory vcat in l)
            {
                VGamePlayerCategory pcat = vcat.VGamePlayerCategories.Where(o => o.PlayerId == playerid).FirstOrDefault();
                this.Sections.Add(new MobileVGameSection(pcat, playerid));
            }
            this.CategoryQueue = new List<MobileVGameCategory>();
            foreach (VGameCategory vcat in vg.VGameCategories)
            {
                this.CategoryQueue.Add(new MobileVGameCategory(vcat, playerid));
            }
            this.VGamePlayers = new List<MobileVGamePlayer>();
            foreach (VGamePlayer vgp in vg.VGamePlayers)
            {
                this.VGamePlayers.Add(new MobileVGamePlayer(vgp));
                if (vgp.PlayerId == playerid) { this.PlayerScore = vgp.Score; this.PlayerGameSteps = vgp.GameSteps; this.PlayerGameStepBank = vgp.StepBank; this.PlayerLastStepQuery = vgp.LastStepUpdate; }
            };
        }

    }

    public class MobileVGameSection
    {
        public int VGamePlayerCategoryId { get; set; }
        public string SectionName { get; set; }
        public int SectionOrder { get; set; }
        public IList<MobileQuestion> Questions { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberAnswered { get; set; }
        public int NumberCorrect { get; set; }
        public int EarnedStars { get; set; }
        public int EarnedPoints { get; set; }
        public GameSectionType SectionType { get; set; }

        public MobileVGameSection() { }
        //public MobileVGameSection(VGame vg, int playerid)  //used to create player first section versus game section
        //{
        //    this.SectionType = GameSectionType.VersusFirst;
        //    this.VGamePlayerCategoryId = 0;
        //    this.SectionName = "Player Start";
        //    this.Questions = new List<MobileQuestion>();
        //    ICollection<VGamePlayerSectionQuestion> secQuestions = vg.VGamePlayerSectionQuestions.Where(o => o.PlayerId == playerid).ToList();
        //    foreach (VGamePlayerSectionQuestion sq in secQuestions)
        //    {
        //        this.Questions.Add(new MobileQuestion(sq.Question, playerid));
        //    }

        //    this.NumberOfQuestions = secQuestions.Count();
        //    List<PlayerQuestionResult> results = (from e in secQuestions select e.Question).Select(x => x.PlayerQuestionResults.Where(o => o.PlayerId == playerid).FirstOrDefault()).Where(o => o != null).ToList();
        //    this.NumberAnswered = results.Count();
        //    if (results.Count() > 0)
        //    {
        //        this.NumberCorrect = results.Where(o => o.IsCorrect.GetValueOrDefault()).Count();
        //        this.EarnedStars = results.Where(o => o.IsCorrect.GetValueOrDefault()).Count();
        //        this.EarnedPoints = results.Select(o => o.PointsRewarded.Value).Sum();
        //    }
        //}
        public MobileVGameSection(VGamePlayerCategory pcat, int playerid) //used to create versus game sections
        {
            this.SectionOrder = pcat.SectionOrder;
            this.SectionType = (this.SectionOrder>1?GameSectionType.VersusRegular:GameSectionType.VersusFirst);
            this.VGamePlayerCategoryId = pcat.VGamePlayerCategoryId;
            this.SectionName = pcat.VGameCategory.Category.CategoryName;
            this.Questions = new List<MobileQuestion>();
            
            ICollection<VGameCategorySectionQuestion> secQuestions = pcat.VGameCategory.VGameCategorySectionQuestions.ToList();
            foreach (VGameCategorySectionQuestion sq in secQuestions)
            {
                this.Questions.Add(new MobileQuestion(sq.Question,playerid));
            }

            this.NumberOfQuestions = secQuestions.Count();
            List<PlayerQuestionResult> results = (from e in secQuestions select e.Question).Select(x => x.PlayerQuestionResults.Where(o => o.PlayerId == playerid).FirstOrDefault()).Where(o => o != null).ToList();
            this.NumberAnswered = results.Count();
            if (results.Count() > 0)
            {
                this.NumberCorrect = results.Where(o => o.IsCorrect.GetValueOrDefault()).Count();
                this.EarnedStars = results.Where(o => o.IsCorrect.GetValueOrDefault()).Count();
                this.EarnedPoints = results.Select(o => o.PointsRewarded.Value).Sum();
            }
        }
    }

    public class MobileVGameCategory
    {
        public int VGameCategoryId { get; set; }
        public string CategoryName { get; set; }
        //public bool IsUsed { get; set; }
        public IList<MobileQuestion> Questions { get; set; }  //holds questions to build stage on device

        public MobileVGameCategory() { }
        public MobileVGameCategory(VGameCategory vcat, int playerid)
        {
            this.VGameCategoryId = vcat.VGameCategoryId;
            this.CategoryName = vcat.Category.CategoryName;
            //this.IsUsed = vcat.VGamePlayerCategories.Where(o => o.PlayerId == playerid).Any();
            this.Questions = new List<MobileQuestion>();
            ICollection<VGameCategorySectionQuestion> secQuestions = vcat.VGameCategorySectionQuestions.ToList();
            foreach (VGameCategorySectionQuestion sq in secQuestions)
            {
                this.Questions.Add(new MobileQuestion(sq.Question, playerid));
            }
        }
    }

    public class MobileVGameSelectedCategories
    {
        public int PlayerId { get; set; }
        public int VGameId { get; set; }
        public IList<MobileVGameCategory> SelectedCategories { get; set; }
    }

    public class MobileVGamePlayer
    {
        public int VGamePlayerId { get; set; }
        public string UserName { get; set; }
        public int GameSteps { get; set; }
        public int StepBank { get; set; }
        public DateTime LastStepUpdate { get; set; }
        public int Score { get; set; }
        public int Stage { get; set; }
        public int QuestionsAnswered { get; set; }

        public MobileVGamePlayer() { }
        public MobileVGamePlayer(VGamePlayer vgp)
        {
            this.VGamePlayerId = vgp.VGamePlayerId;
            this.UserName = vgp.Player.UserName;
            this.GameSteps = vgp.GameSteps;
            this.LastStepUpdate = vgp.LastStepUpdate;
            this.Score = vgp.Score;
            this.QuestionsAnswered = vgp.QuestionsAnswered;
        }
    }

    public class MobileVGamePlayerUpdate
    {
        public int VGameId { get; set; }
        public int PlayerId { get; set; }
        public int GameSteps { get; set; }
        public int StepBank { get; set; }
        public DateTime LastStepUpdate { get; set; }
    }
}