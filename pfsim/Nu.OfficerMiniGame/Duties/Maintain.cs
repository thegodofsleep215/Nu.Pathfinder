using Nu.Game.Common;
using Nu.OfficerMiniGame.Dal.Enums;
using System;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    /// <summary>
    /// 
    /// Maintain – Keep the ship shipshape. DC is 5 plus the ship’s DC modifier, 
    /// with penalties or bonus from command and manage checks, ship’s morale, 
    /// and weather.   Failure indicates that both hull and propulsion take 
    /// damage from decay and neglect.You may have up to 3 assistants.
    ///
    ///Ship Size   Damage from Neglect
    ///Large		1	
    ///Huge        1d3	
    ///Gargantuan  1d4	
    ///Colossal    1d6	
    /// </summary>
    public class Maintain : BaseDuty
    {
        public override List<object> PerformDuty(Ship ship, FleetState state)
        {
            var events = new List<object>();
            var weatherModifier = state.GetWeatherModifier(DutyType.Maintain);
            var dc = 5 + ship.ShipDc - state.ShipStates[ship.Name].CommandModifier - state.ShipStates[ship.Name].ManageModifier + weatherModifier;
            dc += ship.IsShipOverburdened ? (int)Math.Ceiling(ship.OverburdenedFactor + 1) : 0;
            var assistBonus = PerformAssists(ship, DutyType.Maintain, weatherModifier);
            var job = GetDutyBonus(ship, DutyType.Maintain);
            var result = DiceRoller.D20(1) + job.SkillBonus + assistBonus - dc;
            events.Add(new PerformedDutyEvent(ship.Name, DutyType.Maintain, job.CrewName, dc, assistBonus, job.SkillBonus, result));

            if (result < 0)
            {
                int damage;
                switch (ship.ShipSize)
                {
                    default:
                    case ShipSize.Medium:
                    case ShipSize.Large:
                        damage = 1;
                        break;
                    case ShipSize.Huge:
                        damage = DiceRoller.D3(1);
                        break;
                    case ShipSize.Gargantuan:
                        damage = DiceRoller.D4(1);
                        break;
                    case ShipSize.Colossal:
                        damage = DiceRoller.D6(1);
                        break;
                }
                damage = (int)Math.Ceiling(damage * ship.OverburdenedFactor);

                events.Add(new PoorMaintenanceEvent { ShipName = ship.Name, Damage = damage });
            }
            return events;
        }
    }
}
