namespace pfsim.Officer
{
    public class MismanagedSuppliesEvent
    {
        public SupplyType? SupplyType { get; set; }

        public bool CausedConfusion { get; set; }
        public int QuantityLost { get; set; }

        public override string ToString()
        {
            var conf = CausedConfusion ? " The crew is upset at how badly the ship is being managed!" : "";
            if (SupplyType.HasValue)
                return $"Supplies were mismanaged - Lost 1 {SupplyType}.{conf}";
            else
                return conf.Trim();
        }
    }

}
