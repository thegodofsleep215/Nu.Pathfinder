namespace  Nu.OfficerMiniGame
{
    public class PilotSuccessEvent
    {
        public string ShipName { get; set; }
        public override string ToString()
        {
            return "The pilot was successful.";
        }
    }

}
