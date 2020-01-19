using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Dal.Dal;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public abstract class BaseObjectController<T> : ControllerBase where T : class
    {
        protected IObjectDal<T> dal;

        public BaseObjectController(IObjectDal<T> dal)
        {
            this.dal = dal;
        }

        protected abstract string GetObjectName(T obj);

        protected abstract T CreateObject(string name);


        [HttpGet]
        public IActionResult Get(string name)
        {
            var cm = dal.Get(name);
            if (cm == null) return new NotFoundResult();

            return new JsonResult(cm);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Names()
        {
            return new JsonResult(dal.GetNames());
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Update([FromBody] T obj)
        {
            dal.Update(GetObjectName(obj), obj);
            return new OkResult();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Create(string name)
        {
            if (dal.Create(name, CreateObject(name)))
            {
                return new OkResult();
            }
            return new ConflictResult();
        }

        [HttpDelete]
        [Route("[action]")]
        public IActionResult Delete(string name)
        {
            dal.Delete(name);
            return new OkResult();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Exists(string name)
        {
            return new JsonResult(new { exists = dal.Exists(name) });
        }

    }

}
