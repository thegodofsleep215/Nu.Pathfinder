﻿using System;
using System.Collections.Generic;

namespace pfsim.Officer
{
    /// <summary>
    /// Manage – Allocate resources, tools and supplies from the ship’s stock in order to perform the ship’s daily
    /// necessities such as cooking food, making repairs, etc.   Always the second check of the day.   DC is 
    /// 5 + Ship’s difficulty modifier + 1 / 10 crew.   Failure by means some resources are mismanaged, resulting 
    /// in waste, and the potential loss of ship supplies.   Roll a D20.  If the results are less than or equal to 
    /// the amount that the check was failed by, randomly (1d4) deduct one of food (1), rum (2), water (3), or ship’s 
    /// supplies (4).  Additionally, failure by 10 or more results in obstruction and confusion, resulting in a -4 on 
    /// Cook, Maintain, and Repair checks during that day (including attempts to assist those checks), and a +2 
    /// chance of Discipline problems.  You may have up to two assistants.
    /// </summary>
    public class Manage : IDuty
    {
        public void PerformDuty(IShip ship, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 5 + ship.ShipDc + (ship.TotalCrew / 10) - status.CommandModifier;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Manage));
            status.ManageResult = (DiceRoller.D20(1) + ship.ManagerSkillBonus) - dc;

            if (status.ManageResult < 0)
            {
                status.DutyEvents.Add(new MismanagedSuppliesEvent {
                    SupplyType = (SupplyType)DiceRoller.D4(1),
                });
            }
            if (status.ManageResult <= -10)
            {
                status.DutyEvents.Add("The crew is upset at how the resources are being poorly managed.");
                // TODO: Apply horribly mismanaged penalty.
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
