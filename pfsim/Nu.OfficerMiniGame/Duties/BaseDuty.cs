using Nu.Game.Common;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public abstract class BaseDuty : IDuty
    {
        public abstract List<object> PerformDuty(Ship ship, FleetState state);

        protected int PerformAssists(Ship ship, DutyType duty, int dcModifier = 0)
        {
            int retval = 0;

            ship.ShipsCrew.GetAssistanceBonuses(duty).ForEach(x =>
            {
                retval += (DiceRoller.D20(1) + x >= 10 + dcModifier) ? 2 : 0;
            });

            return retval;
        }

        protected (string CrewName, int SkillBonus) GetDutyBonus(Ship ship, DutyType duty)
        {
            var bonus = -5;
            if (ship.ShipsCrew.JobHasAssignedCrewMember(duty, out var name))
            {
                bonus = ship.ShipsCrew.GetDutyBonus(duty);
            }
            return (name, bonus);
        }

        protected int UseMinstrel(Ship ship, FleetState state, List<object> events, DutyType dutyType)
        {
            var result = 0;
            // Use a ministrel.
            var minstrels = ship.ShipsCrew.Ministrels;

            if (minstrels.Count > state.ShipStates[ship.Name].MinistrelResults.Count)
            {
                var dc = 10 + (ship.TotalCrew / 10) > 15 ? 10 + (ship.TotalCrew / 10) : 15;
                var minstrel = minstrels.First(x => !state.ShipStates[ship.Name].MinistrelResults.ContainsKey(x.Key));
                var shanty = DiceRoller.D20(1) + minstrel.Value - dc;
                state.ShipStates[ship.Name].MinistrelResults[minstrel.Key] = shanty;
                if (shanty >= 0)
                {
                    result = 2;
                    events.Add(new SeaShantyEvent(dutyType) { ShipName = ship.Name });
                }
            }
            return result;
        }
    }
}
