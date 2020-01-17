using Nu.OfficerMiniGame.Dal.Dto;
using System.IO;

namespace Nu.OfficerMiniGame.Dal.Dal
{
    public class FileShipStatsDal : BaseJsonFileDal<ShipStats>, IShipStatsDal
    {
        public FileShipStatsDal(string folder) :
            base(Path.Combine(folder, "ships"))
        {
        }
    }
}