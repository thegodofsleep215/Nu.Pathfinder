using pfsim.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace pfsim
{
    public interface ICharacterInteraction
    {
        Guid? FindOpponent(string attackersAffiliation);

        AttackResult Attack(Guid opponentId, IWeaponAttack weapon);
    }

    public class CombatEngine : ICharacterInteraction
    {
        private DiceRoller roller = new DiceRoller();

        class GameCharacterCollection : IEnumerable<GameCharacter>
        {
            private List<GameCharacter> gameCharacters = new List<GameCharacter>();


            public IEnumerator<GameCharacter> GetEnumerator()
            {
                var initiativeOrder = gameCharacters.OrderByDescending(x => x.Initiative).ThenByDescending(x => x.BaseCharacter.Dexterity);
                foreach (var gc in initiativeOrder)
                {
                    yield return gc;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void ForEach(Action<GameCharacter> action)
            {
                gameCharacters.ForEach(action);
            }

            public void Add(GameCharacter character)
            {
                gameCharacters.Add(character);
            }

            public GameCharacter this[Guid id]
            {
                get
                {
                    return gameCharacters.FirstOrDefault(x => x.Id == id);
                }
            }
        }

        private DiceRoller diceRoller = new DiceRoller();

        private GameCharacterCollection characters = new GameCharacterCollection();

        private IEnumerator<GameCharacter> initiativeEnumerator;

        public CombatEngine()
        {
        }

        public void RollForInitiative()
        {
            characters.ForEach(x => x.InitiativeRoll(diceRoller.D20(1)));
            initiativeEnumerator = characters.GetEnumerator();
        }

        public ActionResult NextCombatAction()
        {
            if (initiativeEnumerator == null) return new ActionResult { Message = "You should roll for initiative." };
            if (!initiativeEnumerator.MoveNext())
            {
                initiativeEnumerator = characters.GetEnumerator();
                initiativeEnumerator.MoveNext();
            }
            return initiativeEnumerator.Current.TakeAction();
        }


        public void AddCombatant(Character character, string affiliation)
        {
            var gc = new GameCharacter(character, this)
            {
                Affiliation = affiliation
            };
            characters.Add(gc);
        }

        public Guid? FindOpponent(string attackersAffiliation)
        {
            var opponent = characters.FirstOrDefault(x => x.Affiliation != attackersAffiliation && x.CurrentHitpoints > 0);
            return opponent?.Id;
        }

        public AttackResult Attack(Guid opponentId, IWeaponAttack weapon)
        {
            var result = new AttackResult
            {
                RollToHit = weapon.RollToHit(roller)
            };
            if (result.RollToHit >= characters[opponentId].BaseCharacter.AC)
            {
                if (result.RollToHit >= weapon.StartOfCritRange && weapon.RollToHit(roller) >= characters[opponentId].BaseCharacter.AC)
                {
                    result.AttackType = AttackResult.AttackResultType.Crit;
                    result.Damage = weapon.RollDamage(roller, true);
                }
                else
                {
                    result.AttackType = AttackResult.AttackResultType.Hit;
                    result.Damage = weapon.RollDamage(roller, false);
                }
                characters[opponentId].CurrentHitpoints -= result.Damage;
            }
            else
            {
                result.AttackType = result.RollToHit == 1 ? AttackResult.AttackResultType.Fumble : AttackResult.AttackResultType.Miss;
            }
            return result;
        }
    }
}
