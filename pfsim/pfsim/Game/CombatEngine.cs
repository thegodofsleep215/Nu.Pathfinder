using Nu.Messaging;
using pfsim.Events;
using System.Collections.Generic;

namespace pfsim
{
    public class CombatEngine
    {
        private readonly IMessageRouter messageRouter;

        private List<Character> characters = new List<Character>();

        public CombatEngine(IMessageRouter messageRouter)
        {
            this.messageRouter = messageRouter;
            messageRouter.SubscribeClass(this);
        }

        [Subscription(typeof(CharacterLoaded), "#")]
        public void OnCharacterLoad(CharacterLoaded cl)
        {
            characters.Add(cl.Character);
        }
    }
}
