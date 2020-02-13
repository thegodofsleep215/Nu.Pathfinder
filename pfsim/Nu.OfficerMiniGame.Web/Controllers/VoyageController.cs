using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;

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

        [HttpPost]
        [Route("[action]")]
        public IActionResult GetCurrentProgress(string name)
        {
            var voyage = dal.Get(name);
            return new OkResult();
        }

    }
}
