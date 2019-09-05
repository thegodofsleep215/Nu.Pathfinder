using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    [Serializable]
    public class Job
    {
        public string CrewName { get; set; }
        public DutyType DutyType { get; set; }
        public virtual bool IsAssistant { get; set; }
    }
}
