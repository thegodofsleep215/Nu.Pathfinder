using System;
using System.Collections.Generic;

namespace pfsim.Officer
{
    /// <summary>
    /// Command – Issue instructions to crew.  Always first check of day.  DC is 5 + Ship’s difficulty modifier + 1 / 10 crew.
    /// The ship’s morale bonus is a bonus or penalty to any command check.   Failure by 5 or more results in a -2 check on
    /// all other job checks of that day, and each failure by each additional 5 beyond the first results in a further
    /// -2 penalty.  Failure by 15 or more additionally results in a +4 chance of discipline problems.  Success by 5 or more
    /// results in a +2 bonus on all job checks for that day.  You may have up to two assistants, however.
    /// </summary>
    public class Command : IDuty
    {
        public void PerformDuty(Ship ship, ref MiniGameStatus status)
        {
            var dc = 5 + ship.ShipDc + (ship.TotalCrew / 10) + ship.CurrentVoyage.CommandModifier;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Command));
            var job = ship.CommanderJob;
            status.CommandResult = (DiceRoller.D20(1) + job.SkillBonus + assistBonus) - dc;
            if(status.CommandResult < 0 && status.CommandResult >= -2)
            {
                // Use a ministrel.
                int ministrel = status.MinistrelResults.Count;

                if (ship.MinistrelBonuses.Count > ministrel)
                {
                    dc = 10 + (ship.TotalCrew / 10) > 15 ? 10 + (ship.TotalCrew / 10) : 15;
                    var shanty = DiceRoller.D20(1) + ship.MinistrelBonuses[ministrel] - dc;
                    status.MinistrelResults.Add(shanty);
                    if (shanty >= 0)
                    {
                        status.CommandResult += 2;
                        status.DutyEvents.Add(new SeaShantyEvent(DutyType.Command));
                    }
                }
            }
            if(SettingsManager.Verbose)
                status.DutyEvents.Add(new PerformedDutyEvent(DutyType.Command, job.CrewName, dc, assistBonus, job.SkillBonus, status.CommandResult));
        }

        private int PerformAssists(List<JobMessage> list)
        {
            int retval = 0;

            foreach(var assist in list)
            {
                retval += (DiceRoller.D20(1) + assist.SkillBonus >= 10) ? 2 : 0;
            }

            return retval;
        }
    }
}
