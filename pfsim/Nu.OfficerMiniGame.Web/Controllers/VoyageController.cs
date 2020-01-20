using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoyageController : BaseObjectController<PlanVoyage>
    {
        public VoyageController(IObjectDal<PlanVoyage> dal) : base(new FilePlanVoyageDal(".\\client-app\\data"))
        {
        }

        protected override PlanVoyage CreateObject(string name)
        {
            return new PlanVoyage
            {
                Name = name
            };
        }

        protected override string GetObjectName(PlanVoyage obj)
        {
            return obj.Name;
        }
    }
}
