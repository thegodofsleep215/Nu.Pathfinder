using Nu.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nu.OfficerMiniGame
{
    public static class CombinedCrewHelpers
    {
        public static int CrewPilotModifer(Ship ship, ShipState shipState)
        {
            return SkeletonCrewPenalty(ship, shipState) + ship.ShipPilotingBonus + CrewQuality(ship, shipState) + ship.ShipQuality;
        }

        private static int SkeletonCrewPenalty(Ship ship, ShipState shipState)
        {
            int retval = AvailableCrew(ship, shipState) - ship.CrewSize;

            if (retval > 0)
                return 0;
            else
                return retval < -10 ? -10 : retval;
        }

        private static int AvailableCrew(Ship ship, ShipState shipState)
        {
            return ship.ShipsCrew.CountAsCrew + shipState.Swabbies - shipState.DiseasedCrew - shipState.CrewUnfitForDuty;
        }

        private static int CrewQuality(Ship ship, ShipState shipState)
        {
            double retval = 0;

            retval = Math.Floor(((retval * ship.ShipsCrew.CountAsCrew) + (Convert.ToDouble(ship.AverageSwabbieQuality) * shipState.Swabbies)) / (ship.ShipsCrew.Count + shipState.Swabbies)) - 4;

            if (retval > 4)
                return 4;
            else if (retval < -4)
                return -4;
            else
                return (int)retval;
        }
        public static string GetRandomCrewName(Ship ship, ShipState shipState, int count = 1)
        {
            var swabCount = shipState.Swabbies + ship.ShipsCrew.CountAsCrew;
            var mates = ship.ShipsCrew.Where(a => a.CountsAsCrew).ToList();
            HashSet<int> picked = new HashSet<int>();
            StringBuilder sb = new StringBuilder();
            int result;

            if (count > swabCount)
                count = swabCount;

            for (int i = 0; i < count; i++)
            {
                do
                {
                    result = DiceRoller.Roll(swabCount, 1);
                }
                while (picked.Contains(result));

                picked.Add(result);

                if (result > shipState.Swabbies)
                {
                    var mate = mates[result - (shipState.Swabbies + 1)];

                    sb.AppendLine(string.Format("{0} {1}", mate.Title, mate.Name).Trim());
                }
                else
                {
                    sb.AppendLine(string.Format("Swabby #{0}", result));
                }
            }

            return sb.ToString();
        }

    }

}
