using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class SailingEngine : GameEngine
    {
        public SailingEngine(Ship ship, bool verbose) : base(ship, verbose)
        {
            SetUpQueue();
        }

        public SailingEngine() : base(null, true)
        {
            SetUpQueue();
        }

        private void SetUpQueue()
        {
            gameQueue.Enqueue(new Command());
            gameQueue.Enqueue(new Manage());
            gameQueue.Enqueue(new Watch());
            gameQueue.Enqueue(new Watch());
            if (ship.CurrentVoyage.NightStatus == NightStatus.Underweigh)
                gameQueue.Enqueue(new Watch());
            gameQueue.Enqueue(new Pilot());
            gameQueue.Enqueue(new Navigate());
            gameQueue.Enqueue(new Discipline());
            gameQueue.Enqueue(new Maintain());
            gameQueue.Enqueue(new Cook());
            gameQueue.Enqueue(new Heal());

        }
    }
}
