using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class GameEngine
    {
        protected readonly Ship ship;
        protected readonly Queue<IDuty> gameQueue;
        protected readonly bool sailing;
        private readonly bool verbose;

        public GameEngine(Ship ship, bool verbose, bool sailing = true)
        {
            this.ship = ship;
            gameQueue = new Queue<IDuty>();
            this.sailing = sailing;
            this.verbose = verbose;
        }

        public BaseResponse Run()
        {
            var mgs = new MiniGameStatus();
            BaseResponse validation = ship.ValidateAssignedJobs(sailing);
            if (validation.Success)
            {
                while (gameQueue.Count > 0)
                {
                    var duty = gameQueue.Dequeue();
                    duty.PerformDuty(ship, verbose, ref mgs);
                }
            }
            validation.Messages.AddRange(mgs.DutyEvents.Select(x => x.ToString()));
            return validation;
        }

        public Dictionary<string, List<object>> Sail(Ship[] ships)
        {
            return ships.ToDictionary(x => x.CrewName, Sail);
        }

        public List<object> Sail(Ship ship)
        {
            var mgs = new MiniGameStatus();
            BaseResponse validation = ship.ValidateAssignedJobs(sailing);
            if (validation.Success)
            {
                while (gameQueue.Count > 0)
                {
                    var duty = gameQueue.Dequeue();
                    duty.PerformDuty(ship, verbose, ref mgs);
                }
                return null;
            }
            return mgs.DutyEvents;

        }

    }
}