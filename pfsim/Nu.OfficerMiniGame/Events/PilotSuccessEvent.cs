namespace  Nu.OfficerMiniGame
{
    public class PilotSuccessEvent : IShipReportEvent
    {
        public string ShipName { get; set; }
        public override string ToString()
        {
            return "The pilot was successful.";
        }
    }

}
