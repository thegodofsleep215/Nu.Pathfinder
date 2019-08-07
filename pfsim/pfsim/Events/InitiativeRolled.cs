namespace pfsim.Events
{
    public class InitiativeRolled
    {
        public string CharacterName { get; set; }

        public int Initiative { get; set; }
    }

    public class CharacterLoaded
    {
        public static string RoutingKey = "Character.Loaded";
        public Character Character { get; set; }
    }
}
