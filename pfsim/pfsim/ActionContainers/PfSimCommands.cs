using Newtonsoft.Json;
using Nu.CommandLine.Attributes;
using Nu.Messaging;
using pfsim.Commands;
using pfsim.Events;
using System.Collections.Generic;
using System.IO;

namespace pfsim.ActionContainers
{

    public class PfSimCommands
    {
        private readonly IMessageRouter messageRouter;

        public PfSimCommands(IMessageRouter messageRouter)
        {
            this.messageRouter = messageRouter;
        }

        [TypedCommand("load", "Loads an object from a file.")]
        public string LoadNpc(string type, string filename)
        {
            if (!File.Exists(filename))
            {
                return "File not found.";
            }
            var content = File.ReadAllText(filename);

            string message = "Nothing to load.";
            switch (type.ToLower())
            {
                case "character":
                    var character = JsonConvert.DeserializeObject<Character>(content);
                    messageRouter.Publish(new CharacterLoaded { Character = character }, CharacterLoaded.RoutingKey);
                    message = $"Loaded, {character.Name}.";
                    break;
                case "characters":
                    var characters = JsonConvert.DeserializeObject<List<Character>>(content);
                    characters.ForEach(c => messageRouter.Publish(new CharacterLoaded { Character = c }, CharacterLoaded.RoutingKey));
                    message = $"Loaded, {string.Join(", ", characters)}.";
                    break;
                default:
                    message = "Valid object types are, character";
                    break;
            }
            return message;
        }
        
        [TypedCommand("add", "Adds combatants.")]
        public string AddCombatant(string name, int quantity)
        {
            messageRouter.Publish(new AddCombatants { Name = name, Quantity = quantity }, "Combat.Add");
            return "Added.";
        }

        [TypedCommand("rollForInitiative", "")]
        [TypedCommand("rfi", "")]
        public string RollForInitiative()
        {
            messageRouter.Publish(new RollForInitiative(), "Combat.RollForInitiative");
            return "Done.";
        }


        [TypedCommand("next", "Runs the next combat action")]
        public string NextCombatAction()
        {
            var result = messageRouter.RemoteCall<NextCombatAction, ActionResult>(new NextCombatAction());
            return result.Message;
        }

        [TypedCommand("exit", "")]
        public string Exit()
        {
            messageRouter.Publish(new ShutdownCommand(), ShutdownCommand.RoutingKey);
            return "Exiting";
        }
    }
}
