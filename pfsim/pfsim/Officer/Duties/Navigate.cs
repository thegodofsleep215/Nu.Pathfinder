using System;
using System.Collections.Generic;

namespace pfsim.Officer
{
    /// <summary>
    /// Navigate – Determine the ship’s position, set the ship’s course and advise the pilot how 
    /// to maintain it.  DC is 12 if within sight of land, and 17 if in the open sea – modified 
    /// by relevant modifiers from weather.  Increase DC by 5 if the ship sails through the 
    /// night.  Failure by 5 or more means that the ship is lost and not heading in the direction 
    /// that the crew believes.  Without proper compass, maps and other tools give a -5 penalty 
    /// to this check.  Masterwork versions of the same on the other hand give a +2 bonus on the 
    /// check.  You may have 1 assistant.
    public class Navigate : IDuty
    {
        public void PerformDuty(IShip ship, DailyInput input, ref MiniGameStatus status)
        {
            var dc = input.NavigateDc - status.CommandModifier - input.WeatherModifier;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Navigate));
            status.NavigationResult = (DiceRoller.D20(1) + ship.NavigatorSkillBonus + assistBonus) - dc;

            if (status.NavigationResult >= 0)
            {
                status.DutyEvents.Add(new OffCourseEvent());
            }
        }

        private int PerformAssists(List<Assists> list)
        {
            int retval = 0;

            foreach (var assist in list)
            {
                retval += ((DiceRoller.D20(1) + assist.SkillBonus) >= 10) ? 2 : 0;
            }

            return retval;
        }
    }
}
