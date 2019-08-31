using System.Collections.Generic;
using System.Linq;

namespace pfsim.Officer
{
    public class GameEngine
    {
        protected readonly Ship ship;
        protected readonly Queue<IDuty> gameQueue;

        public GameEngine(Ship ship)
        {
            this.ship = ship;
            gameQueue = new Queue<IDuty>();
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
                    duty.PerformDuty(ship, ref mgs);
                }
                validation.Messages.AddRange(mgs.DutyEvents.Select(x => x.ToString()));
            }

            return validation;
        }
    }
}