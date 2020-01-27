using Nu.OfficerMiniGame.Dal.Enums;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class ShipStats
    {
        public string Name { get; set; }

        public ShipType ShipType { get; set; }

        public ShipSize ShipSize { get; set; }

        public List<Propulsion> PropulsionTypes { get; set; } = new List<Propulsion>();

        public int HullHitPoints { get; set; }

        public int OfficerSize { get; set; }

        public int CrewSize { get; set; }

        public int CargoPoints { get; set; }

        public int Passengers { get; set; }

        public int ShipDc { get; set; } // General modifier to the difficulty to sail that increases with the size of the ship.

        public int ShipPilotingBonus { get; set; } // Bonus or penalty that the ship recieves for being especially easy to sail.

        public int ShipQuality { get; set; } // Place holder for generic bonus.

        public List<string> SpecialFeatures { get; set; } = new List<string>();


    }
}
