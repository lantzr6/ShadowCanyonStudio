using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriviaTraverseApi.Models
{
    public class MobilePlayer
    {
        public int PlayerId { get; set; }
        public string UserName { get; set; }
        public string EmailAddr { get; set; }
        public bool FbLogin { get; set; }
        public string Password { get; set; }
        public int PlayerLevel { get; set; }
        public int CurrentSteps { get; set; }
        public int StepBank { get; set; }
        public DateTime LastStepUpdate { get; set; }
        public int Coins { get; set; }
        public int Stars { get; set; }
        public int Points { get; set; }
    }

    public class MobileTutorialMessagesStatus
    {
        public bool QuestionStatusOne { get; set; }
        public bool QuestionStatusTwo { get; set; }

        public bool CampaignSectionNewlyComplete { get; set; }
        public bool CampaignStageNewlyUnlocked { get; set; }
        public bool CampaignStageBonus { get; set; }
    }
}