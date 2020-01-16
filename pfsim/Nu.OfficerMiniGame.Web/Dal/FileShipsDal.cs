using System.IO;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public class FileShipsDal : BaseJsonFileDal<BaseShip>, IShipDal
    {

        public FileShipsDal(string rootDir) :
            base(Path.Combine(rootDir, "Ships"))
        {
        }
    }
}
