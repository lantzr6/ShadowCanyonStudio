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
    public class StartController : ApiController
    {
        private TriviaTraverse171207Entities db = new TriviaTraverse171207Entities();

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Start/NewAccount")]
        [ResponseType(typeof(MobilePlayer))]
        public IHttpActionResult NewAccount(MobilePlayer inObj)
        {
            Player player = null;
            if (inObj == null)
            {
                player = CreateTempPlayer("", "", "");
            }
            else
            {
                player = CreateTempPlayer(inObj.UserName, inObj.EmailAddr, inObj.Password, inObj.PlayerLevel);
            }

            MobilePlayer mPlayer = new MobilePlayer();
            mPlayer.PlayerId = player.PlayerId;
            mPlayer.UserName = player.UserName;
            mPlayer.EmailAddr = player.EmailAddr;
            mPlayer.Password = player.Password;  //temp password
            mPlayer.PlayerLevel = player.PlayerLevel;

            //int tutorialSectionId = db.CampaignCategories.Where(o => o.IsTutorial && !o.Deleted).FirstOrDefault().CampaignSections.Where(o => !o.Deleted).FirstOrDefault().CampaignSectionId;
            //int playerCampaignId = db.PlayerCampaigns.Where(o => o.PlayerId == player.PlayerId).FirstOrDefault().PlayerCampaignId; //there is only one during the tutorial

            //MobileCampaignSection MS = GetSectionObject(tutorialSectionId, player.PlayerId, playerCampaignId);
            //MS.SectionType = GameSectionType.CampaignTutorial;

            return Ok(mPlayer);
        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("api/Start/NewPlayer")]
        //[ResponseType(typeof(MobilePlayer))]
        //public IHttpActionResult GetCreatePlayer()
        //{
        //    Player player = CreateTempPlayer("","","");

        //    MobilePlayer mPlayer = new MobilePlayer();
        //    mPlayer.PlayerId = player.PlayerId;
        //    mPlayer.UserName = player.UserName;
        //    mPlayer.EmailAddr = player.EmailAddr;
        //    mPlayer.Password = player.Password;  //temp password
        //    mPlayer.PlayerLevel = player.PlayerLevel;

        //    return Ok(mPlayer);
        //}

        [Authorize]
        [HttpGet]
        [Route("api/Start/GetTutorial")]
        [ResponseType(typeof(MobileCampaignSection))]
        public IHttpActionResult GetTutorial(int playerId)
        {
            int tutorialSectionId = db.CampaignCategories.Where(o => o.IsTutorial && !o.Deleted).FirstOrDefault().CampaignSections.Where(o => !o.Deleted).FirstOrDefault().CampaignSectionId;
            int playerCampaignId = db.PlayerCampaigns.Where(o => o.PlayerId == playerId).FirstOrDefault().PlayerCampaignId; //there is only one during the tutorial

            MobileCampaignSection MS = GetSectionObject(tutorialSectionId, playerId, playerCampaignId);
            MS.SectionType = GameSectionType.CampaignTutorial;

            return Ok(MS);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Start/UpdateTutorialMessageStatus")]
        [ResponseType(typeof(string))]
        public string PostSignUp(MobileTutorialMessagesStatus inObj)
        { 
            PlayerTutorialMessageStatus tutorial = db.PlayerTutorialMessageStatuses.Where(o => o.PlayerId == 1).FirstOrDefault();

            if (tutorial == null)
            {
                tutorial = new PlayerTutorialMessageStatus();
                tutorial.CreatedAt = DateTime.Now;
                tutorial.Deleted = false;
                db.PlayerTutorialMessageStatuses.Add(tutorial);
            }
            tutorial.QuestionStatusOne = inObj.QuestionStatusOne;
            tutorial.QuestionStatusTwo = inObj.QuestionStatusTwo;
            tutorial.CampaignSectionNewlyComlplete = inObj.CampaignSectionNewlyComplete;
            tutorial.CampaignStageNewlyUnlocked = inObj.CampaignSectionNewlyComplete;
            tutorial.CampaignStageBonus = inObj.CampaignStageBonus;
            tutorial.UpdatedAt = DateTime.Now;

            db.SaveChanges();

            return "Ok";
        }


        [Authorize]
        [HttpGet]
        [Route("api/Start/GetSection")]
        [ResponseType(typeof(MobileCampaignSection))]
        public IHttpActionResult GetSection(int id, int player, int pcId, bool retry)
        {
            MobileCampaignSection MS = GetSectionObject(id, player, pcId, retry);
            MS.SectionType = GameSectionType.Campaign;

            return Ok(MS);
        }



        private Player CreateTempPlayer(string username, string email, string password, int level=0)
        {
            string name = "";
            string email2 = "";
            string password2 = "";
            int playerLevel = level;
            if (string.IsNullOrEmpty(username))
            {
                do
                {
                    name = GetUniqueTempName((level==0?"Temp":"Guest"));
                } while (name == "");
                email2 = name + "@temp.com";
                password2 = name;
            }
            else
            {
                name = username;
                email2 = email;
                password2 = password;
                playerLevel = level;
            }

            Player player = new Player();
            player.UserName = name;
            player.EmailAddr = email2;
            player.Password = password2;
            player.PlayerLevel = playerLevel;
            player.LastStepUpdate = DateTime.Today;
            player.CreatedAt = DateTime.Now;
            player.UpdatedAt = DateTime.Now;
            player.Deleted = false;

            db.Players.Add(player);
            db.SaveChanges();

            player = db.Players.Where(o => o.UserName == name).FirstOrDefault();

            //setup main campaign with tutorial
            CampaignCategory campaignCat = db.CampaignCategories.Where(o => o.IsTutorial).FirstOrDefault();
            CampaignStage campaignStage = db.CampaignStages.Where(o => o.StageLevel == 0).FirstOrDefault();

            PlayerCampaign playerCampaign = new PlayerCampaign();
            playerCampaign.PlayerId = player.PlayerId;

            
            PlayerCampaignStageCategory newObj = new PlayerCampaignStageCategory() { PlayerCampaign = playerCampaign, CampaignCategory = campaignCat, CampaignStage = campaignStage };
            db.PlayerCampaignStageCategories.Add(newObj);
            db.SaveChanges();

            BuildNewStageCat(newObj);

            //playerCampaign.PlayerCampaignStageCategories.Add(new PlayerCampaignStageCategory() { CampaignCategory = campaignCat, CampaignStage = campaignStage });
            //db.PlayerCampaigns.Add(playerCampaign);
            //db.SaveChanges();

            PlayerCampaignCategoryQueue queue = new PlayerCampaignCategoryQueue() { CampaignCategoryId = campaignCat.CampaignCategoryId, PlayerCampaignId = playerCampaign.PlayerCampaignId,
                                                                                           IsUsed = true, CreatedAt = DateTime.Now, UpdateAt = DateTime.Now, Deleted = false };
            db.PlayerCampaignCategoryQueues.Add(queue);
            db.SaveChanges();

            return player;
        }

        public void BuildNewStageCat(PlayerCampaignStageCategory PCSC)
        {
            int playerid = PCSC.PlayerCampaign.PlayerId;
            int playercampaignid = PCSC.PlayerCampaign.PlayerCampaignId;

            int sectionQuestionOrder = 1;

            foreach (CampaignSection cs in PCSC.CampaignCategory.CampaignSections)
            {

                ICollection<PlayerCampaignSectionQuestion> secQuestions = cs.PlayerCampaignSectionQuestions.Where(o=>o.PlayerCampaignId==playercampaignid).ToList();

                for (int i = 1; i < 6; i++)
                {
                    PlayerCampaignSectionQuestion levelQ = (from a in secQuestions.Where(o => o.SectionQuestionOrder == sectionQuestionOrder && o.Question.QuestionLevel == i) select a).OrderByDescending(o => o.CreatedAt).FirstOrDefault();
                    if (levelQ == null)
                    {
                        var list = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == cs.CampaignSectionId).FirstOrDefault().Questions.Where(o => o.CampaignSectionQuestionOrder == sectionQuestionOrder && o.QuestionLevel == i)
                                    join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerid) on q.QuestionId equals r.QuestionId into g
                                    from j in g.DefaultIfEmpty()
                                    select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).ToList();
                        levelQ = new PlayerCampaignSectionQuestion()
                        {
                            PlayerCampaignId = playercampaignid,
                            CampaignSectionId = cs.CampaignSectionId,
                            SectionQuestionOrder = sectionQuestionOrder,
                            QuestionId = list[0].RowA.QuestionId
                            ,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            Deleted = false
                        };
                        db.PlayerCampaignSectionQuestions.Add(levelQ);
                        db.SaveChanges();
                    }
                }
            }
        }

        private string GetUniqueTempName(string prefix)
        {
            string name = prefix + DateTime.Now.Millisecond.ToString();
            if (db.Players.Where(o => o.UserName == name).Any())
            {
                return "";
            }
            else
            {
                return name;
            }
        }

        private MobileCampaignSection GetSectionObject(int id, int playerid, int pcId,bool retry = false)
        {
            int sectionQuestionOrder = (retry ? 2 : 1);
            if (retry)
            {
                CampaignSection cs = db.CampaignSections.Where(o => o.CampaignSectionId == id).FirstOrDefault();
                ICollection<PlayerCampaignSectionQuestion> secQuestions = cs.PlayerCampaignSectionQuestions.Where(o => o.PlayerCampaignId == playerid).ToList();

                for (int i = 1; i < 6; i++)
                {
                    int numQuestions = (i == 2 || i == 3 ? 2 : 1);
                    var list = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == cs.CampaignSectionId).FirstOrDefault().Questions.Where(o => o.CampaignSectionQuestionOrder == sectionQuestionOrder && o.QuestionLevel == i)
                                join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerid) on q.QuestionId equals r.QuestionId into g
                                from j in g.DefaultIfEmpty()
                                select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).Take(numQuestions).ToList();
                    for (int x = numQuestions - 1; x >= 0; x--)
                    {
                        PlayerCampaignSectionQuestion levelQ = new PlayerCampaignSectionQuestion()
                            {
                                PlayerCampaignId = pcId,
                                CampaignSectionId = cs.CampaignSectionId,
                                SectionQuestionOrder = sectionQuestionOrder,
                                QuestionId = list[0].RowA.QuestionId
                                ,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                Deleted = false
                            };
                    db.PlayerCampaignSectionQuestions.Add(levelQ);
                    }
                    db.SaveChanges();
                }
            }

            MobileCampaignSection MS = new MobileCampaignSection();
            List<MobileQuestion> MSQuestions = new List<MobileQuestion>();

            //loop through levels to get last added CampaignSectionQuestion
            for (int i = 1; i < 6; i++)
            {
                int numQuestions = (i == 2 || i == 3 ? 2 : 1);
                List<PlayerCampaignSectionQuestion> PCSQ = db.PlayerCampaignSectionQuestions.Where(o => o.CampaignSectionId == id && o.PlayerCampaignId == pcId && o.SectionQuestionOrder == sectionQuestionOrder && o.Question.QuestionLevel == i)
                    .OrderByDescending(o => o.CreatedAt).Take(numQuestions).ToList();
                for (int x = numQuestions - 1; x >= 0; x--)
                {
                    MSQuestions.Add(new MobileQuestion(PCSQ[x].Question, playerid));
                }
            }


            //var list1 = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == id).FirstOrDefault().Questions.Where(o => o.QuestionLevel == 1)
            //             join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerId) on q.QuestionId equals r.QuestionId into g
            //             from j in g.DefaultIfEmpty()
            //             select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).ToList();
            ////list1.Shuffle();
            //MSQuestions.Add(new MobileQuestion(list1[0].RowA));
            //var list2 = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == id).FirstOrDefault().Questions.Where(o => o.QuestionLevel == 2)
            //             join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerId) on q.QuestionId equals r.QuestionId into g
            //             from j in g.DefaultIfEmpty()
            //             select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).ToList();
            ////list1.Shuffle();
            //MSQuestions.Add(new MobileQuestion(list2[0].RowA));
            //var list3 = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == id).FirstOrDefault().Questions.Where(o => o.QuestionLevel == 3)
            //             join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerId) on q.QuestionId equals r.QuestionId into g
            //             from j in g.DefaultIfEmpty()
            //             select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).ToList();
            ////list1.Shuffle();
            //MSQuestions.Add(new MobileQuestion(list3[0].RowA));
            //var list4 = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == id).FirstOrDefault().Questions.Where(o => o.QuestionLevel == 4)
            //             join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerId) on q.QuestionId equals r.QuestionId into g
            //             from j in g.DefaultIfEmpty()
            //             select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).ToList();
            ////list1.Shuffle();
            //MSQuestions.Add(new MobileQuestion(list4[0].RowA));
            //var list5 = (from q in db.CampaignSections.Where(o => o.CampaignSectionId == id).FirstOrDefault().Questions.Where(o => o.QuestionLevel == 5)
            //             join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerId) on q.QuestionId equals r.QuestionId into g
            //             from j in g.DefaultIfEmpty()
            //             select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).ToList();
            ////list1.Shuffle();
            //MSQuestions.Add(new MobileQuestion(list5[0].RowA));


            //foreach (Question q in questions)
            //{
            //    MobileQuestion msq = new MobileQuestion(q);
            //    MSQuestions.Add(msq);
            //}
            MS.Questions = MSQuestions;
            //List<MobileSectionCategoryQuestionResult> MSQuestionResults = new List<MobileSectionCategoryQuestionResult>();
            //foreach (SectionCategoryQuestion q in questions)
            //{
            //    MobileSectionQuestion msq = new MobileSectionQuestion();
            //    msq.CategoryName = q.Question.Category.CategoryName;
            //    msq.SectionCategoryQuestion = new MobileSectionCategoryQuestion(q);
            //    MSQuestions.Add(msq);
            //}
            //MS.SectionQuestions = MSQuestions;

            return MS;
        }
    }
}
