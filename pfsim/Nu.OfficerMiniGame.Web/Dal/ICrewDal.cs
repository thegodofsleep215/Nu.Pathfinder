using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public interface ICrewDal 
    {
        List<string> GetNames();

        BaseCrewMember Get(string name);

        void Update(string name, BaseCrewMember crewMember);

    }
}