using pfsim.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace pfsim
{
    public class CombatEngine
    {
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
            if (!initiativeEnumerator.MoveNext())
            {
                initiativeEnumerator = characters.GetEnumerator();
                initiativeEnumerator.MoveNext();
            }
            return initiativeEnumerator.Current.TakeAction();
        }


        public void AddCombatant(Character character)
        {
            characters.Add(new GameCharacter(character));
        }
    }
}
