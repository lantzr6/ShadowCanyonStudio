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
    public class AccountController : ApiController
    {

        private TriviaTraverse171207Entities db = new TriviaTraverse171207Entities();

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Account/UpdateAccount")]
        [ResponseType(typeof(string))]
        public string UpdateAccount(MobilePlayer inObj)
        {
            Player player = db.Players.Find(inObj.PlayerId);
            if (player != null)
            {
                player.UserName = inObj.UserName;
                player.EmailAddr = inObj.EmailAddr;
                player.Password = inObj.Password;
                player.PlayerLevel = inObj.PlayerLevel;
                player.CurrentSteps = inObj.CurrentSteps;
                player.StepBank = inObj.StepBank;
                player.LastStepUpdate = inObj.LastStepUpdate;
                player.Coins = inObj.Coins;
                player.Stars = inObj.Stars;
                player.Points = inObj.Points;
                player.UpdatedAt = DateTime.Now;
                db.SaveChanges();
            }

            return "Ok";
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Account/GetAccount")]
        [ResponseType(typeof(MobilePlayer))]
        public IHttpActionResult GetAccount(string email, string password)
        {
            Player player = db.Players.Where(o => o.EmailAddr.ToLower().Trim() == email.ToLower() && o.Password == password).FirstOrDefault();

            MobilePlayer mPlayer = null;
            if (player != null)
            {
                mPlayer = new MobilePlayer();
                mPlayer.PlayerId = player.PlayerId;
                mPlayer.UserName = player.UserName;
                mPlayer.EmailAddr = player.EmailAddr;
                mPlayer.Password = player.Password;  //temp password
                mPlayer.PlayerLevel = player.PlayerLevel;
                mPlayer.CurrentSteps = player.CurrentSteps;
                mPlayer.StepBank = player.StepBank;
                mPlayer.LastStepUpdate = player.LastStepUpdate;
                mPlayer.Coins = player.Coins;
                mPlayer.Stars = player.Stars;
                mPlayer.Points = player.Points;
            }

            return Ok(mPlayer);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Account/CheckEmailExists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult CheckEmailExists(string email)
        {
            bool result = db.Players.Where(o => o.EmailAddr.ToLower().Trim() == email.ToLower() && !o.Deleted).Any();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Account/CheckUserNameExists")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult CheckUserNameExists(string username)
        {
            bool result = db.Players.Where(o => o.UserName.ToLower().Trim() == username.ToLower() && !o.Deleted).Any();

            return Ok(result);
        }
    }

}
