using System;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class ShipState
    {
        public ShipState() { }
       
        public ShipState(ShipInput input)
        {
            LoadoutName = input.LoadoutName;
            DiseasedCrew = input.DiseasedCrew;
            Swabbies = input.Swabbies;
            CrewUnfitForDuty = input.CrewUnfitForDuty;
            DisciplineStandards = input.DisciplineStandards;
            TemporaryDisciplineModifier = input.DisciplineModifier;
            TemporaryCommandModifier = input.CommandModifier;
        }

        public string LoadoutName { get; set; }

        public List<object> ShipReportEvents = new List<object>();

        public int DiseasedCrew { get; set; }

        public int Swabbies { get; set; }

        public int CrewUnfitForDuty { get; set; }

        public DisciplineStandards DisciplineStandards { get; set; }

        public bool DiseaseAboardShip
        {
            get
            {
                return DiseasedCrew > 0;
            }
        }

        public int TemporaryDisciplineModifier { get; set; }

        public int TemporaryCommandModifier { get; set; }

        public Morale CrewMorale { get; set; } = new Morale();

        public int DisciplineStandardsModifier
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
