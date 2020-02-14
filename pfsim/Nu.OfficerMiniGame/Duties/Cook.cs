using Nu.Game.Common;
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
    public class Cook : BaseDuty
    {
        public override List<object> PerformDuty(Ship ship, FleetState state)
        {
            var events = new List<object>();
            var dc = 7 + (ship.TotalCrew / 10) - state.ShipStates[ship.Name].ManageModifier - ship.CrewMorale.WellBeing;
            if (dc < 10)
                dc = 10;
            var assistBonus = PerformAssists(ship, DutyType.Cook);
            var job = GetDutyBonus(ship, DutyType.Cook);
            var result = DiceRoller.D20(1) + assistBonus + job.SkillBonus - dc;

            events.Add(new PerformedDutyEvent(ship.Name, DutyType.Cook, job.CrewName, dc, assistBonus, job.SkillBonus, result));

            if (result < 0)
            {
                var wm = Math.Abs(result) / 5 + 1;
                wm = wm > ship.CrewMorale.WellBeing ? ship.CrewMorale.WellBeing : wm;
                events.Add(new EpicCookingFailureEvent
                {
                    ShipName = ship.Name,
                    WellbeingPenalty = wm * -1,
                    HealthCheckModifier = result <= -15 ? 4 : 0
                });
            }
            return events;
        }
    }
}
