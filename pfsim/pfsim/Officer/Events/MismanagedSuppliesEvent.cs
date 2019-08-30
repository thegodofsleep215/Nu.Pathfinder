namespace pfsim.Officer
{
    public class MismanagedSuppliesEvent
    {
        public SupplyType SupplyType { get; set; }

        public bool CausedConfusion { get; set; }

        public override string ToString()
        {
            var conf = CausedConfusion ? "It was so based it caused confustion amongst the crew." : "";
            return $"Supplies were miss managed Lost 1 {SupplyType}.{conf}";
        }
    }

}
