using Nu.OfficerMiniGame.Dal.Dto;
using System.IO;

namespace Nu.OfficerMiniGame.Dal.Dal
{
    public class FileVoyageDal : BaseJsonFileDal<Voyage>, IVoyageDal
    {
        public FileVoyageDal(string folder) : base(Path.Combine(folder, "voyages"))
        {
        }
    }
}