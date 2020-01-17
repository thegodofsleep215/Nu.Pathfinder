using System;
using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipLoadoutController : BaseObjectController<ShipLoadOut>
    {
        public ShipLoadoutController() : base(new FileShipLoadoutDal(".\\client-app\\data"))
        {
        }

        protected override ShipLoadOut CreateObject(string name)
        {
            return new ShipLoadOut
            {
                Name = name,
                CrewMembers = new System.Collections.Generic.List<LoadoutCrewMember>()
            };
        }

        protected override string GetObjectName(ShipLoadOut obj)
        {
            return obj.Name;
        }

    }
}
