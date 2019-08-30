﻿using System.Collections.Generic;

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
        public void PerformDuty(IShip ship, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 5 + ship.ShipDc - status.CommandModifier - status.ManageModifier - input.WeatherModifier;
            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Maintain), input.WeatherModifier);
            status.MaintainResult = DiceRoller.D20(1) + ship.MaintainSkillBonus + assistBonus - dc;

            if(status.MaintainResult < 0)
            {
                int damage;
                switch (ship.ShipSize)
                {
                    default:
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
                status.DutyEvents.Add(new PoorMaintenanceEvent { Damage = damage });
            }
        }
        
        private int PerformAssists(List<Assists> list, int modifier)
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
