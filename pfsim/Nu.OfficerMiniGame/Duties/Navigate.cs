﻿using Nu.Game.Common;
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
    public class Navigate : IDuty
    {
        public void PerformDuty(Ship ship, bool verbose, ref MiniGameStatus status)
        {
            var dc = ship.TemporaryNavigationModifier - status.CommandModifier;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Navigate));
            var job = ship.NavigatorJob;
            status.NavigationResult = (DiceRoller.D20(1) + job.SkillBonus + assistBonus) - dc + 3; // The +3 because I am assuming compass + map
            if(verbose)
                status.GameEvents.Add(new PerformedDutyEvent(DutyType.Navigate, job.CrewName, dc, assistBonus, job.SkillBonus, status.NavigationResult));

            if (status.NavigationResult <= -5)
            {
                status.GameEvents.Add(new OffCourseEvent(true));
            }
            else if (status.NavigationResult < 0)
            {
                status.GameEvents.Add(new OffCourseEvent(false));
            }
        }

        private int PerformAssists(List<JobMessage> list)
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
