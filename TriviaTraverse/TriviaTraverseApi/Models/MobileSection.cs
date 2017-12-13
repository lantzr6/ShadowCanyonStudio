using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaTraverseApi.Models
{


    public enum GameSectionType
    {
        Campaign,
        CampaignTutorial,
        VersusFirst,
        VersusRegular
    }


    public class MobileDashboard
    {
        public IList<MobileCampaign> Campaigns { get; set; }
        public IList<MobileVGame> VGames { get; set; }
    }

    public class MobileCampaign
    {
        public int PlayerCampaignId { get; set; }
        public IList<MobileCampaignStageCategory> Stages { get; set; }
        public IList<MobileCampaignCategory> CategoryQueue { get; set; }

        public MobileCampaign() { }
        public MobileCampaign(PlayerCampaign pc)
        {
            this.PlayerCampaignId = pc.PlayerCampaignId;
        }
    }

    public class MobileCampaignStageCategory
    {
        public MobileCampaignStageCategory() { }

        public int CampaignStageCategoryId { get; set; }
        public int StageLevel { get; set; }
        public int StarsRequired { get; set; }
        public IList<MobileCampaignSection> Sections { get; set; }
    }

    public class MobileCampaignSection
    {
        public int CampaignSectionId { get; set; }
        public string SectionName { get; set; }
        public IList<MobileQuestion> Questions { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberAnswered { get; set; }
        public int NumberCorrect { get; set; }
        public int EarnedStars { get; set; }
        public int EarnedPoints { get; set; }
        public GameSectionType SectionType { get; set; }
    }

    public class MobileNewCampaignStageInfo
    {
        public int PlayerCampaignId { get; set; }
        public int CampaignCategoryId { get; set; }
        public int StageLevel { get; set; }
    }
    public class MobileNewCampaignStageReturn
    {
        public MobileCampaignStageCategory NewStage { get; set; }
        public IList<MobileCampaignCategory> CategoryQueue { get; set; }
    }

    public class MobileCampaignCategory
    {
        public int CampaignCategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class MobileQuestion
    {
        public MobileQuestion() { }
        public MobileQuestion(Question SCQ, int playerid)
        {
            this.QuestionId = SCQ.QuestionId;
            this.CategoryName = SCQ.Category.CategoryName;
            this.Text = SCQ.Text.Trim();
            this.QuestionLevel = SCQ.QuestionLevel;
            this.QuestionType = SCQ.QuestionType.Description.Trim();
            this.HasImage = SCQ.HasImage;
            string imageUrl = SCQ.ImageUrl;
            if (imageUrl == null)
            {
                imageUrl = "";
            }
            else
            {
                imageUrl = imageUrl.Trim();
            }
            this.ImageUrl = imageUrl;
            this.AnswerCorrect = SCQ.AnswerCorrect.Trim();
            this.AnswerWrong1 = SCQ.AnswerWrong1.Trim();
            this.AnswerWrong2 = SCQ.AnswerWrong2.Trim();
            this.AnswerWrong3 = SCQ.AnswerWrong3.Trim();

            PlayerQuestionResult results = SCQ.PlayerQuestionResults.Where(o => o.PlayerId == playerid).FirstOrDefault();
            this.IsUnanswered = (results == null);
        }
        public int QuestionId { get; set; }
        public string CategoryName { get; set; }
        public string Text { get; set; }
        public int QuestionLevel { get; set; }
        public string QuestionType { get; set; }
        public bool IsMultiChoice
        {
            get
            {
                return (QuestionType.Trim() == "MultipleChoice");
            }
        }
        public bool? HasImage { get; set; }
        public string ImageUrl { get; set; }
        public string AnswerCorrect { get; set; }
        public string AnswerWrong1 { get; set; }
        public string AnswerWrong2 { get; set; }
        public string AnswerWrong3 { get; set; }
        public string PlayerAnswer { get; set; }
        public int PointsRewarded { get; set; }
        public bool IsUnanswered { get; set; }
    }

    public class MobilePlayerQuestionResult
    {
        public int PlayerQuestionResultId { get; set; }
        public int PlayerId { get; set; }
        public int QuestionId { get; set; }
        public string PlayerAnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsRewarded { get; set; }
    }

    public class MobilePlayerVGameQuestionResult
    {
        public int PlayerQuestionResultId { get; set; }
        public int PlayerId { get; set; }
        public int QuestionId { get; set; }
        public string PlayerAnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsRewarded { get; set; }
        public int VGameId { get; set; }
    }

}