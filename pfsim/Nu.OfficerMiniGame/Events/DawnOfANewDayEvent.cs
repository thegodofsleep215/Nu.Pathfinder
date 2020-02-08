﻿using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class DawnOfANewDayEvent
    {
        public bool OpenOcean { get; set; }

        public NightStatus NightStatus { get; internal set; }

        public WeatherConditions WeatherConditions { get; set; }

        public override string ToString()
        {
            return "A new day of sailing has begun.";
        }
    }

}
