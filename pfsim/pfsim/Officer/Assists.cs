using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    /// <summary>
    /// TODO: Would it make sense to unify this with job?
    /// </summary>
    public class Assists
    {
        public DutyType Duty { get; set; }
        public int SkillBonus { get; set; }
    }
}
