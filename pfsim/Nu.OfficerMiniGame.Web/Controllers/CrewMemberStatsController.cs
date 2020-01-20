using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;
using System;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrewMemberStatsController : BaseObjectController<CrewMemberStats>
    {
        public CrewMemberStatsController() : base(new FileCrewMemberStats(".\\client-app\\data"))
        {

        }

        protected override CrewMemberStats CreateObject(string name)
        {
            return new CrewMemberStats { Name = name, Skills = new CrewSkills() };
        }

        protected override string GetObjectName(CrewMemberStats obj)
        {
            return obj.Name;
        }
    }


}
