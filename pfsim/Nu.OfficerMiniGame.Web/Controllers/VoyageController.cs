using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Weather;
using Nu.OfficerMiniGame.Web.Model;
using System.Linq;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoyageController : BaseObjectController<Voyage>
    {
        private static string data = ".\\client-app\\data";

        public VoyageController() : base(new FileVoyageDal(data))
        {
        }
        protected override Voyage CreateObject(string name)
        {
            return new Voyage
            {
                Name = name
            };
        }

        protected override string GetObjectName(Voyage obj)
        {
            return obj.Name;
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult SetCourse([FromQuery] string name, [FromBody] SetCourseEvent course)
        {
            Voyage voyage = null;
            if (!dal.Exists(name))
            {
                voyage = new Voyage()
                {
                    Name = name
                };
                dal.Create(name, voyage);
            }
            else
            {
                voyage = dal.Get(name);
            }
            voyage.AddEvent(course);
            dal.Update(name, voyage);
            return new OkResult();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetCurrentProgress(string name)
        {
            var fleetState = GetState(name);
            var state = VoyageStatus.FromFleetState(fleetState);
            var voyageParams = SailingParameters.FromFleetState(name, fleetState);
            var results = fleetState.ShipStates.Select(ss =>
            {
                return new
                {
                    loadout = ss.Key,
                    messages = ss.Value.ShipReportEvents.Select(x => x.ToString())
                };
            }).ToList();
            return new JsonResult(new { state, voyageParams, results });
        }

        private FleetState GetState(string name)
        {
            var voyage = dal.Get(name);
            return EventProcessor.Process(voyage.EventsAsObject);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Sail([FromBody]SailingParameters parameters)
        {
            var voyage = dal.Get(parameters.VoyageName);
            var state = EventProcessor.Process(voyage.EventsAsObject);
            var mgd = new MiniGameDal(new FileShipLoadoutDal(data), new FileShipStatsDal(data), new FileCrewMemberStats(data));
            var ships = parameters.ShipInputs.Select(x => mgd.GetLoadout(x.LoadoutName)).ToList();
            var events = MultiShipGameEngine.Sail(ships, ref state, parameters, new PathfinderWeatherGenerator());
            voyage.AddEvents(events);
            dal.Update(voyage.Name, voyage);
            return new OkResult();
        }

    }
}
