using System;
using System.Collections.Generic;

namespace pfsim.Officer
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
        public void PerformDuty(IShip crew, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 7 + (crew.TotalCrew / 10) - input.Wellbeing;
            var assistBonus = PerformAssists(crew.GetAssistance(DutyType.Cook));
            var result = DiceRoller.D20(1) + assistBonus + crew.CookSkillBonus - dc;
            status.CookResult = result;
            if (result >= 0)
            {
                status.ActionResults.Add("The crew eats well.");
            }
            else
            {
                var wm = Math.Abs(result) / 5 + 1;
                wm = wm > input.Wellbeing ? input.Wellbeing : wm;
                status.ActionResults.Add($"A sorry meal has been served reducing the wellbeing score by {wm} for a day.");
                // TODO: Raise an event listening for temporary wellbeing penalty?
            }
        }

        private int PerformAssists(List<Assists> list)
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
