using Microsoft.AspNetCore.Mvc;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrewController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string name)
        {
            var dal = new FileCrewDal(".\\client-app\\data");
            var cm = dal.Get(name);
            if (cm == null) return new NotFoundResult();

            return new JsonResult(cm);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Names()
        {
            var dal = new FileCrewDal(".\\client-app\\data");
            return new JsonResult(dal.GetNames());
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Update([FromBody] CrewMember crewMember)
        {
            var dal = new FileCrewDal(".\\client-app\\data");
            dal.Update(crewMember.Name, crewMember);
            return new OkResult();
        }

    }
}
