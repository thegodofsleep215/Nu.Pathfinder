using Nu.Messaging;
using pfsim.Commands;
using pfsim.Events;
using System.Collections.Generic;
using System.Linq;

namespace pfsim
{
    public class CombatManager
    {
        private readonly IMessageRouter messageRouter;

        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CombatEngine activeCombat;

        public CombatManager(IMessageRouter messageRouter, List<Character> characters)
        {
            this.messageRouter = messageRouter;
            messageRouter.SubscribeClass(this);
            this.characters = characters.ToDictionary(x => x.Name.ToLower(), x => x);
            activeCombat = new CombatEngine();
        }

        [Subscription("#")]
        public void OnCharacterLoad(CharacterLoaded cl)
        {
            characters[cl.Character.Name.ToLower()] = (cl.Character);
        }
        [Subscription("#")]
        public void OnAddCombatants(AddCombatants ac)
        {
            if (activeCombat != null)
            {
                Enumerable.Repeat(characters[ac.Name.ToLower()], ac.Quantity).ToList().ForEach(x => activeCombat.AddCombatant(x, ac.Affiliation));
            }
        }

        [Subscription("#")]
        public void OnRollForInitiative(RollForInitiative rfi)
        {
            if (activeCombat != null)
            {
                activeCombat.RollForInitiative();
            }
        }

        [RemoteCall]
        public ActionResult OnNextCombatAction(NextCombatAction nca)
        {
            if (activeCombat != null)
            {
                return activeCombat.NextCombatAction();
            }
            return new ActionResult { Message = "No active combat." };

        }
    }
}
