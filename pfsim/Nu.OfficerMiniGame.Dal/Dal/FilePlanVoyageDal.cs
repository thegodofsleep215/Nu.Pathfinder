using Nu.OfficerMiniGame.Dal.Dto;
using System.IO;

namespace Nu.OfficerMiniGame.Dal.Dal
{
    public class FilePlanVoyageDal : BaseJsonFileDal<PlanVoyage>, IPlanVoyageDal
    {
        public FilePlanVoyageDal(string folder) : base(Path.Combine(folder, "voyagePlans"))
        {
        }
    }
}