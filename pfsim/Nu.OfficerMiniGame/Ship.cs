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

        public int DisciplineAlignmentModifier
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

        public decimal AverageSwabbieQuality { get; set; }

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
