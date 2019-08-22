using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class BaseResponse
    {
        public bool Success
        {
            get
            {
                return Messages.Count == 0;
            }
        }

        public List<string> Messages { get; private set; } = new List<string>();
    }
}
