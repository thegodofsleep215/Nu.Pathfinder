using System;
using System.Collections.Generic;
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

        public override IActionResult Update([FromBody] ShipLoadOut obj)
        {
            var mgd = new MiniGameDal(new StubLoadoutDal(obj), new FileShipStatsDal(".\\client-app\\data"), new FileCrewMemberStats(".\\client-app\\data"));
            var ship = mgd.GetLoadout(obj.Name);
            var valid = ShipValidation.ValidateShip(ship);
            if (valid.Success)
            {
                return base.Update(obj);
            }
            else
            {
                return BadRequest(valid.Messages);
            }
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

    public class StubLoadoutDal : IShipLoadoutDal
    {
        private ShipLoadOut slo;

        public StubLoadoutDal(ShipLoadOut slo)
        {
            this.slo = slo;
        }
        public bool Create(string name, ShipLoadOut crewMember)
        {
            throw new NotImplementedException();
        }

        public void Delete(string name)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        public ShipLoadOut Get(string name)
        {
            return slo;
        }

        public List<string> GetNames()
        {
            throw new NotImplementedException();
        }

        public void Update(string name, ShipLoadOut crewMember)
        {
            throw new NotImplementedException();
        }
    }
}
