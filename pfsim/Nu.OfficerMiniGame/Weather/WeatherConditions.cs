using Nu.OfficerMiniGame.Calendar;

namespace Nu.OfficerMiniGame.Weather
{
    public class WeatherConditions
    {
        public WeatherStatus WeatherStatus { get; set; }

        public int TemperatureInF { get; set; }

        public int DurationOfTemperature { get; set; }

        public PrecipitationType PrecipitationType { get; internal set; }

        public int PreciptationStartTime { get; internal set; }

        public int PrecipitationHours { get; internal set; }

        public WindSpeed WindSpeed { get; internal set; }
        
        public CloudCover CloudCover { get; internal set; }

        public PfDateTime Date { get; set; }
    }
}
