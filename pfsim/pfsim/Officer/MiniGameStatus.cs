using System;
using System.Collections.Generic;

namespace pfsim.Officer
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

        public int WatchResult { get; set; }

        public int WatchModifier
        {
            get
            {
                return WatchResult < 0 ? -2 : 0;
            }
        }

        public List<string> ActionResults { get; } = new List<string>();

        public int WeatherModifier { get; set; }
        public int PilotResult { get; set; }
        public int NavigationResult { get; set; }
        public int MaintainResult { get; set; }
        public int CookResult { get; internal set; }
    }
}
