using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nu.Game.Common;
using Nu.OfficerMiniGame.Dal.Dto;
using System;

namespace Nu.OfficerMiniGame.Weather
{
    public interface IWeatherEngine
    {
        WeatherConditions GetWeatherConditions(WeatherInput input);
    }
}
