using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Weather;
using System;
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
            if (voyage == null)
            {
                return new NotFoundResult();
            }
            List<Ship> ships = voyage.ShipLoadouts.Select(x => mgd.GetLoadout(x)).ToList();
            FleetVoyageProgress fleetProgress = new FleetVoyageProgress();
            if (voyage.Events != null && voyage.Events.Any())
            {
                fleetProgress = new FleetVoyageProgress(ships.Select(x => EventProcessor.Process(x, voyage, voyage.Events[x.CrewName].Select(y => y.Event).ToList())).ToList(),
                    null);
            }

            return new JsonResult(new { voyage = voyage, state = fleetProgress });
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CourseChange([FromQuery] string name, [FromBody] SetCourseEvent sc)
        {
            var vd = new FileVoyageDal(rootDir);
            var voyage = vd.Get(name);
            voyage.AddEventToAllShips(sc);
            vd.Update(voyage.Name, voyage);
            return new OkResult();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Sail([FromBody] SailingParameters sp)
        {
            //var vd = new FileVoyageDal(rootDir);
            //var mgd = new MiniGameDal(new FileShipLoadoutDal(rootDir), new FileShipStatsDal(rootDir), new FileCrewMemberStats(rootDir));

            //var voyage = vd.Get(sp.VoyageName);
            //List<Ship> ships = voyage.ShipLoadouts.Select(x => mgd.GetLoadout(x)).ToList();

            //if (voyage.HoistingAnchor)
            //{
            //    voyage.AddEventToAllShips(new VoyageUpdateEvent
            //    {
            //        DaysPlanned = voyage.DaysPlanned,
            //        StartDate = voyage.StartDate,
            //        ShipLoadouts = voyage.ShipLoadouts
            //    });
            //}

            //var cp = new List<VoyageProgress>();
            //ships.ForEach(x =>
            //{
            //    if (voyage.Events.ContainsKey(x.CrewName))
            //    {
            //        var p = EventProcessor.Process(x, voyage, voyage.Events[x.CrewName].Select(y => y.Event).ToList());
            //        cp.Add(p);
            //    }
            //    ProcessSailingParameters(sp, ref x);
            //});
            //var dov = 0;
            //if (cp.Any())
            //{
            //    var fp = new FleetVoyageProgress(cp, null);
            //    dov = fp.DayOfVoyage;
            //}

            //var engine = new MultiShipGameEngine(true);

            //var wg = new PathfinderWeatherGenerator();

            //WeatherConditions oldWc = null;
            //if (voyage.weatherConditions.Any())
            //{
            //    oldWc = voyage.weatherConditions[voyage.weatherConditions.Count - 1];
            //}

            //var wc = wg.GetWeatherConditions(new WeatherInput
            //{
            //    LastConditions = oldWc,
            //    Date = voyage.StartDate + TimeSpan.FromDays(dov),
            //    ElevationFt = 0,
            //    Region = Region.Tropical
            //});
            //var results = engine.Sail(ships.ToArray(), sp, wc);
            //voyage.AddEvents(results);
            //voyage.weatherConditions.Add(wc);
            //vd.Update(sp.VoyageName, voyage);

            //var currentProgress = new FleetVoyageProgress(ships.Select(x => EventProcessor.Process(x, voyage, voyage.Events[x.CrewName].Select(y => y.Event).ToList())).ToList(),
            //    wc);

            //var anon = new
            //{
            //    results = results.Select(kvp => new { loadout = kvp.Key, message = kvp.Value.Select(x => x.ToString()).ToArray(), }).ToList(),
            //    state = currentProgress
            //};
            //return new JsonResult(anon);
            throw new NotImplementedException();
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
                }
            }
        }

    }

}
