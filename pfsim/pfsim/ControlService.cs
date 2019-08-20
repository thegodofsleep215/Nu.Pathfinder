using Newtonsoft.Json;
using Nu.CommandLine;
using Nu.CommandLine.Communication;
using Nu.Messaging;
using pfsim.ActionContainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Topshelf;

namespace pfsim
{
    public class ControlService : ServiceControl
    {

        private HostControl hostControl;
        private CommandProcessor cp;
        private readonly IMessageRouter messageRouter;
        private CombatManager ce;

        public ControlService()
        {
            this.messageRouter = new TopicRouter();
            messageRouter.SubscribeClass(this);

            cp = CommandProcessor.GenerateCommandProcessor(new InteractiveCommandLineCommunicator("pfsim"));
            cp.RegisterObject(new PfSimCommands(messageRouter));
            cp.RegisterObject(new OfficerMiniGameCommands());

            ce = new CombatManager(messageRouter, LoadAssets());

        }
        public bool Start(HostControl hostControl)
        {
            this.hostControl = hostControl;
            cp.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            cp.Stop();
            return true;
        }

        [Subscription(ShutdownCommand.RoutingKey)]
        public void ShutDown(ShutdownCommand sc)
        {
            hostControl.Stop();
            Environment.Exit(0);
        }

        private List<Character> LoadAssets()
        {
            var folder = ".\\Assets";
            if (!Directory.Exists(folder))
            {
                return new List<Character>();
            }
            var charFiles = Directory.GetFiles(folder, "*.json");
            return charFiles.Select(cf => JsonConvert.DeserializeObject<Character>(File.ReadAllText(cf))).ToList();
        }
    }
}
