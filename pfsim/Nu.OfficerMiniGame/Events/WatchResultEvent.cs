using Nu.OfficerMiniGame.Dal.Dto;

namespace Nu.OfficerMiniGame
{
    public class WatchResultEvent : IShipReportEvent
    {
        private string[] ordinals = new string[] { "th", "st", "nd", "rd", "th", "th", "th", "th", "th", "th", "th" };

        public string ShipName { get; set; }
        public int Watch { get; set; }
        public bool Success { get; set; }

        public override string ToString()
        {
            return Success ? string.Format("The {0}{1} watch was successful.", Watch, ordinals[Watch % 10]) : string.Format("The {0}{1} watch was a failure.", Watch, ordinals[Watch % 10]);
        }
    }

}
