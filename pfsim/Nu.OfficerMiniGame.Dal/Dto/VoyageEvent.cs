using Newtonsoft.Json;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class VoyageEvent
    {
        public string EventName { get; set; }

        public int EventNumber { get; set; }

        public string EventData { get; set; }

        public object Event
        {
            get
            {
                var type = System.Type.GetType(EventName);
                return JsonConvert.DeserializeObject(EventData, type);
            }
        }
    }
}
