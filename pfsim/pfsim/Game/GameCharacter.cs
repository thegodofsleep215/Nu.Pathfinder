using pfsim.Events;
using System;

namespace pfsim
{
    public class GameCharacter
    {
        public GameCharacter(Character character, ICharacterInteraction characterInteraction)
        {
            BaseCharacter = character;
            this.characterInteraction = characterInteraction;
            Id = Guid.NewGuid();
            CurrentHitpoints = BaseCharacter.MaxHitPoints;
        }

        public ICharacterInteraction characterInteraction;

        public Guid Id { get; set; }

        public Character BaseCharacter { get; set; }

        public int CurrentHitpoints { get; set; }

        public int Initiative { get; set; }

        public string Team { get; set; }

        public string Affiliation { get; internal set; }

        private Guid? opponent { get; set; }

        public void InitiativeRoll(int initiativeRoll)
        {
            Initiative = BaseCharacter.InitiativeBonus + initiativeRoll;
        }

        public ActionResult TakeAction()
        {
            if (CurrentHitpoints > 0)
            {
                opponent = characterInteraction.FindOpponent(Affiliation);
                if (opponent != null)
                {
                    var result = characterInteraction.Attack(opponent.Value, BaseCharacter.MeleeAttacks[0]);
                    switch (result.AttackType)
                    {
                        case AttackResult.AttackResultType.Hit:
                        case AttackResult.AttackResultType.Crit:
                            return new ActionResult
                            {
                                Message = $"{BaseCharacter.Name} {result.AttackType} {opponent} for {result.Damage} damage."
                            };
                        case AttackResult.AttackResultType.Miss:
                        case AttackResult.AttackResultType.Fumble:
                            return new ActionResult
                            {
                                Message = $"{BaseCharacter.Name}'s attack resulted in a {result.AttackType}"
                            };
                    }
                }
            }
            return new ActionResult { Message = "Nobody to attack!" };
        }
    }
}
