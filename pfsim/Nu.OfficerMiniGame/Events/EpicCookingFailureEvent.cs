namespace Nu.OfficerMiniGame
{
    public class EpicCookingFailureEvent : IShipReportEvent
    {
        public string ShipName { get; set; }
        public int WellbeingPenalty { get; set; }

        public int HealthCheckModifier { get; set; }

        public override string ToString()
        {
            return $"Cooking was a failure. WellbeingPenalty: {WellbeingPenalty} HealthCheckModifier: {HealthCheckModifier}";
        }
    }

}
