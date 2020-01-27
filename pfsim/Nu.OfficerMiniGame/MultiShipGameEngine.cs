using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{

    public class MultiShipGameEngine
    {
        private static Random rand = new Random();
        protected readonly bool sailing;
        private readonly bool verbose;

        public MultiShipGameEngine(bool verbose, bool sailing = true)
        {
            this.verbose = verbose;
            this.sailing = sailing;
        }

        public Dictionary<string, List<object>> Sail(Ship[] ships, SailingParameters sailingParameters, WeatherConditions weatherConditions)
        {
            var results = ships.ToDictionary(x => x.CrewName, x => Sail(x, sailingParameters, weatherConditions));
            var pe = GenerateProgressMadeEvent(results.SelectMany(x => x.Value).ToList(), sailingParameters.NightStatus);
            results.ToList().ForEach(x => x.Value.Add(pe));
            return results;
        }

        private List<object> Sail(Ship ship, SailingParameters sailingParameters, WeatherConditions weatherConditions)
        {
            var gameQueue = CreateSailingQueue(sailingParameters.NightStatus);
            var mgs = new MiniGameStatus(weatherConditions, sailingParameters.OpenOcean, sailingParameters.NightStatus);
            BaseResponse validation = ShipValidation.ValidateShip(ship);
            if (validation.Success)
            {
                while (gameQueue.Count > 0)
                {
                    var duty = gameQueue.Dequeue();
                    duty.PerformDuty(ship, verbose, ref mgs);
                }
            }

            return mgs.GameEvents;

        }

        public static ProgressMadeEvent GenerateProgressMadeEvent(List<object> allShipsEventsForADay, NightStatus nightStatus)
        {
            var pe = new ProgressMadeEvent();
            var progressEvents = allShipsEventsForADay.Where(y => y is PilotFailedEvent || y is OffCourseEvent || y is PilotSuccessEvent).ToList();
            if (progressEvents.Any(x => x is OffCourseEvent))
            {
                pe.DaysofProgress = OffCourse(nightStatus);
            }
            else if (progressEvents.Any(x => x is PilotFailedEvent))
            {
                pe.DaysofProgress = SlowProgress(nightStatus);
            }
            else
            {
                pe.DaysofProgress = RegularProgress(nightStatus);
            }
            return pe;
        }
        private static double SlowProgress(NightStatus nightStatus)
        {
            return AdjustStatusForNightStatus(nightStatus, .5);
        }

        private static double OffCourse(NightStatus nightStatus)
        {
            return AdjustStatusForNightStatus(nightStatus, -.5);
        }

        private static double RegularProgress(NightStatus nightStatus)
        {
            return AdjustStatusForNightStatus(nightStatus, 1);
        }

        private static double AdjustStatusForNightStatus(NightStatus nightStatus, double progress)
        {
            var result = progress;
            if (nightStatus == NightStatus.Underweigh)
            {
                switch (progress)
                {
                    case .5:
                        result += .25;
                        break;
                    case 1:
                        result += .5;
                        break;
                    case -.5:
                        result += -.5;
                        break;
                }
            }
            else if (nightStatus == NightStatus.Drifting)
            {
                // Probably overly simplified and too great of a magnitude.
                switch (rand.Next(0, 3))
                {
                    case 0:
                        result += -.25;
                        break;
                    case 1:
                        result += 0;
                        break;
                    case 2:
                        result += .25;
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }
            return result;
        }


        private Queue<IDuty> CreateSailingQueue(NightStatus nightStatus)
        {
            var gameQueue = new Queue<IDuty>();
            gameQueue.Enqueue(new Command());

            gameQueue.Enqueue(new Manage());
            gameQueue.Enqueue(new Watch(WatchShift.First));
            gameQueue.Enqueue(new Watch(WatchShift.Second));
            if (nightStatus == NightStatus.Underweigh)
                gameQueue.Enqueue(new Watch(WatchShift.Third));
            gameQueue.Enqueue(new Pilot());
            gameQueue.Enqueue(new Navigate());
            gameQueue.Enqueue(new Discipline());
            gameQueue.Enqueue(new Maintain());
            gameQueue.Enqueue(new Cook());
            gameQueue.Enqueue(new Heal());
            return gameQueue;
        }

        private Queue<IDuty> CreateAnchoredQueue()
        {
            var gameQueue = new Queue<IDuty>();
            gameQueue.Enqueue(new Command());
            gameQueue.Enqueue(new Manage());
            gameQueue.Enqueue(new Watch(WatchShift.First));
            gameQueue.Enqueue(new Watch(WatchShift.Second));
            gameQueue.Enqueue(new Watch(WatchShift.Third));
            gameQueue.Enqueue(new Discipline());
            gameQueue.Enqueue(new Maintain());
            gameQueue.Enqueue(new Cook());
            gameQueue.Enqueue(new Heal());
            return gameQueue;
        }
    }
}
