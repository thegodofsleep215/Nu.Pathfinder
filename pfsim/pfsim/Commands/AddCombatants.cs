namespace pfsim.Commands
{
    public class AddCombatants
    {
        public string Name { get; set; }

        public int Quantity { get; set; }
        public string Affiliation { get; internal set; }
    }
}
