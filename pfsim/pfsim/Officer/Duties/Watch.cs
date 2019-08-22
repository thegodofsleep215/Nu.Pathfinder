using System.Collections.Generic;

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
        public void PerformDuty(IShip crew, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 10 + status.WeatherModifier + status.CommandModifier;
            var assistBonus = PerformAssists(crew.GetAssistance(DutyType.Watch), status);
            var result = (DiceRoller.D20(1) + crew.FirstWatchBonus + assistBonus) - dc;
            status.WatchResults.Add(result);
            status.ActionResults.Add($"Watch Result #{status.WatchResults.Count}: {result}");
        }

        private int PerformAssists(List<Assists> list, MiniGameStatus status)
        {
            int retval = 0;
            int watch = status.WatchResults.Count;

            if(list.Count <= watch)
            {
                var assist = list[watch - 1];
                retval += ((DiceRoller.D20(1) + assist.SkillBonus) >= (10 + status.WeatherModifier)) ? 2 : 0;
            }

            return retval;
        }
    }

}
