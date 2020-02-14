using Nu.Game.Common;
using Nu.OfficerMiniGame.Dal.Enums;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    /// <summary>
    /// Pilot – Steer the ship and maintain course.   DC is 7 + ship’s DC modifier, plus any relevant
    /// modifiers from failed command and watch checks, and any relevant modifiers from insufficient 
    /// crew, crew quality and weather.   Increase DC by 5 if the ship makes way through the night, 
    /// or by 2 if the ship is not at anchor in the night.   DC increases by +5 if over the course of 
    /// the day the ship in close quarters such as navigating a reef, entering a busy harbor, or 
    /// making passage in river.  DC increases by a further +5 over the course of the day the water 
    /// is particularly shallow.  Failure means that the ship makes poor headway that day, and covers 
    /// only half the usual distance.  Failure by 15 or more means the ship has been badly mishandled.  
    /// If the ship is near shore, in close quarters, or in shallow water, its grounds and takes hull 
    /// damage until a successful pilot check extricates the ship or the ship flounders or sinks.  
    /// If the ship is not near shore, it takes damage to the propulsion.  You may have 3 assistants 
    /// on a ship of up to huge size, and 6 assistants on a larger ship.
    ///
    /// Ship Size   Damage When Mishandled
    /// Large		3d8	
    /// Huge		4d8	
    /// Gargantuan	6d8	
    /// Colossal	8d8	
    public class Pilot : BaseDuty
    {
        public override List<object> PerformDuty(Ship ship, FleetState state)
        {
            var events = new List<object>();
            var weatherModifier = state.GetWeatherModifier(DutyType.Pilot);
            var dc = 7 + ship.ShipDc - state.ShipStates[ship.Name].CommandModifier - CombinedCrewHelpers.CrewPilotModifer(ship, state.ShipStates[ship.Name]) + weatherModifier +
                state.ShipStates[ship.Name].WatchModifier + state.PilotingModifier;

            var job = GetDutyBonus(ship, DutyType.Pilot);
            var assistBonus = PerformAssists(ship, DutyType.Pilot, weatherModifier);
            var result = (DiceRoller.D20(1) + job.SkillBonus + assistBonus) - dc;
            events.Add(new PerformedDutyEvent(ship.Name, DutyType.Pilot, job.CrewName, dc, assistBonus, job.SkillBonus, result));

            if (result >= 0)
            {
                events.Add(new PilotSuccessEvent { ShipName = ship.Name });
            }
            if (result < 0)
            {
                int damage = 0;
                if (result <= -15)
                {
                    switch (ship.ShipSize)
                    {
                        case ShipSize.Medium:
                            damage = DiceRoller.D8(2);
                            break;
                        case ShipSize.Large:
                            damage = DiceRoller.D8(3);
                            break;
                        case ShipSize.Huge:
                            damage = DiceRoller.D8(4);
                            break;
                        case ShipSize.Gargantuan:
                            damage = DiceRoller.D8(6);
                            break;
                        case ShipSize.Colossal:
                            damage = DiceRoller.D8(8);
                            break;
                    }
                    events.Add(new PilotFailedEvent
                    {
                        ShipName = ship.Name,
                        Damage = damage
                    });
                }
                else
                {
                    events.Add(new PilotFailedEvent
                    {
                        ShipName = ship.Name,
                        Damage = 0
                    });
                }
            }

            return events;
        }

    }
}
