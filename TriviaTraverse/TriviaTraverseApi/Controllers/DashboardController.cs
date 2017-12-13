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
    public class DashboardController : ApiController
    {
        private TriviaTraverse171207Entities db = new TriviaTraverse171207Entities();

        [Authorize]
        [HttpGet]
        [Route("api/Dashboard/GetGames")]
        [ResponseType(typeof(MobileDashboard))]
        public IHttpActionResult GetGames(int playerid)
        {
            PlayerCampaign campaign = db.PlayerCampaigns.Where(o => o.PlayerId == playerid).FirstOrDefault();
            MobileCampaign mobileCampaign = new MobileCampaign(campaign);

            List<VGame> vGames = (from vg in db.VGames
                                  join vgp in db.VGamePlayers.Include("Player").Where(o => o.PlayerId == playerid) on vg.VGameId equals vgp.VGameId
                                  select vg).ToList();


            List<MobileVGame> mobileVGames = new List<MobileVGame>();
            foreach (VGame vg in vGames)
            {
                mobileVGames.Add(new MobileVGame(vg, playerid));
            }

            MobileDashboard dashboard = new MobileDashboard()
            {
                Campaigns = new List<MobileCampaign>() { mobileCampaign },
                VGames = mobileVGames
            };

            return Ok(dashboard);

        }
        
    }
}
