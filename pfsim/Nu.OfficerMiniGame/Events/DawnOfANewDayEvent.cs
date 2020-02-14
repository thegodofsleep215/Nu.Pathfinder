using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class DawnOfANewDayEvent
    {
        public bool OpenOcean { get; set; }

        public bool NarrowPassage { get; set; }

        public bool ShallowWater { get; set; }

        public NightStatus NightStatus { get; set; }

        public List<ShipInput> CurrentShipStates { get; set; }

        public WeatherConditions WeatherConditions { get; set; }

        public override string ToString()
        {
            return "A new day of sailing has begun.";
        }

        public static DawnOfANewDayEvent FromInput(SailingParameters parameters, WeatherConditions weather)
        {
            var result = new DawnOfANewDayEvent();
            result.OpenOcean = parameters.OpenOcean;
            result.NarrowPassage = parameters.NarrowPassage;
            result.ShallowWater = parameters.ShallowWater;
            result.NightStatus = parameters.NightStatus;
            result.WeatherConditions = weather;
            result.CurrentShipStates = parameters.ShipInputs;
            return result;
        }
    }
}
