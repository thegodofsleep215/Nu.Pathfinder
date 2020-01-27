using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class Voyage
    {
        public string Name { get; set; }

        public PfDateTime StartDate { get; set; }

        public string Port { get; set; }

        public string DestinationPort { get; set; }

        public string[] ShipLoadouts { get; set; }

        public int DaysPlanned { get; set; }

        public NightStatus NightStatus { get; set; }

        public bool OpenOcean { get; set; }

        public bool ShallowWater { get; set; }

        public bool NarrowPassage { get; set; }

        public bool Underweigh { get; set; }

        public DisciplineStandards DisciplineStandards { get; set; }

        public List<SwabbieCount> Swabbies { get; set; }

        public Dictionary<string, List<VoyageEvent>> Events { get; set; } = new Dictionary<string, List<VoyageEvent>>();

        public List<WeatherConditions> weatherConditions { get; set; } = new List<WeatherConditions>();

        public void AddEvents(Dictionary<string, List<object>> events)
        {
            events.ToList().ForEach(x => AddEvents(x.Key, x.Value));
        }

        public void AddEvents(string shipName, List<object> events)
        {
            if (!Events.ContainsKey(shipName))
            {
                Events[shipName] = new List<VoyageEvent>();
            }

            var nextNumber = !Events[shipName].Any() ? 1 : Events[shipName].Max(x => x.EventNumber) + 1;
            events.ForEach(x =>
            {
                var ve = new VoyageEvent
                {
                    EventName = x.GetType().AssemblyQualifiedName,
                    EventData = JsonConvert.SerializeObject(x),
                    EventNumber = nextNumber
                };
                nextNumber++;
                Events[shipName].Add(ve);
            });
        }
    }
}
