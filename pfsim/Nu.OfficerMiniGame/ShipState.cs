using System;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class ShipState
    {
        public string LoadoutName { get; set; }

        public int DiseasedCrew { get; set; }

        public DisciplineStandards DisciplineStandards { get; set; }

        public int SwabbieCount { get; set; }

        public int TemporaryDisciplineModifier { get; set; }

        public int TemporaryCommandModifier { get; set; }

        public int TemporaryPilotingModifier { get; set; }

        public int TemporaryNavigationModifier { get; set; }

        public int CrewDisciplineModifier
        {
            get
            {
                int retval = TemporaryDisciplineModifier;

                switch (DisciplineStandards)
                {
                    case DisciplineStandards.Lax:
                        retval += 2;
                        break;
                    case DisciplineStandards.Strict:
                        retval -= 2;
                        break;
                }
                return retval;
            }
        }


        public Dictionary<string, int> MinistrelResults { get; set; } = new Dictionary<string, int>();

        public List<int> WatchResults { get; set; } = new List<int>();

        public int CommandModifier
        {
            get
            {
                if (CommandResult >= 5) return 2;
                if (CommandResult >= 0) return 0;
                return Math.Abs(CommandResult) / 5 * -2;
            }
        }

        public int ManageModifier
        {
            get
            {
                if (ManageResult <= -10) return -4;
                return 0;
            }
        }

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

        public int CommandResult { get; set; }

        public int CookResult { get; set; }

        public int ManageResult { get; set; }

        public int MaintainResult { get; set; }

        public int NavigateResult { get; set; }

        public int PilotResult { get; set; }
    }

}
