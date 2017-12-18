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
    public class JobController : ApiController
    {

        private TriviaTraverse171207Entities db = new TriviaTraverse171207Entities();

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Job/AddSteps")]
        [ResponseType(typeof(string))]
        public IHttpActionResult AddSteps()
        {
            List<Player> botPlayers = db.Players.Where(o => o.IsBot).ToList();
            List<BotInfo> bots = db.BotInfoes.ToList();

            foreach (BotInfo pb in bots)
            {
                DateTime now = DateTime.Now;

                int botAcc = pb.AccurancyRate;
                int botOpenChance = pb.OpenRate;
                double botStepGoalChance = pb.StepGoalPercent;
                int stepGoalVariance = 10;

                //open chance
                Random rO = new Random(now.Millisecond);
                int rResult = rO.Next(1, 100);
                if (rResult <= botOpenChance)
                {
                    //bp.StepBank += 100;
                    List<VGame> vGames = (from vg in db.VGames.Where(o=>o.EndTime>now)
                                          join vgp in db.VGamePlayers.Include("Player").Where(o => o.PlayerId == pb.Player.PlayerId) on vg.VGameId equals vgp.VGameId
                                          select vg).ToList();
                    foreach (VGame vGame in vGames)
                    {
                        //get percentage of game progress
                        DateTime start = vGame.StartTime;
                        DateTime end = vGame.EndTime;
                        double total = (end - start).TotalSeconds;

                        double percentage = (now - start).TotalSeconds * 100 / total;

                        //get bots total step value
                        int gStepCap = vGame.StepCap;
                        Random r = new Random(now.Second);
                        double rDouble = r.NextDouble() * stepGoalVariance; //step goal variance
                        int tSteps = Convert.ToInt32(gStepCap * ((botStepGoalChance /100) + (rDouble /100)));
                        //if (tSteps > gStepCap) { tSteps = gStepCap; }

                        int stepProgress = Convert.ToInt32(tSteps * (percentage/100));
                        int stepCost = 1000;
                        switch (vGame.StepCap)
                        {
                            case 5000:
                                stepCost = 500;
                                break;
                            case 8000:
                                stepCost = 800;
                                break;
                        }

                        VGamePlayer vgp = vGame.VGamePlayers.Where(o => o.PlayerId == pb.Player.PlayerId).FirstOrDefault();
                        if (vgp.QuestionsAnswered == 0)
                        {
                            while (vgp.QuestionsAnswered < 5)
                            {
                                aaa(vgp, botAcc, 0);
                            }
                            vgp.LastStepUpdate = now;
                            db.SaveChanges();
                        }
                        if (vgp.GameSteps < stepProgress)
                        {
                            vgp.StepBank += stepProgress - vgp.GameSteps; //add the new steps
                            vgp.GameSteps = stepProgress;
                            while (vgp.StepBank >= stepCost)
                            {
                                aaa(vgp, botAcc, stepCost);
                            }
                            vgp.LastStepUpdate = now;
                            db.SaveChanges();
                        }
                    }
                }
                
            }
            return Ok("Ok");

        }

        private void aaa(VGamePlayer vgp, int botAcc, int stepCost)
        {
            Random rO = new Random(DateTime.Now.Millisecond);

            string progress = vgp.BotAvailableQuestions.Trim();
            Char delimeter = ',';
            List<string> aQs = progress.Split(delimeter).ToList();
            aQs.Shuffle();
            int val = int.Parse(aQs.Take(1).FirstOrDefault());
            aQs.Remove(val.ToString());

            int rCorrect = rO.Next(1, 100);
            if (rCorrect <= botAcc)
            {
                vgp.Score += (val * 100);
            }
            vgp.StepBank -= stepCost;
            vgp.QuestionsAnswered += 1;
            vgp.BotStageProgress += 1;
            if (vgp.BotStageProgress == 5)
            {
                vgp.BotStageProgress = 0;
                vgp.BotAvailableQuestions = "1,2,2,3,3,4,5";
            }
            else
            {
                vgp.BotAvailableQuestions = string.Join(",", aQs);
            }
        }

    }
}
