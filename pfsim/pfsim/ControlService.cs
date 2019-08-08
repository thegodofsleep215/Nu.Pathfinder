using Nu.CommandLine;
using Nu.CommandLine.Communication;
using Nu.Messaging;
using pfsim.ActionContainers;
using System;
using Topshelf;

namespace pfsim
{
    public class ControlService : ServiceControl
    {

        private HostControl hostControl;
        private CommandProcessor cp;
        private readonly IMessageRouter messageRouter;
        private CombatEngine ce;

        public ControlService()
        {
            this.messageRouter = new TopicRouter();
            messageRouter.SubscribeClass(this);

            cp = CommandProcessor.GenerateCommandProcessor(new InteractiveCommandLineCommunicator("pfsim"));
            cp.RegisterObject(new PfSimCommands(messageRouter));

            ce = new CombatEngine(messageRouter);

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

        [Subscription(typeof(ShutdownCommand), ShutdownCommand.RoutingKey)]
        public void ShutDown(ShutdownCommand sc)
        {
            hostControl.Stop();
            Environment.Exit(0);
        }
    }
}
