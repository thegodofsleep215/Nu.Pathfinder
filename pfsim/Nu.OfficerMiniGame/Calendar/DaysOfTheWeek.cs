using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nu.OfficerMiniGame.Calendar
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DaysOfTheWeek
    {
        Moonday,
        Toliday,
        Wealday,
        Oathday,
        Fireday,
        Starday,
        Sunday,
    }
}
