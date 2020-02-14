namespace  Nu.OfficerMiniGame
{
    public class PoorMaintenanceEvent : IShipReportEvent
    {
        public string ShipName { get; set; }
        public int Damage { get; set; }
        public bool IsOverburdened { get; set; }

        public override string ToString()
        {
            if(IsOverburdened)
                return $"The ship is overburdened and in danger of sinking!  The ship has taken {Damage} points in damage.";
            else
                return $"The ship has taken {Damage} points in damage to the ship and propulsion due to poor maintanence.";
        }
    }

}
