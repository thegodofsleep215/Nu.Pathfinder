using Nu.OfficerMiniGame.Dal.Dto;
using System.IO;

namespace Nu.OfficerMiniGame.Dal.Dal
{
    public class FileShipLoadoutDal : BaseJsonFileDal<ShipLoadOut>, IShipLoadoutDal
    {

        public FileShipLoadoutDal(string rootDir) :
            base(Path.Combine(rootDir, "shipLoadouts"))
        {
        }
    }
}
