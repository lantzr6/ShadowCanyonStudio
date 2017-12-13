using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TriviaTraverseApi.Models;
using TriviaTraverseApi.Helpers;

namespace TriviaTraverseApi.Controllers
{
    public class GameController : ApiController
    {
        private TriviaTraverse171207Entities db = new TriviaTraverse171207Entities();

        [Authorize]
        [HttpGet]
        [Route("api/Game/JoinNewGame")]
        [ResponseType(typeof(MobileVGame))]
        public IHttpActionResult JoinNewGame(int playerid, string gameType, string gameCap, bool gameAuto)
        {
            MobileVGame retval = null;

            DateTime now = DateTime.Now;
            VGame vGame;
            int stepCapValue = 15000;
            switch (gameCap)
            {
                case "5K":
                    stepCapValue = 5000;
                    break;
                case "8K":
                    stepCapValue = 8000;
                    break;
                case "10K":
                    stepCapValue = 10000;
                    break;
            }
            //check there is an open game
            List<VGame> openGames = (from vg in db.VGames
                                     where vg.StartTime > now
                                     && vg.PlayerMax > vg.VGamePlayers.Count()
                                     && vg.StepCap == stepCapValue
                                     && !vg.IsPrivate
                                     && vg.GameLength == 24
                                     && (vg.VGamePlayers.Where(o => o.PlayerId == playerid).Any() == false)
                                     select vg).ToList();
            if (openGames.Any())
            {
                vGame = openGames.FirstOrDefault();
                vGame.VGamePlayers.Add(new VGamePlayer() { VGameId = vGame.VGameId, PlayerId = playerid, AutoRenew = gameAuto, CreatedAt = now, UpdatedAt = now, Deleted = false });
                db.SaveChanges();
            }
            else
            {
                List<VGameName> vGNames = db.VGameNames.ToList();
                vGNames.Shuffle();

                var currentNames = (from n in vGNames
                                    join vg in db.VGames on n.Name equals vg.GameName into g
                                    select new { name = n.Name, count = g.Count() }
                                    ).OrderBy(o => o.count).FirstOrDefault();

                string name = string.Format("{0}{1}", currentNames.name, currentNames.count > 0 ? (currentNames.count + 1).ToString() : "");


                vGame = new VGame()
                {
                    GameName = name,
                    GameTypeId = 1, //Versus Game
                    GameLength = 24,
                    IsPrivate = (gameType == "Private" ? true : false),
                    StepCap = stepCapValue,
                    PlayerMax = 5,
                    StartTime = now,
                    EndTime = now.AddHours(24),
                    CreatedAt = now,
                    UpdatedAt = now,
                    Deleted = false
                };
                vGame.VGamePlayers.Add(new VGamePlayer() { VGameId = vGame.VGameId, PlayerId = playerid, AutoRenew = gameAuto, LastStepUpdate = now, CreatedAt = now, UpdatedAt = now, Deleted = false });
                db.VGames.Add(vGame);
                db.SaveChanges();
            }
            List<int> playerIds = vGame.VGamePlayers.Select(o => o.PlayerId).ToList();
            for (int i = 1; i < 6; i++)
            {
                int numQuestions = (i == 2 || i == 3 ? 2 : 1);
                var listQ = (from q in db.Questions.Where(o => o.QuestionLevel == i && o.CampaignSectionId == null).ToList()
                             join r in db.PlayerQuestionResults.Where(o => o.PlayerId == playerid) on q.QuestionId equals r.QuestionId into g
                             from j in g.DefaultIfEmpty()
                             select new { RowA = q, LastUsed = j?.CreatedAt ?? null }).OrderBy(o => o.LastUsed).OrderBy(r => Guid.NewGuid()).Take(numQuestions).ToList();
                foreach (var q in listQ)
                {
                    VGamePlayerSectionQuestion levelQ = new VGamePlayerSectionQuestion
                    {
                        VGameId = vGame.VGameId,
                        PlayerId = playerid,
                        QuestionId = q.RowA.QuestionId,
                        CreatedAt = now,
                        UpdateAt = now,
                        Deleted = false
                    };
                    db.VGamePlayerSectionQuestions.Add(levelQ);
                }
             }
            db.SaveChanges();

            //refresh vGame
            vGame = db.VGames.Include(c=>c.GameType).Include(c => c.VGamePlayers.Select(d=>d.Player)).Where(o => o.VGameId == vGame.VGameId).FirstOrDefault();

            retval = new MobileVGame(vGame, playerid);

            return Ok(retval);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Game/UpdateVGamePlayer")]
        [ResponseType(typeof(MobileVGame))]
        public IHttpActionResult PostUpdateVGamePlayer(MobileVGamePlayerUpdate inObj)
        {
            MobileVGame retval = null;

            VGamePlayer vgp = db.VGamePlayers.Where(o => o.VGameId == inObj.VGameId && o.PlayerId == inObj.PlayerId).FirstOrDefault();
            vgp.GameSteps = inObj.GameSteps;
            vgp.LastStepUpdate = inObj.LastStepUpdate;
            vgp.UpdatedAt = DateTime.Now;
            db.SaveChanges();

            //refresh vGame
            VGame vGame = db.VGames.Include(c => c.GameType).Include(c => c.VGamePlayers.Select(d => d.Player)).Where(o => o.VGameId == inObj.VGameId).FirstOrDefault();
            retval = new MobileVGame(vGame, inObj.PlayerId);
            return Ok(retval);
        }

        [Authorize]
        [HttpGet]
        [Route("api/Game/GetGame")]
        [ResponseType(typeof(MobileVGame))]
        public IHttpActionResult GetGame(int vgameid, int playerid)
        {
            MobileVGame retval = null;

            VGame vGame = db.VGames.Where(o => o.VGameId == vgameid).FirstOrDefault();

            retval = new MobileVGame(vGame, playerid);

            return Ok(retval);
        }

        [Authorize]
        [HttpGet]
        [Route("api/Game/StartGame")]
        [ResponseType(typeof(MobileVGame))]
        public IHttpActionResult StartGame(int vgameid, int playerid)
        {
            MobileVGame retval = null;

            DateTime now = DateTime.Now;
            VGame vGame = db.VGames.Where(o => o.VGameId == vgameid).FirstOrDefault();
            int catMax = (vGame.StepCap == 15000 ? 4 : 3);

            if (vGame.VGameCategories.Count() == 0)
            {
                //add random categories
                List<Category> cats = db.Categories.OrderBy(r => Guid.NewGuid()).Take(catMax).ToList();
                foreach (Category c in cats)
                {
                    VGameCategory vcat = new VGameCategory
                    {
                        CategoryId = c.CategoryId,
                        VGameId = vGame.VGameId,
                        CreatedAt = now,
                        UpdatedAt = now,
                        Deleted = false
                    };
                    db.VGameCategories.Add(vcat);
                    db.SaveChanges(); //save to get id

                    //build Category Section Questions
                    Category cat = vcat.Category;
                    List<int> playerIds = vGame.VGamePlayers.Select(o => o.PlayerId).ToList();
                    for (int i = 1; i < 6; i++)
                    {
                        int numQuestions = (i == 2 || i == 3 ? 2 : 1);
                        var listQ = (from q in cat.Questions.Where(o => o.QuestionLevel == i && o.CampaignSectionId == null).ToList()
                                     join r in db.PlayerQuestionResults.Where(o => playerIds.Contains(o.PlayerId)) on q.QuestionId equals r.QuestionId into g1
                                     from j1 in g1.DefaultIfEmpty()
                                     join l in db.VGamePlayerSectionQuestions.Where(o => playerIds.Contains(o.PlayerId) && o.VGameId == vgameid) on q.QuestionId equals l.QuestionId into g2
                                     from j2 in g2.DefaultIfEmpty()
                                     select new { RowA = q, LastUsed = (j1?.CreatedAt ?? (j2?.CreatedAt ?? null)) }).OrderBy(o => o.LastUsed).OrderBy(r => Guid.NewGuid()).Take(numQuestions).ToList();
                        for (int x = numQuestions - 1; x >= 0; x--)
                        {
                            VGameCategorySectionQuestion levelQ = new VGameCategorySectionQuestion
                            {
                                VGameCategoryId = vcat.VGameCategoryId,
                                QuestionId = listQ[x].RowA.QuestionId,
                                CreatedAt = now,
                                UpdatedAt = now,
                                Deleted = false
                            };
                            db.VGameCategorySectionQuestions.Add(levelQ);
                        }
                    }
                    db.SaveChanges();
                }
            }

            vGame = db.VGames.Where(o => o.VGameId == vgameid).FirstOrDefault();
            retval = new MobileVGame(vGame, playerid);

            return Ok(retval);
        }

        [Authorize]
        [HttpPost]
        [Route("api/Game/PostSelectedCategories")]
        [ResponseType(typeof(MobileVGame))]
        public IHttpActionResult PostSelectedCategories(MobileVGameSelectedCategories objIn)
        {
            MobileVGame retval = null;
            DateTime now = DateTime.Now;

            foreach (MobileVGameCategory vcat in objIn.SelectedCategories)
            {
                VGamePlayerCategory playerVCat = new VGamePlayerCategory()
                {
                    PlayerId = objIn.PlayerId,
                    VGameCategoryId = vcat.VGameCategoryId,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Deleted = false
                };
                db.VGamePlayerCategories.Add(playerVCat);
            }
            db.SaveChanges();

            VGame vGame = db.VGames.Where(o => o.VGameId == objIn.VGameId).FirstOrDefault();
            retval = new MobileVGame(vGame, objIn.PlayerId);

            return Ok(retval);
        }

    }
}
