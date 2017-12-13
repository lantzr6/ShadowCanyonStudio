using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TriviaTraverseApi.Helpers;
using TriviaTraverseApi.Models;

namespace TriviaTraverseApi.Controllers
{
    public class CampaignController : ApiController
    {
        private TriviaTraverse171207Entities db = new TriviaTraverse171207Entities();

        [Authorize]
        [HttpGet]
        [Route("api/Campaign/Retrieve")]
        [ResponseType(typeof (MobileCampaign))]
        public IHttpActionResult GetMainCampaign(int playerid)
        {
            PlayerCampaign campaign = db.PlayerCampaigns.Where(o => o.PlayerId == playerid).FirstOrDefault();

            if (campaign != null)
            {

                MobileCampaign mCampaign = new MobileCampaign();
                mCampaign.PlayerCampaignId = campaign.PlayerCampaignId;
                mCampaign.Stages = new List<MobileCampaignStageCategory>();
                foreach (PlayerCampaignStageCategory PCSC in campaign.PlayerCampaignStageCategories)
                {
                    MobileCampaignStageCategory mCampaignStageCat = BuildNewStageCat(PCSC);
                    mCampaign.Stages.Add(mCampaignStageCat);
                }


                FillCampaignQueue(campaign, playerid);

                mCampaign.CategoryQueue = new List<MobileCampaignCategory>();
                foreach (PlayerCampaignCategoryQueue queue in campaign.PlayerCampaignCategoryQueues.Where(o => o.IsUsed == false).ToList())
                {
                    mCampaign.CategoryQueue.Add(new MobileCampaignCategory() { CampaignCategoryId = queue.CampaignCategoryId, CategoryName = queue.CampaignCategory.CategoryName });
                }

                return Ok(mCampaign);
            }
            else
            {
                return NotFound();
            }
        }


        public MobileCampaignStageCategory BuildNewStageCat(PlayerCampaignStageCategory PCSC)
        {
            int playerid = PCSC.PlayerCampaign.PlayerId;
            int playercampaignid = PCSC.PlayerCampaign.PlayerCampaignId;

            int sectionQuestionOrder = 1;

            MobileCampaignStageCategory mCampaignStageCat = new MobileCampaignStageCategory();
            mCampaignStageCat.CampaignStageCategoryId = PCSC.PlayerCampaignStageCategoryId;
            mCampaignStageCat.StageLevel = PCSC.CampaignStage.StageLevel;
            mCampaignStageCat.StarsRequired = PCSC.CampaignStage.StarsRequired;
            mCampaignStageCat.Sections = new List<MobileCampaignSection>();

            //consolidate this with function and section in StartController
            foreach (CampaignSection cs in PCSC.CampaignCategory.CampaignSections)
            {
                MobileCampaignSection mSection = new MobileCampaignSection() { CampaignSectionId = cs.CampaignSectionId, SectionName = cs.SectionName};
                mSection.SectionType = (PCSC.CampaignStage.StageLevel > 0 ? GameSectionType.Campaign : GameSectionType.CampaignTutorial);

                ICollection<PlayerCampaignSectionQuestion> secQuestions = cs.PlayerCampaignSectionQuestions.Where(o => o.PlayerCampaignId == playercampaignid).ToList();

                for (int i = 1; i < 6; i++)
                {
                    int numQuestions = (i == 2 || i == 3 ? 2 : 1);
                    //PlayerCampaignSectionQuestion levelQ = (from a in secQuestions.Where(o => o.SectionQuestionOrder == sectionQuestionOrder && o.Question.QuestionLevel == i) select a).OrderByDescending(o => o.CreatedAt).FirstOrDefault();
                    //if (levelQ == null)
                    //{
                        var list = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == cs.CampaignSectionId).FirstOrDefault().Questions.Where(o => o.CampaignSectionQuestionOrder == sectionQuestionOrder && o.QuestionLevel == i)
                                    join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerid) on q.QuestionId equals r.QuestionId into g
                                    from j in g.DefaultIfEmpty()
                                    select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).Take(numQuestions).ToList();
                    for (int x = numQuestions - 1; x >= 0; x--)
                    {
                        PlayerCampaignSectionQuestion levelQ = new PlayerCampaignSectionQuestion()
                        {
                            PlayerCampaignId = playercampaignid,
                            CampaignSectionId = cs.CampaignSectionId,
                            SectionQuestionOrder = sectionQuestionOrder,
                            QuestionId = list[x].RowA.QuestionId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            Deleted = false
                        };
                        db.PlayerCampaignSectionQuestions.Add(levelQ);
                    }
                    db.SaveChanges();
                    //}
                }
                mSection.NumberOfQuestions = 5;// secQuestions.Count();

