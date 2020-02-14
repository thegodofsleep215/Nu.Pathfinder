namespace  Nu.OfficerMiniGame
{
    public class PilotFailedEvent : IShipReportEvent
    {
        public string ShipName { get; set; }
        public int Damage { get; set; }

        public override string ToString()
        {
            if (Damage > 0)
                return $"A piloting error resulted in {Damage} points of damage to the ship.";
            else
                return $"Poor piloting resulted in reduced ship progress for the day.";
        }
    }

}
