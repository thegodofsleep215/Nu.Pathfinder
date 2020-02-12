using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class DawnOfANewDayEvent
    {
        public string ShipName { get; set; }

        public bool OpenOcean { get; set; }

        public List<ShipState> CurrentShipStates { get; set; }

        public WeatherConditions WeatherConditions { get; set; }

        public override string ToString()
        {
            return "A new day of sailing has begun.";
        }
    }
}
