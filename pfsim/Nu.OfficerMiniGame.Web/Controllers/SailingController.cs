using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SailingEngineController : ControllerBase
    {
        private readonly string rootDir = ".\\client-app\\data";

        [HttpPost]
        [Route("[action]")]
        public IActionResult Sail([FromBody] SailingParameters sp)
        {
            var vd = new FilePlanVoyageDal(rootDir);
            var mgd = new MiniGameDal(new FileShipLoadoutDal(rootDir), new FileShipStatsDal(rootDir), new FileCrewMemberStats(rootDir));

            var voyage = vd.Get(sp.VoyageName);
            List<Ship> ships = voyage.ShipLoadouts.Select(x => mgd.GetLoadout(x)).ToList();

            var results = ships.Select(ship =>
            {
                var game = new SailingEngine(ship, true);
                return new { loadout = ship.CrewName, results = game.Run()};
            }).ToList();
            return new JsonResult(results);
        }
    }

    public class SailingParameters
    {
        public string VoyageName { get; set; }
    }
}
