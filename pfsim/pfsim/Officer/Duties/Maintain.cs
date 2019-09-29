using System;
using System.Collections.Generic;

namespace pfsim.Officer
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
    public class Maintain : IDuty
    {
        public void PerformDuty(Ship ship, ref MiniGameStatus status)
        {
            var weatherModifier = ship.CurrentVoyage.GetWeatherModifier(DutyType.Maintain);
            var dc = 5 + ship.ShipDc - status.CommandModifier - status.ManageModifier - weatherModifier;
            dc += ship.IsShipOverburdened ? (int)Math.Ceiling(ship.OverburdenedFactor + 1) : 0;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Maintain), weatherModifier);
            var job = ship.MaintainJob;
            status.MaintainResult = DiceRoller.D20(1) + job.SkillBonus + assistBonus - dc;
            if(SettingsManager.Verbose)
                status.DutyEvents.Add(new PerformedDutyEvent(DutyType.Maintain, job.CrewName, dc, assistBonus, job.SkillBonus, status.MaintainResult));

            if (status.MaintainResult < 0)
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

                status.DutyEvents.Add(new PoorMaintenanceEvent { Damage = damage });
            }
        }
        
        private int PerformAssists(List<JobMessage> list, int modifier)
        {
            int retval = 0;

            foreach (var assist in list)
            {
                retval += ((DiceRoller.D20(1) + assist.SkillBonus) >= (10 - modifier)) ? 2 : 0;
            }

            return retval;
        }
    }
}
