using System.Collections.Generic;
using System.IO;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    public class FileCrewDal : BaseJsonFileDal<BaseCrewMember>, ICrewDal
    {
        public FileCrewDal(string rootDir)
            : base(Path.Combine(rootDir, "crews"))
        {
        }
    }
}
