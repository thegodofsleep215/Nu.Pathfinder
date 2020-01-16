using System;
using Microsoft.AspNetCore.Mvc;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipController : ControllerBase 
    {
        [HttpGet]
        public IActionResult Get(string name)
        {
            var dal = new FileShipsDal(".\\client-app\\data");
            var ship = dal.Get(name);
            if (ship == null) return new NotFoundResult();

            return new JsonResult(ship);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Names()
        {
            var dal = new FileShipsDal(".\\client-app\\data");
            return new JsonResult(dal.GetNames());
        }
    }
}
