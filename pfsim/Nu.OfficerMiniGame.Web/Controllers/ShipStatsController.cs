using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipStatsController: BaseObjectController<ShipStats>
    {
        public ShipStatsController() : base (new FileShipStatsDal(".\\client-app\\data")) { }

        protected override ShipStats CreateObject(string name)
        {
            return new ShipStats
            {
                Name = name
            };
        }

        protected override string GetObjectName(ShipStats obj)
        {
            return obj.Name;
        }
    }
}
