﻿using System.Collections.Generic;
using System.Linq;

namespace pfsim.Officer
{
    public class SailingEngine : GameEngine
    {
        public SailingEngine(Ship ship) : base(ship)
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
