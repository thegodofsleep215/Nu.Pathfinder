using Nu.Game.Common;
using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nu.OfficerMiniGame
{

    /// <summary>
    /// This class helps sthe <see cref="GameEngine"/> run.
    /// </summary>
    public class Ship
    {
        public string Name { get; set; }

        public ShipSize ShipSize { get; set; }

        public int CrewSize { get; set; }

        public int TotalCrew => CrewSize + ShipsCrew.Count + Marines + Passengers;

        // Below this number, the ship cannot be piloted successfully.
        public int MinimumCrewSize
        {
            get
            {
                if (CrewSize >= 6)
                    return CrewSize / 2;
                else
                    return 1; // Allow boats to be controlled 
            }
        }

        public int ShipDc { get; set; } // General modifier to the difficulty to sail that increases with the size of the ship.

        public int ShipPilotingBonus { get; set; } // Bonus or penalty that the ship recieves for being especially easy to sail.

        public int ShipQuality { get; set; } // Place holder for generic bonus.

        public ShipsCrew ShipsCrew { get; set; }

        public Morale CrewMorale { get; set; } = new Morale();

        public int CrewQuality
        {
            get
            {
                double retval = 0;

                retval = Math.Floor(((retval * ShipsCrew.CountAsCrew) + (Convert.ToDouble(AverageSwabbieQuality) * Swabbies)) / (ShipsCrew.Count + Swabbies)) - 4;

                if (retval > 4)
                    return 4;
                else if (retval < -4)
                    return -4;
                else
                    return (int)retval;
            }
        }

        public DisciplineStandards DisciplineStandards { get; set; }

        public int CrewDisciplineModifier
        {
            get
            {
                switch (ShipsAlignment)
                {
                    case Alignment.Chaotic:
                    return 2;
                    case Alignment.Lawful:
                        return  2;
                    default:
                        return 0;
                }
            }
        }

        public Alignment ShipsAlignment { get; set; }

        public int Marines { get; set; }

        public int Passengers { get; set; }

        public int Swabbies { get; set; }

        public decimal AverageSwabbieQuality { get; set; }

        public int AvailableCrew
        {
            get
            {
                return ShipsCrew.CountAsCrew + Swabbies - DiseasedCrew - CrewUnfitForDuty;
            }
        }

        public int CrewUnfitForDuty { get; set; }

        public bool DiseaseAboardShip
        {
            get
            {
                return DiseasedCrew > 0;
            }
        }

        public int DiseasedCrew { get; set; }

        public int SkeletonCrewPenalty
        {
            get
            {
                int retval = AvailableCrew - CrewSize;

                if (retval > 0)
                    return 0;
                else
                    return retval < -10 ? -10 : retval;
            }
        }

        public bool HasMinimumCrew
        {
            get
            {
                return AvailableCrew - MinimumCrewSize >= 0;
            }
        }

        public string GetRandomCrewName(int count = 1)
        {
            var swabCount = Swabbies + ShipsCrew.CountAsCrew;
            var mates = ShipsCrew.Where(a => a.CountsAsCrew).ToList();
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

                if (result > Swabbies)
                {
                    var mate = mates[result - (Swabbies + 1)];

                    sb.AppendLine(string.Format("{0} {1}", mate.Title, mate.Name).Trim());
                }
                else
                {
                    sb.AppendLine(string.Format("Swabby #{0}", result));
                }
            }

            return sb.ToString();
        }

        public int CrewPilotModifier
        {
            get
            {
                return SkeletonCrewPenalty + ShipPilotingBonus + CrewQuality;
            }
        }


        public int CargoPoints { get; set; }

        public CargoHold ShipsCargo { get; private set; } = new CargoHold();

        public bool IsShipOverburdened
        {
            get
            {
                return CargoPoints < ShipsCargo.TotalCargoPointsUsed;
            }
        }

        public decimal OverburdenedFactor
        {
            get
            {
                return 1; // Not tracking cargo for now.
                if (CargoPoints > ShipsCargo.TotalCargoPointsUsed)
                    return 1;
                else if (CargoPoints == 0 && ShipsCargo.TotalCargoPointsUsed == 0)
                    return 1;
                else if (CargoPoints == 0)
                    return 1 + (((ShipsCargo.TotalCargoPointsUsed * 2) - CargoPoints + 1) / (decimal)CargoPoints + 1);
                else
                    return 1 + ((ShipsCargo.TotalCargoPointsUsed - CargoPoints) / (decimal)CargoPoints);
            }
        }
    }
}
