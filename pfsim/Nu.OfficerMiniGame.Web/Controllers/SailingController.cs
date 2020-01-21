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

            ships.ForEach(x => ProcessSailingParameters(sp, ref x));

            var results = ships.Select(ship =>
            {
                var game = new SailingEngine(ship, true);
                return new { loadout = ship.CrewName, results = game.Run() };
            }).ToList();
            return new JsonResult(results);
        }

        private void ProcessSailingParameters(SailingParameters sp, ref Ship ship)
        {
            if (sp.ShipModifiers != null && sp.ShipModifiers.ContainsKey(ship.CrewName))
            {
                ship.CrewMorale.TemporaryMoralePenalty = sp.ShipModifiers[ship.CrewName].MoralModifier;
                ship.CurrentVoyage.DisciplineModifier = sp.ShipModifiers[ship.CrewName].DisciplineModifier;
                ship.CurrentVoyage.CommandModifier = sp.ShipModifiers[ship.CrewName].CommandModifier;
                ship.CurrentVoyage.CrewUnfitForDuty = sp.ShipModifiers[ship.CrewName].NumberOfCrewUnfitForDuty;
                ship.CurrentVoyage.DiseasedCrew = sp.ShipModifiers[ship.CrewName].NumberOfCrewDiseased;
                ship.DisciplineStandards = sp.ShipModifiers[ship.CrewName].DisciplineStandards;
                ship.Swabbies = sp.ShipModifiers[ship.CrewName].Swabbies;
            }
            ship.CurrentVoyage.NarrowPassage = sp.NarrowPassage;
            ship.CurrentVoyage.ShallowWater = sp.ShallowWater;
            ship.CurrentVoyage.OpenOcean = sp.OpenOcean;
            ship.CurrentVoyage.NightStatus = sp.NightStatus;
        }

    }

    public class SailingParameters
    {
        public string VoyageName { get; set; }

        public bool NarrowPassage { get; set; }

        public bool ShallowWater { get; set; }

        public bool OpenOcean { get; set; }

        public NightStatus NightStatus { get; set; }

        public Dictionary<string, ShipModifiers> ShipModifiers { get; set; }

    }

    public class ShipModifiers
    {
        public int MoralModifier { get; set; }

        public int DisciplineModifier { get; set; }

        public int CommandModifier { get; set; }

        public int NumberOfCrewUnfitForDuty { get; set; }

        public int NumberOfCrewDiseased { get; set; }
        public DisciplineStandards DisciplineStandards { get; set; }

        public int Swabbies { get; set; }
    }


}
