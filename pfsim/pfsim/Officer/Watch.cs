namespace pfsim.Officer
{
    /// <summary>
    /// Watch – Keep watch over the ship.  Always the third check of the day.   Two or three checks are required 
    /// over the course of the day depending on whether the ship makes way at night, but which ever officer makes
    /// the Pilot check may make one of these checks as a free action if they so desire.  DC is 10, modified by
    /// weather conditions.   Failure of any watch results in a -2 penalty on Pilot checks for that day, 
    /// cumulative for each watch failure.   Success on a watch results in a +2 circumstance bonus on initiative
    /// checks for encounters during that watch.   You may have 1 assistant on each watch. 
    /// </summary>
    public class Watch : IDuty
    {
        public void PerformDuty(Crew crew, ref MiniGameStatus status)
        {
            var dc = 10 + status.WeatherModifier + status.CommandModifier;
            status.WatchResult = (DiceRoller.D20(1) + crew.FirstWatchBonus) - dc;
            status.ActionResults.Add($"Watch Result: {status.WatchResult}");
        }
    }
}
