using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public interface IShipStatsDal
    {
        List<string> GetNames();

        ShipStats Get(string name);
    }
}