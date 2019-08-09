namespace pfsim.Events
{
    public class InitiativeRolled
    {
        public string CharacterName { get; set; }

        public int Initiative { get; set; }
    }

    public class ActionResult
    {
        public string Message { get; set; }
    }
}
