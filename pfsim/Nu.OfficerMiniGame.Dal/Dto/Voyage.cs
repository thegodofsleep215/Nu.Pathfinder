using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class Voyage
    {
        public string Name { get; set; }

        public string[] ShipLoadouts { get; set; }
        public Dictionary<string, List<VoyageEvent>> Events { get; set; } = new Dictionary<string, List<VoyageEvent>>();

        public void AddEvents(Dictionary<string, List<object>> events)
        {
            events.ToList().ForEach(x => AddEvents(x.Key, x.Value));
        }

        public void AddEventToAllShips(object evt)
        {
            ShipLoadouts.ToList().ForEach(x => AddEvents(x, new List<object> { evt }));
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
