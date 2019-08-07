using Newtonsoft.Json;
using Nu.CommandLine;
using Nu.CommandLine.Attributes;
using Nu.Messaging;
using pfsim.Events;
using System.Collections.Generic;
using System.IO;
using Topshelf;

namespace pfsim
{
    public static class Factory
    {
        private static IMessageRouter messageRouter;

        static Factory()
        {
            messageRouter = new TopicRouter();
        }

        public static IMessageRouter MessageRouter { get => messageRouter; }
    }

    public class ControlService : ServiceControl
    {
        public ControlService(IMessageRouter messageRouter)
        {
            this.messageRouter = messageRouter;
            messageRouter.SubscribeClass(this);
        }

        private HostControl hostControl;
        private readonly IMessageRouter messageRouter;

        public bool Start(HostControl hostControl)
        {
            this.hostControl = hostControl;
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }

        [Subscription(typeof(ShutdownCommand), ShutdownCommand.RoutingKey)]
        public void ShutDown()
        {
            hostControl.Stop();
        }
    }

    public class PfSimCommands : IActionContainer
    {
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
                    Factory.MessageRouter.Publish(new CharacterLoaded { Character = character }, CharacterLoaded.RoutingKey);
                    message = $"Loaded, {character.Name}.";
                    break;
                case "characters":
                    var characters = JsonConvert.DeserializeObject<List<Character>>(content);
                    characters.ForEach(c => Factory.MessageRouter.Publish(new CharacterLoaded { Character = c }, CharacterLoaded.RoutingKey));
                    message = $"Loaded, {string.Join(", ", characters)}.";
                    break;
                default:
                    message = "Valid object types are, character";
                    break;
            }
            return message;
        }

        [TypedCommand("exit", "")]
        public string Exit()
        {
            Factory.MessageRouter.Publish(new ShutdownCommand(), ShutdownCommand.RoutingKey);
            return "Exiting";
        }
    }
}
