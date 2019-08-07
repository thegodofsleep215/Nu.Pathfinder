namespace pfsim
{
    public class GameCharacter
    {
        public Character BaseCharacter { get; set; }

        public int CurrentHitpoints { get; set; }

        public int Initiative { get; set; }

        public string Team { get; set; }

        public void InitiativeRoll(int initiativeRoll)
        {
            Initiative = BaseCharacter.InitiativeBonus + initiativeRoll;
        }

        public void TakeAction()
        {
        }
    }
}
