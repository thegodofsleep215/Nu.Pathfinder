using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public interface IShipDal
    {
        List<string> GetNames();

        BaseShip Get(string name);
    }
}