using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Wpf
{
    public interface IShipDal
    {
        List<Ship> GetAll();

        Ship Get(string name);
    }
}

