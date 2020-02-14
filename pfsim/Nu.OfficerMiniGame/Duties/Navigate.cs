using Nu.Game.Common;
using System;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    /// <summary>
    /// Navigate – Determine the ship’s position, set the ship’s course and advise the pilot how 
    /// to maintain it.  DC is 12 if within sight of land, and 17 if in the open sea – modified 
    /// by relevant modifiers from weather.  Increase DC by 5 if the ship sails through the 
    /// night.  Failure by 5 or more means that the ship is lost and not heading in the direction 
    /// that the crew believes.  Without proper compass, maps and other tools give a -5 penalty 
    /// to this check.  Masterwork versions of the same on the other hand give a +2 bonus on the 
    /// check.  You may have 1 assistant.
    public class Navigate : BaseDuty
    {
        public override List<object> PerformDuty(Ship ship, FleetState state)
        {
            var events = new List<object>();
            var dc = state.ShipStates[ship.Name].CommandModifier + state.NavigationModifier;
            var assistBonus = PerformAssists(ship, DutyType.Navigate);
            var job = GetDutyBonus(ship, DutyType.Navigate);
            var result = (DiceRoller.D20(1) + job.SkillBonus + assistBonus) - dc + 3; // The +3 because I am assuming compass + map
            events.Add(new PerformedDutyEvent(ship.Name, DutyType.Navigate, job.CrewName, dc, assistBonus, job.SkillBonus, result));

            if (result <= -5)
            {
               events.Add(new OffCourseEvent(true) { ShipName = ship.Name });
            }
            else if (result < 0)
            {
                events.Add(new OffCourseEvent(false) { ShipName = ship.Name });
            }
            return events;
        }
    }
}
