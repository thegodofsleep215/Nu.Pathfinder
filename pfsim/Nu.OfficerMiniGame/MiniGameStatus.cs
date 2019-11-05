using System;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class MiniGameStatus 
    {
        public int CommandResult { get; set; }

        public int CommandModifier
        {
            get
            {
                if (CommandResult >= 5) return 2;
                if (CommandResult >= 0) return 0;
                return Math.Abs(CommandResult) / 5 * -2;
            }
        }

        public int ManageResult { get; set; }

        public int ManageModifier
        {
            get
            {
                if (ManageResult <= -10) return -4;
                return 0;
            }
        }

        public List<int> WatchResults { get; set; } = new List<int>();

        public List<int> MinistrelResults { get; set; } = new List<int>();

        public int WatchModifier
        {
            get
            {
                int retval = 0;
                foreach (var result in WatchResults)
                {
                    retval += result < 0 ? -2 : 0;
                }

                return retval;
            }
        }

        public int PilotResult { get; set; }

        public int NavigationResult { get; set; }

        public int MaintainResult { get; set; }

        public int CookResult { get; internal set; }

        public List<object> DutyEvents { get; } = new List<object>();
    }
}
