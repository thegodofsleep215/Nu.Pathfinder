namespace pfsim.Events
{
    public class CharacterLoaded
    {
        public static string RoutingKey = "Character.Loaded";
        public Character Character { get; set; }
    }
}
