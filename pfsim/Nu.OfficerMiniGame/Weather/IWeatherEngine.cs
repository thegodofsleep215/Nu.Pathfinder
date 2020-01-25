using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nu.Game.Common;
using System;

namespace Nu.OfficerMiniGame.Weather
{
    public interface IWeatherEngine
    {
        WeatherConditions GetWeatherConditions(WeatherInput input);
    }
}
