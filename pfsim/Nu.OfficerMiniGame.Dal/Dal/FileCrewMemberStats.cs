using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;
using System.IO;

namespace Nu.OfficerMiniGame.Dal.Dal
{
    public class FileCrewMemberStats : BaseJsonFileDal<CrewMemberStats>, ICrewMemberStatsDal
    {
        public FileCrewMemberStats(string rootDir)
            : base(Path.Combine(rootDir, "crews"))
        {
        }
    }
}
