using Nu.OfficerMiniGame.Dal.Enums;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class WeatherConditions
    {
        public int TemperatureInF { get; set; }

        public int DurationOfTemperature { get; set; }

        public PrecipitationType PrecipitationType { get; set; }

        public int PrecipitationStartTime { get; set; }

        public int PrecipitationHours { get; set; }

        public WindSpeed WindSpeed { get; set; }

        public CloudCover CloudCover { get; set; }

        public PfDateTime Date { get; set; }
    }
}
