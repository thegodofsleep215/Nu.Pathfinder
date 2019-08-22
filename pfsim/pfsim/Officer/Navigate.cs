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
        public void PerformDuty(IShip crew, ref MiniGameStatus status)
        {
            var dc = status.NavigateDc + status.CommandModifier + status.WeatherModifier;
            status.NavigationResult = (DiceRoller.D20(1) + crew.NavigatorSkillBonus) - dc;

            if (status.NavigationResult >= 0)
            {
                status.ActionResults.Add("On course.");
            }
            else
            {
                status.ActionResults.Add("Off course.");
            }
        }
    }
}
