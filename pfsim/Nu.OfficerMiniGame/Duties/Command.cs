using Nu.Game.Common;
using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    /// <summary>
    /// Command – Issue instructions to crew.  Always first check of day.  DC is 5 + Ship’s difficulty modifier + 1 / 10 crew.
    /// The ship’s morale bonus is a bonus or penalty to any command check.   Failure by 5 or more results in a -2 check on
    /// all other job checks of that day, and each failure by each additional 5 beyond the first results in a further
    /// -2 penalty.  Failure by 15 or more additionally results in a +4 chance of discipline problems.  Success by 5 or more
    /// results in a +2 bonus on all job checks for that day.  You may have up to two assistants, however.
    /// </summary>
    public class Command : BaseDuty
    {
        public override List<object> PerformDuty(Ship ship, FleetState state)
        {
            var events = new List<object>();
            var dc = 5 + ship.ShipDc + (ship.TotalCrew / 10) + state.ShipStates[ship.Name].TemporaryCommandModifier;
            var assistBonus = PerformAssists(ship, DutyType.Command);
            var job = GetDutyBonus(ship, DutyType.Command);
            var result = (DiceRoller.D20(1) + job.SkillBonus + assistBonus) - dc;
            if (result < 0 && result >= -2)
            {
                result += UseMinstrel(ship, state, events, DutyType.Command);
            }
            events.Add(new PerformedDutyEvent(ship.Name, DutyType.Command, job.CrewName, dc, assistBonus, job.SkillBonus, result));
            return events;
        }
    }
}
