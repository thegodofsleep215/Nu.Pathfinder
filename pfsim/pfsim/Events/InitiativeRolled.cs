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

    public class AttackResult
    {
        public enum AttackResultType
        {
            Miss,
            Hit,
            Crit,
            Fumble
        }

        public int RollToHit { get; set; }

        public AttackResultType AttackType { get; set; }

        public int Damage { get; set; }
    }
}
