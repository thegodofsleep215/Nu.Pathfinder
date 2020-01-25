using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace Nu.OfficerMiniGame.Calendar
{
    public class PfDateTimeJsonConverter : JsonConverter
    {
        private static Regex read = new Regex(@"(?<year>\d+)/(?<month>\d+)/(?<day>\d+)\s(?<hour>\d+):(?<minute>\d+):(?<second>\d+)");
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetType() == typeof(PfDateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var v = (string)reader.Value;
            var match = read.Match(v);
            var year = int.Parse(match.Groups["year"].Value);
            var month = int.Parse(match.Groups["month"].Value);
            var day = int.Parse(match.Groups["day"].Value);
            var hour = int.Parse(match.Groups["hour"].Value);
            var minute = int.Parse(match.Groups["minute"].Value);
            var second = int.Parse(match.Groups["second"].Value);

            return new PfDateTime(year, month, day, hour, minute, second);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dt = (PfDateTime)value;
            var outString = $"{dt.Year}/{dt.Month}/{dt.Day} {dt.Hour}:{dt.Minute}:{dt.Second}";
            writer.WriteValue(outString);
        }
    }
}
