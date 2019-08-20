using System.Collections.Generic;

namespace pfsim.Officer
{
    public class OfficerEngine
    {
        private readonly Crew crew;
        private MiniGameStatus mgs;
        private readonly Queue<IDuty> gameQueue;

        public OfficerEngine(Crew crew, MiniGameStatus mgs)
        {
            this.crew = crew;
            this.mgs = mgs;
            gameQueue = new Queue<IDuty>();
            gameQueue.Enqueue(new Command());
            gameQueue.Enqueue(new Manage());
            gameQueue.Enqueue(new Watch());
            gameQueue.Enqueue(new Pilot());
            gameQueue.Enqueue(new Navigate());
            gameQueue.Enqueue(new Discipline());
            gameQueue.Enqueue(new Maintain());
        }

        public List<string> Run()
        {
            while(gameQueue.Count > 0)
            {
                var duty = gameQueue.Dequeue();
                duty.PerformDuty(crew, ref mgs);
            }
            return mgs.ActionResults;
        }
    }
}
