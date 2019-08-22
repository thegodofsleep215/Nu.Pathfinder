using System.Collections.Generic;
using System.Linq;

namespace pfsim.Officer
{
    public class OfficerEngine
    {
        private readonly Ship ship;
        private readonly DailyInput input;
        private readonly Queue<IDuty> gameQueue;

        public OfficerEngine(Ship ship, DailyInput input)
        {
            this.ship = ship;
            this.input = input;
            gameQueue = new Queue<IDuty>();
            gameQueue.Enqueue(new Command());
            gameQueue.Enqueue(new Manage());
            gameQueue.Enqueue(new Watch());
            gameQueue.Enqueue(new Pilot());
            gameQueue.Enqueue(new Navigate());
            gameQueue.Enqueue(new Discipline());
            gameQueue.Enqueue(new Maintain());
            gameQueue.Enqueue(new Cook());
            gameQueue.Enqueue(new Heal());
        }

        public List<string> Run()
        {
            var mgs = new MiniGameStatus();
            while(gameQueue.Count > 0)
            {
                var duty = gameQueue.Dequeue();
                duty.PerformDuty(ship, input, ref mgs);
            }
            return mgs.DutyEvents.Select(x => x.ToString()).ToList(); // This won't do anything yet
        }
    }
}
