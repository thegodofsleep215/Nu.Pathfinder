using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class Voyage
    {
        public string Name { get; set; }

        public List<VoyageEvent> Events { get; set; } = new List<VoyageEvent>();

        public void AddEvents(List<object> events)
        {
            var nextNumber = !Events.Any() ? 1 : Events.Max(x => x.EventNumber) + 1;
            events.ForEach(x =>
            {
                var ve = new VoyageEvent
                {
                    EventName = x.GetType().AssemblyQualifiedName,
                    EventData = JsonConvert.SerializeObject(x),
                    EventNumber = nextNumber
                };
                nextNumber++;
                Events.Add(ve);
            });
        }

        public void AddEvent(object evt)
        {
            var nextNumber = !Events.Any() ? 1 : Events.Max(x => x.EventNumber) + 1;
            var ve = new VoyageEvent
            {
                EventName = evt.GetType().AssemblyQualifiedName,
                EventData = JsonConvert.SerializeObject(evt),
                EventNumber = nextNumber
            };
            nextNumber++;
            Events.Add(ve);

        }
    }
}
