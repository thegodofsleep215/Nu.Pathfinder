using System.Collections.Generic;
using System.Linq;

namespace pfsim.Officer
{
    public class OfficerEngine
    {
        private readonly IShip ship;
        private readonly DailyInput input;
        private readonly Queue<IDuty> gameQueue;

        public OfficerEngine(IShip ship, DailyInput input)
        {
            this.ship = ship;
            this.input = input;
            gameQueue = new Queue<IDuty>();
            gameQueue.Enqueue(new Command());
            gameQueue.Enqueue(new Manage());
            gameQueue.Enqueue(new Watch());
            gameQueue.Enqueue(new Watch());
            // TODO: By this point, we need to make the minigame.
            // TODO: Alternately, we could just take a ship, and no daily input?
            gameQueue.Enqueue(new Pilot());
            gameQueue.Enqueue(new Navigate());
            gameQueue.Enqueue(new Discipline());
            gameQueue.Enqueue(new Maintain());
            gameQueue.Enqueue(new Cook());
            gameQueue.Enqueue(new Heal());
        }

        public BaseResponse Run()
        {
            var mgs = new MiniGameStatus();
            BaseResponse validation = ship.ValidateAssignedJobs();
            if (validation.Success)
            {
                while (gameQueue.Count > 0)
                {
                    var duty = gameQueue.Dequeue();
                    duty.PerformDuty(ship, input, ref mgs);
                }
                validation.Messages.AddRange(mgs.DutyEvents.Select(x => x.ToString()));
            }

            return validation;
        }
    }
}
