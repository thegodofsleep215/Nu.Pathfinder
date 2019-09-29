using System;
using System.Collections.Generic;

namespace pfsim.Officer
{
    /// <summary>
    /// Manage – Allocate resources, tools and supplies from the ship’s stock in order to perform the ship’s daily
    /// necessities such as cooking food, making repairs, etc.   Always the second check of the day.   DC is 
    /// 5 + Ship’s difficulty modifier + 1 / 10 crew.   Failure by means some resources are mismanaged, resulting 
    /// in waste, and the potential loss of ship supplies.  Roll a D20.  If the results are less than or equal to 
    /// the amount that the check was failed by, randomly (1d4) deduct one of food (1), rum (2), water (3), or ship’s 
    /// supplies (4).  Additionally, failure by 10 or more results in obstruction and confusion, resulting in a -4 on 
    /// Cook, Maintain, and Repair checks during that day (including attempts to assist those checks), and a +2 
    /// chance of Discipline problems.  You may have up to two assistants.
    /// </summary>
    public class Manage : IDuty
    {
        public void PerformDuty(Ship ship, ref MiniGameStatus status)
        {
            var dc = 5 + ship.ShipDc + (ship.TotalCrew / 20) - status.CommandModifier;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Manage));
            var job = ship.ManagerJob;
            status.ManageResult = (DiceRoller.D20(1) + job.SkillBonus + assistBonus) - dc;
            if ((status.ManageResult < 0 && status.ManageResult >= -2) || (status.ManageResult <= -10 && status.ManageResult > -12))
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
                        status.ManageResult += 2;
                        status.DutyEvents.Add(new SeaShantyEvent(DutyType.Manage));
                    }
                }
            }
            if(SettingsManager.Verbose)
                status.DutyEvents.Add(new PerformedDutyEvent(DutyType.Manage, job.CrewName, dc, assistBonus, job.SkillBonus, status.ManageResult));

            if (status.ManageResult < 0)
            { 
                var e = new MismanagedSuppliesEvent();
                if (DiceRoller.D20(1) <= (ship.TotalCrew / 10 + 1))  // Possibly replace this random balancer with finer granularity in the future.
                {
                    e.SupplyType = (SupplyType)DiceRoller.D4(1);
                    if (e.SupplyType == SupplyType.ShipSupplies)
                        e.QuantityLost = ShipConstants.ShipSuppliesPerCargoPoint;
                    else
                        e.QuantityLost = ShipConstants.ShipFoodPerCargoPoint;
                }

                if(status.ManageResult <= -10)
                {
                    e.CausedConfusion = true;
                }

                if(e.CausedConfusion || e.SupplyType.HasValue)
                    status.DutyEvents.Add(e);
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
