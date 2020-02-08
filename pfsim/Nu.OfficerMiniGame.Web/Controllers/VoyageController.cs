using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Web.Model;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoyageController : BaseObjectController<Voyage>
    {
        public VoyageController() : base(new FileVoyageDal(".\\client-app\\data"))
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
    }
}
