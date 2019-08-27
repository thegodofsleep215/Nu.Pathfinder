namespace pfsim.Officer
{
    public class PilotFailedEvent
    {
        public int Damage { get; set; }

        public override string ToString()
        {
            return $"A piloting error resulted in {Damage} points of damage to the ship.";
        }
    }

}
