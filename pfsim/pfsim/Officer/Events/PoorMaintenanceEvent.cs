namespace pfsim.Officer
{
    public class PoorMaintenanceEvent
    {
        public int Damage { get; set; }

        public override string ToString()
        {
            return $"The ship has taken {Damage} points in damage to the ship and propulsion due to poor maintanence.";
        }
    }

}
