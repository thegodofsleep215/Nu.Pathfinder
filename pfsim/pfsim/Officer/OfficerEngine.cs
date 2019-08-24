using System.Collections.Generic;

namespace pfsim.Officer
{
    public class OfficerEngine
    {
        private readonly IShip crew;
        private readonly DailyInput input;
        private readonly Queue<IDuty> gameQueue;

        public OfficerEngine(IShip crew, DailyInput input)
        {
            this.crew = crew;
            this.input = input;
            gameQueue = new Queue<IDuty>();
            gameQueue.Enqueue(new Command());
            gameQueue.Enqueue(new Manage());
            gameQueue.Enqueue(new Watch());
            gameQueue.Enqueue(new Watch());
            // TODO: By this point, we need to make the minigame.
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
            BaseResponse validation = crew.ValidateAssignedJobs();
            if (validation.Success)
            {
                while (gameQueue.Count > 0)
                {
                    var duty = gameQueue.Dequeue();
                    duty.PerformDuty(crew, input, ref mgs);
                }
                return mgs.ActionResults;
            }
            else
            {
                return validation.Messages;
            }
        }
    }
}
