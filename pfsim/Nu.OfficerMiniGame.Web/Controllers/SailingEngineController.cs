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

        [HttpGet]
        [Route("[action]")]
        public IActionResult State(string name)
        {
            var vd = new FileVoyageDal(rootDir);
            var mgd = new MiniGameDal(new FileShipLoadoutDal(rootDir), new FileShipStatsDal(rootDir), new FileCrewMemberStats(rootDir));

            var voyage = vd.Get(name);
            if(voyage == null)
            {
                return new NotFoundResult();
            }
            List<Ship> ships = voyage.ShipLoadouts.Select(x => mgd.GetLoadout(x)).ToList();

            var fleetProgress= new FleetVoyageProgress(ships.Select(x => EventProcessor.Process(x, voyage.Events[x.CrewName].Select(y => y.Event).ToList())).ToList());

            return new JsonResult(new { voyage = voyage, state = fleetProgress });
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Sail([FromBody] SailingParameters sp)
        {
            var vd = new FileVoyageDal(rootDir);
            var mgd = new MiniGameDal(new FileShipLoadoutDal(rootDir), new FileShipStatsDal(rootDir), new FileCrewMemberStats(rootDir));

            var voyage = vd.Get(sp.VoyageName);
            List<Ship> ships = voyage.ShipLoadouts.Select(x => mgd.GetLoadout(x)).ToList();

            ships.ForEach(x =>
            {
                EventProcessor.Process(x, voyage.Events[x.CrewName].Select(y => y.Event).ToList());
                ProcessSailingParameters(sp, ref x);
            });

            var engine = new MultiShipGameEngine(true);
            var results = engine.Sail(ships.ToArray(), sp);
            voyage.AddEvents(results);
            vd.Update(sp.VoyageName, voyage);

            var currentProgress = new FleetVoyageProgress(ships.Select(x => EventProcessor.Process(x, voyage.Events[x.CrewName].Select(y => y.Event).ToList())).ToList());

            var anon = new
            {
                results = results.Select(kvp => new { loadout = kvp.Key, message = kvp.Value.Select(x => x.ToString()).ToArray(), }).ToList(),
                state = currentProgress
            };
            return new JsonResult(anon);
        }


        private void ProcessSailingParameters(SailingParameters sp, ref Ship ship)
        {
            if (sp.ShipModifiers != null)
            {
                var sm = sp.ShipModifiers.ToDictionary(x => x.LoadoutName, x => x);
                if (sm.ContainsKey(ship.CrewName))
                {
                    ship.CrewMorale.TemporaryMoralePenalty = sm[ship.CrewName].MoraleModifier;
                    ship.TemporaryDisciplineModifier = sm[ship.CrewName].DisciplineModifier;
                    ship.TemporaryCommandModifier = sm[ship.CrewName].CommandModifier;
                    ship.CrewUnfitForDuty = sm[ship.CrewName].NumberOfCrewUnfitForDuty;
                    ship.DiseasedCrew = sm[ship.CrewName].NumberOfCrewDiseased;
                    ship.DisciplineStandards = sm[ship.CrewName].DisciplineStandards;
                    ship.Swabbies = sm[ship.CrewName].Swabbies;
                    ship.TemporaryPilotingModifier = sp.PilotingModifier;
                    ship.TemporaryNavigationModifier = sp.NavigationModifier;
                    ship.TemporaryPilotModifier = sp.GetWeatherModifier(DutyType.Pilot);
                    ship.TemporaryMaintainModifier = sp.GetWeatherModifier(DutyType.Maintain);
                    ship.TemporaryWatchModifier = sp.GetWeatherModifier(DutyType.Watch);
                }
            }
        }

    }

}
