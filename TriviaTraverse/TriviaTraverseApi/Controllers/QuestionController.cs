using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TriviaTraverseApi.Models;

namespace TriviaTraverseApi.Controllers
{
    public class QuestionController : ApiController
    {

        private TriviaTraverse171207Entities db = new TriviaTraverse171207Entities();

        [Authorize]
        [HttpPost]
        [Route("api/Question/UpdateCampaignQuestionResult")]
        [ResponseType(typeof(string))]
        public string PostPlayerCampaignQuestionResult(MobilePlayerQuestionResult InResults)
        {
            try
            {
                Player player = db.Players.Find(InResults.PlayerId);

                PlayerQuestionResult result = new PlayerQuestionResult();
                result.PlayerId = InResults.PlayerId;
                result.QuestionId = InResults.QuestionId;
                result.PlayerAnswerText = InResults.PlayerAnswerText;
                result.IsCorrect = InResults.IsCorrect;
                result.PointsRewarded = InResults.PointsRewarded;
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = DateTime.Now;
                result.Deleted = false;
                db.PlayerQuestionResults.Add(result);

                db.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/Question/UpdateVGameQuestionResult")]
        [ResponseType(typeof(string))]
        public string PostPlayerVGameQuestionResult(MobilePlayerVGameQuestionResult InResults)
        {
            try
            {
                Player player = db.Players.Find(InResults.PlayerId);

                PlayerQuestionResult result = new PlayerQuestionResult();
                result.PlayerId = InResults.PlayerId;
                result.QuestionId = InResults.QuestionId;
                result.PlayerAnswerText = InResults.PlayerAnswerText;
                result.IsCorrect = InResults.IsCorrect;
                result.PointsRewarded = InResults.PointsRewarded;
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = DateTime.Now;
                result.Deleted = false;
                db.PlayerQuestionResults.Add(result);

                VGamePlayer vGameP = db.VGamePlayers.Where(o => o.VGameId == InResults.VGameId && o.PlayerId == InResults.PlayerId).FirstOrDefault();
                vGameP.Score = vGameP.Score + InResults.PointsRewarded;
                vGameP.QuestionsAnswered = vGameP.QuestionsAnswered + 1;

                db.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}
