using Nu.Game.Common;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    /// <summary>
    /// Watch – Keep watch over the ship.  Always the third check of the day.   Two or three checks are required 
    /// over the course of the day depending on whether the ship makes way at night, but which ever officer makes
    /// the Pilot check may make one of these checks as a free action if they so desire.  DC is 10, modified by
    /// weather conditions.  Failure of any watch results in a -2 penalty on Pilot checks for that day, 
    /// cumulative for each watch failure.  Success on a watch results in a +2 circumstance bonus on initiative
    /// checks for encounters during that watch.  You may have 1 assistant on each watch. 
    /// </summary>
    public class Watch : BaseDuty
    {
        private WatchShift watchShift;

        public Watch(WatchShift watchShift)
        {
            this.watchShift = watchShift;
        }

        public override List<object> PerformDuty(Ship ship, FleetState state)
        {
            var events = new List<object>();
            var weatherModifier =  state.GetWatchModifier(watchShift);
            var dc = 10 + weatherModifier - state.ShipStates[ship.Name].CommandModifier;
            var assistBonus = PerformAssists(ship, state, weatherModifier);

            int watch = state.ShipStates[ship.Name].WatchResults.Count;

            var result = -5;
            if(ship.ShipsCrew.WatchBonuses.Count > watch)
                result = (DiceRoller.D20(1) + ship.ShipsCrew.WatchBonuses[watch] + assistBonus) - dc;
            state.ShipStates[ship.Name].WatchResults.Add(result);
            events.Add(new WatchResultEvent
            {
                ShipName = ship.Name,
                Watch = watch + 1,
                Success = result >= 0
            });
            return events;
        }

        private int PerformAssists(Ship ship, FleetState state, int weatherModifier)
        {
            var list = ship.ShipsCrew.GetWatchBonuses();
            int retval = 0;
            int watch = state.ShipStates[ship.Name].WatchResults.Count;

            if (list.Count > watch)
            {
                var assist = list[watch - 1];
                retval += ((DiceRoller.D20(1) + assist) >= (10 - weatherModifier)) ? 2 : 0;
            }

            return retval;
        }
    }
}
