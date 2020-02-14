using Nu.Game.Common;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
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
    public class Manage : BaseDuty
    {
        public override List<object> PerformDuty(Ship ship, FleetState state)
        {
            var events = new List<object>();
            var dc = 5 + ship.ShipDc + (ship.TotalCrew / 20) - state.ShipStates[ship.Name].CommandModifier;
            var assistBonus = PerformAssists(ship, DutyType.Manage);
            var job = GetDutyBonus(ship, DutyType.Manage);
            var result = (DiceRoller.D20(1) + job.SkillBonus + assistBonus) - dc;
            if ((result < 0 && result >= -2) || (result <= -10 && result > -12))
            {
                result += UseMinstrel(ship, state, events, DutyType.Manage);
            }
            events.Add(new PerformedDutyEvent(ship.Name, DutyType.Manage, job.CrewName, dc, assistBonus, job.SkillBonus, result));

            if (result < 0)
            {
                var e = new MismanagedSuppliesEvent { ShipName = ship.Name };
                if (DiceRoller.D20(1) <= (ship.TotalCrew / 10 + 1))  // Possibly replace this random balancer with finer granularity in the future.
                {
                    e.SupplyType = (SupplyType)DiceRoller.D4(1);
                    if (e.SupplyType == SupplyType.ShipSupplies)
                        e.QuantityLost = ShipConstants.ShipSuppliesPerCargoPoint;
                    else
                        e.QuantityLost = ShipConstants.ShipFoodPerCargoPoint;
                }

                if (result <= -10)
                {
                    e.CausedConfusion = true;
                }

                if (e.CausedConfusion || e.SupplyType.HasValue)
                    events.Add(e);
            }
            return events;
        }
    }
}
