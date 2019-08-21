namespace pfsim.Officer
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
    public class Pilot : IDuty
    {
        public void PerformDuty(Crew crew, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 7 + crew.ShipDc + status.CommandModifier + status.WatchModifier
                + crew.CrewPilotModifier + input.SailingModifiers;
            status.PilotResult = (DiceRoller.D20(1) + crew.PilotSkillBonus) - dc;

            if (status.PilotResult >= 0)
            {
                status.ActionResults.Add("Piloting successful.");
            }
            else
            {
                status.ActionResults.Add("Piloting failed, progress halved.");
                if(status.PilotResult <= -15)
                {
                    int damage = 0;
                    switch (crew.ShipSize)
                    {
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
                    status.ActionResults.Add($"Piloting faled so bad thate the ship took {damage} points of damage.");
                }
            }
        }
    }
}