                List<PlayerQuestionResult> results = (from e in secQuestions select e.Question).Select(x => x.PlayerQuestionResults.Where(o => o.PlayerId == playerid).FirstOrDefault()).Where(o => o != null).ToList();
                mSection.NumberAnswered = results.Count();
                if (results.Count() > 0)
                {
                    mSection.NumberCorrect = results.Where(o => o.IsCorrect.GetValueOrDefault()).Count();
                    mSection.EarnedStars = results.Where(o => o.IsCorrect.GetValueOrDefault()).Count();
                    mSection.EarnedPoints = results.Select(o => o.PointsRewarded.Value).Sum();
                }

                mCampaignStageCat.Sections.Add(mSection);
            }

            return mCampaignStageCat;
        }

        [Authorize]
        [HttpPost]
        [Route("api/Campaign/PostNextStage")]
        [ResponseType(typeof (MobileNewCampaignStageReturn))]
        public IHttpActionResult PostNextCampaignStage(MobileNewCampaignStageInfo objIn)  //PlayerCampaignId, CampaignCategoryId, StageLevel
        {
            PlayerCampaign campaign = db.PlayerCampaigns.Where(o => o.PlayerCampaignId == objIn.PlayerCampaignId).FirstOrDefault();
            CampaignCategory campaignCat = db.CampaignCategories.Where(o => o.CampaignCategoryId == objIn.CampaignCategoryId).FirstOrDefault();
            CampaignStage campaignStage = db.CampaignStages.Where(o => o.StageLevel == objIn.StageLevel).FirstOrDefault();

            PlayerCampaignStageCategory newObj = new PlayerCampaignStageCategory() { PlayerCampaign = campaign, CampaignCategory = campaignCat, CampaignStage = campaignStage };
            db.PlayerCampaignStageCategories.Add(newObj);

            PlayerCampaignCategoryQueue usedObj = db.PlayerCampaignCategoryQueues.Where(o => o.PlayerCampaignId == objIn.PlayerCampaignId && o.CampaignCategoryId == objIn.CampaignCategoryId).FirstOrDefault();
            usedObj.IsUsed = true;

            db.SaveChanges();

            MobileNewCampaignStageReturn objOut = new MobileNewCampaignStageReturn();
            objOut.NewStage = BuildNewStageCat(newObj);

            FillCampaignQueue(campaign, newObj.PlayerCampaign.PlayerId);

            List<MobileCampaignCategory> newCategoryQueue = new List<MobileCampaignCategory>();
            foreach (PlayerCampaignCategoryQueue queue in campaign.PlayerCampaignCategoryQueues.Where(o => o.IsUsed == false).ToList())
            {
                newCategoryQueue.Add(new MobileCampaignCategory() { CampaignCategoryId = queue.CampaignCategoryId, CategoryName = queue.CampaignCategory.CategoryName });
            }

            objOut.CategoryQueue = newCategoryQueue;

            return Ok(objOut);
        }

        private void FillCampaignQueue(PlayerCampaign campaign, int playerid)
        {
            if (campaign.PlayerCampaignCategoryQueues.Where(o => o.IsUsed == false).Count() < 3)
            {
                var list = (from ee in db.CampaignCategories orderby ee.QueueLevel
                            where !db.PlayerCampaignCategoryQueues.Where(o => o.PlayerCampaign.PlayerId == playerid).Any(o => o.CampaignCategory.CampaignCategoryId == ee.CampaignCategoryId)
                            select ee).ToList();
                if (list.Count > 0)  //add more to the queue unless all campaign categories are added
                {
                    int idx = 0;
                    do
                    {
                        PlayerCampaignCategoryQueue queue = new PlayerCampaignCategoryQueue()
                        {
                            CampaignCategoryId = list[idx].CampaignCategoryId
                            ,
                            PlayerCampaignId = campaign.PlayerCampaignId
                            ,
                            CreatedAt = DateTime.Now
                            ,
                            UpdateAt = DateTime.Now
                            ,
                            Deleted = false
                        };
                        campaign.PlayerCampaignCategoryQueues.Add(queue);
                        idx++;
                    } while (campaign.PlayerCampaignCategoryQueues.Where(o => o.IsUsed == false).Count() < 3);
                    db.SaveChanges();
                }
            }
        }


    }
}
