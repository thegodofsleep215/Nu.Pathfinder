using System;

namespace pfsim.Officer
{
    /// <summary>
    /// Command – Issue instructions to crew.  Always first check of day.  DC is 5 + Ship’s difficulty modifier + 1 / 10 crew.
    /// The ship’s morale bonus is a bonus or penalty to any command check.   Failure by 5 or more results in a -2 check on
    /// all other job checks of that day, and each failure by each additional 5 beyond the first results in a further
    /// -2 penalty.   Failure by 15 or more additionally results in a +4 chance of discipline problems.  Success by 5 or more
    /// results in a +2 bonus on all job checks for that day.  You may have up to two assistants, however.
    /// </summary>
    public class Command : IDuty
    {
        public void PerformDuty(Crew crew, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 5 + crew.ShipDc + (crew.CrewSize / 10);
            status.CommandResult = (DiceRoller.D20(1) + crew.CommanderSkillBonus) - dc;
            status.ActionResults.Add($"Command result: {status.CommandResult}");
        }
    }

    /// Cook – Keep the crew well fed and provisioned.DC is 7 plus 1/10 crew, minus the ship’s current wellbeing, 
    /// and any applicable modifiers from the manage and command checks and the weather.Additionally, there is 
    /// a -4 penalty if the ship has not been able to take on fresh provision in a month, or a -2 circumstance 
    /// penalty if the ship’s provisions lack diversity because resupply has been limited.Failure on the check 
    /// results in the effective wellbeing of the crew for the next day being 1 less than normal.Failure by 5 or
    /// more results in effective wellbeing being 2 less than normal, by 10 or more 3 less than normal, and so 
    /// forth to a minimum of zero(equivalent to starving).   Additionally, if the check fails by 15 or more, 
    /// there is a +4 chance of medical problems the next day.You may have 1 assistant.
    public class Cook : IDuty
    {
        public void PerformDuty(Crew crew, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 7 + (crew.CrewSize / 10) - input.Wellbeing;
            var result = DiceRoller.D20(1) + crew.CookSkillBonus - dc;
            status.CookResult = result;
            if (result >= 0)
            {
                status.ActionResults.Add("The crew eats well.");
            }
            else
            {
                var wm = Math.Abs(result) / 5;
                wm = wm > input.Wellbeing ? input.Wellbeing : wm;
                status.ActionResults.Add($"A sorry meal has been served reducing the wellbeing score by {wm} for a day.");
            }
        }
    }
}
