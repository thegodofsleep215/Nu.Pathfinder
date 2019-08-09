using pfsim.Events;

namespace pfsim
{
    public class GameCharacter
    {
        public GameCharacter(Character character)
        {
            BaseCharacter = character;
        }
        public Character BaseCharacter { get; set; }

        public int CurrentHitpoints { get; set; }

        public int Initiative { get; set; }

        public string Team { get; set; }

        public void InitiativeRoll(int initiativeRoll)
        {
            Initiative = BaseCharacter.InitiativeBonus + initiativeRoll;
        }

        public ActionResult TakeAction()
        {
            return new ActionResult
            {
                Message = $"{BaseCharacter.Name} took an action!"
            };
        }
    }
}
