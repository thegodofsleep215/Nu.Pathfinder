using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Weather;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{

    public static class MultiShipGameEngine
    {
        private static Random rand = new Random();


        public static List<object> Sail(List<Ship> ships, ref FleetState currentState, SailingParameters parameters, IWeatherEngine weatherEngine)
        {
            var events = new List<object>();
            var weather = weatherEngine.GetWeatherConditions(new WeatherInput(currentState.WeatherConditions, 0, currentState.CurrentDate, Region.Tropical));
            var dawn = DawnOfANewDayEvent.FromInput(parameters, weather);
            EventProcessor.Update(ref currentState, dawn);
            events.Add(dawn);

            var queue = CreateSailingQueue(currentState.NightStatus);
            while (queue.Count > 0)
            {
                var duty = queue.Dequeue();
                var tempState = currentState;
                var temp = ships.SelectMany(ship => duty.PerformDuty(ship, tempState)).ToList();
                foreach (var e in temp)
                {
                    EventProcessor.Update(ref currentState, e);
                }
                events.AddRange(temp);
            }
            var pme = GenerateProgressMadeEvent(events, currentState.NightStatus);
            EventProcessor.Update(ref currentState, pme);
            events.Add(pme);
            return events;
        }

        private static ProgressMadeEvent GenerateProgressMadeEvent(List<object> allShipsEventsForADay, NightStatus nightStatus)
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

        private static Queue<IDuty> CreateSailingQueue(NightStatus nightStatus)
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

        private static Queue<IDuty> CreateAnchoredQueue()
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
