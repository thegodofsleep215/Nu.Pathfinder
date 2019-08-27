using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class BaseResponse
    {
        public bool Success { get; set; } = false;

        public List<string> Messages
        {
            get
            {
                if (_messages == null)
                    _messages = new List<string>();

                return _messages;
            }
        }
        private List<string> _messages;
    }
}
