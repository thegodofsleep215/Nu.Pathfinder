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

            var engine = new SailingEngine();
            var results = engine.Sail(ships.ToArray());
            voyage.AddEvents(results);
            vd.Update(sp.VoyageName, voyage);

            var shim = results.Select(kvp =>
            {
                return new { loadout = kvp.Key, results = kvp.Value.Select(x => x.ToString()).ToList() };
            }).ToList();
            return new JsonResult(shim);
        }

        private void ProcessSailingParameters(SailingParameters sp, ref Ship ship)
        {
            if (sp.ShipModifiers != null)
            {
                var sm = sp.ShipModifiers.ToDictionary(x => x.LoadoutName, x => x);
                if (sm.ContainsKey(ship.CrewName))
                {
                    ship.CrewMorale.TemporaryMoralePenalty = sm[ship.CrewName].MoraleModifier;
                    ship.CurrentVoyage.DisciplineModifier = sm[ship.CrewName].DisciplineModifier;
                    ship.CurrentVoyage.CommandModifier = sm[ship.CrewName].CommandModifier;
                    ship.CurrentVoyage.CrewUnfitForDuty = sm[ship.CrewName].NumberOfCrewUnfitForDuty;
                    ship.CurrentVoyage.DiseasedCrew = sm[ship.CrewName].NumberOfCrewDiseased;
                    ship.DisciplineStandards = sm[ship.CrewName].DisciplineStandards;
                    ship.Swabbies = sm[ship.CrewName].Swabbies;
                }
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

        public List<ShipModifiers> ShipModifiers { get; set; }

    }

    public class ShipModifiers
    {
        public string LoadoutName { get; set; }
        public int MoraleModifier { get; set; }

        public int DisciplineModifier { get; set; }

        public int CommandModifier { get; set; }

        public int NumberOfCrewUnfitForDuty { get; set; }

        public int NumberOfCrewDiseased { get; set; }
        public DisciplineStandards DisciplineStandards { get; set; }

        public int Swabbies { get; set; }
    }


}
