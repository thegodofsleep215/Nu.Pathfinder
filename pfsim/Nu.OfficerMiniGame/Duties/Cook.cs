﻿using Nu.Game.Common;
using System;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    /// Cook – Keep the crew well fed and provisioned.DC is 7 plus 1/10 crew, minus the ship’s current wellbeing, 
    /// and any applicable modifiers from the manage and command checks and the weather.Additionally, there is 
    /// a -4 penalty if the ship has not been able to take on fresh provision in a month, or a -2 circumstance 
    /// penalty if the ship’s provisions lack diversity because resupply has been limited.Failure on the check 
    /// results in the effective wellbeing of the crew for the next day being 1 less than normal.Failure by 5 or
    /// more results in effective wellbeing being 2 less than normal, by 10 or more 3 less than normal, and so 
    /// forth to a minimum of zero(equivalent to starving).  Additionally, if the check fails by 15 or more, 
    /// there is a +4 chance of medical problems the next day.You may have 1 assistant.
    public class Cook : IDuty
    {
        public void PerformDuty(Ship ship, bool verbose, ref MiniGameStatus status)
        {
            var dc = 7 + (ship.TotalCrew / 10) - status.ManageModifier - ship.CrewMorale.WellBeing;
            if (dc < 10)
                dc = 10;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Cook));
            var job = ship.CookJob;
            var result = DiceRoller.D20(1) + assistBonus + job.SkillBonus - dc;
            status.CookResult = result;

            if(verbose)
                status.GameEvents.Add(new PerformedDutyEvent(DutyType.Cook, job.CrewName, dc, assistBonus, job.SkillBonus, status.CookResult));

            if (result < 0)
            {
                var wm = Math.Abs(result) / 5 + 1;
                wm = wm > ship.CrewMorale.WellBeing ? ship.CrewMorale.WellBeing : wm;
                status.GameEvents.Add(new EpicCookingFailureEvent
                {
                    WellbeingPenalty = wm * -1,
                    HealthCheckModifier = result <= -15 ? 4 : 0
                });
            }
        }

        private int PerformAssists(List<JobMessage> list)
        {
            int retval = 0;

            foreach (var assist in list)
            {
                retval += (DiceRoller.D20(1) + assist.SkillBonus >= 10) ? 2 : 0;
            }

            return retval;
        }
    }
}
