using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public static class EventProcessor
    {
        public static void Update(ref FleetVoyageProgress fleetProgress, Dictionary<string, Ship> ships, object evt)
        {
            switch (evt)
            {
                case AddShipLoadoutToVoyageEvent asltve:
                    fleetProgress.ShipStates[asltve.InitialState.LoadoutName] = asltve.InitialState;
                    break;
                case RemoveShipLoadoutFromVoyageEvent rslfve:
                    fleetProgress.ShipStates.Remove(rslfve.ShipLoadoutName);
                    break;
                case SetCourseEvent sc:
                    fleetProgress.ResetProgress();
                    fleetProgress.StartDate = sc.StartDate;
                    fleetProgress.DaysPlanned = sc.DaysPlanned;
                    fleetProgress.ShipStates = sc.InitialShipStates.ToDictionary(x => x.LoadoutName, x => x);
                    break;
                case DawnOfANewDayEvent doande:
                    fleetProgress.OpenOcean = doande.OpenOcean;
                    fleetProgress.WeatherConditions = doande.WeatherConditions;
                    fleetProgress.ShipStates = doande.CurrentShipStates.ToDictionary(x => x.LoadoutName, x => x);
                    AddDayToVoyage(fleetProgress, ships);
                    break;
                case WeatherConditions wc:
                    break;


                case EpicCookingFailureEvent ecfe:
                    ships[ecfe.ShipName].CrewMorale.AddTemporaryModifier(MoralTypes.Wellbeing, ecfe.WellbeingPenalty);
                    // TODO: Add the penalty for the heal check?
                    break;
                case UnrulyCrewEvent uce:
                    ships[uce.ShipName].CrewMorale.AddTemporaryModifier(MoralTypes.Shipshape, -1);
                    break;
                case SicknessEvent se:
                    ships[se.ShipName].DiseasedCrew += se.NumberAffected;
                    fleetProgress.ShipStates[se.ShipName].DiseasedCrew = ships[se.ShipName].DiseasedCrew;
                    break;
                case MismanagedSuppliesEvent mse:
                    // TODO: Cargo is not currently being tracked.
                    //if (mse.SupplyType.HasValue)
                    //    ship.ShipsCargo.ConsumeSupply(mse.SupplyType.Value, mse.QuantityLost);
                    // TODO: Apply Penalty?
                    break;
                case PoorMaintenanceEvent pme:
                    ships[pme.ShipName].AlterHullDamage(pme.Damage);
                    ships[pme.ShipName].AlterSailDamage(pme.Damage);
                    break;
                case PilotFailedEvent pfe:
                    if (pfe.Damage > 0)
                    {
                        if (fleetProgress.OpenOcean)
                            ships[pfe.ShipName].AlterSailDamage(pfe.Damage);
                        else
                            ships[pfe.ShipName].AlterHullDamage(pfe.Damage);
                    }
                    break;
                case ProgressMadeEvent pme:
                    var cd = fleetProgress.CurrentDate.ToSemanticString();
                    if (fleetProgress.ProgressForEachDay.ContainsKey(cd))
                    {
                        var old = fleetProgress.ProgressForEachDay[cd];
                        if (pme.DaysofProgress < old)
                        {
                            fleetProgress.ProgressMade -= old;
                            fleetProgress.ProgressMade += pme.DaysofProgress;
                            fleetProgress.ProgressForEachDay[cd] = pme.DaysofProgress;
                        }
                    }
                    else
                    {
                        fleetProgress.ProgressMade += pme.DaysofProgress;
                        fleetProgress.ProgressForEachDay[cd] = pme.DaysofProgress;
                    }
                    break;
                case PilotSuccessEvent _:
                case OffCourseEvent _:
                case SeaShantyEvent _:
                case SuppliesExhaustedEvent _:
                case PerformedDutyEvent _:
                case WatchResultEvent _:
                    // General information events. Avoiding the default case.
                    break;

                default:
                    throw new NotImplementedException(); // This should never happen.
            }

        }

        public static FleetVoyageProgress Process(Dictionary<string, Ship> ships, List<object> events)
        {
            var fleetProgress = new FleetVoyageProgress()
            ;
            events.ForEach(evt =>
            {
                Update(ref fleetProgress, ships, evt);
            });
            return fleetProgress;
        }

        private static void AddDayToVoyage(FleetVoyageProgress fleetProgress, Dictionary<string, Ship> ships)
        {
            var days = 1;
            fleetProgress.AddDaysToVoyage(days);
            ships.Values.ToList().ForEach(x => AddDaysToVoyage(x));
            void AddDaysToVoyage(Ship ship)
            {
                ship.CrewMorale.ClearTemporaryModifiers();
                var healCount = ship.AssignedJobs.Count(a => a.DutyType == DutyType.Heal && !a.IsAssistant);
                var repairCount = ship.AssignedJobs.Count(a => (a.DutyType == DutyType.Maintain && !a.IsAssistant)
                                || (a.DutyType == DutyType.RepairHull && !a.IsAssistant)
                                || (a.DutyType == DutyType.RepairSails && !a.IsAssistant)
                                || (a.DutyType == DutyType.RepairSeigeEngine && !a.IsAssistant));

                ship.ShipsCargo.ConsumeSupplies(ship.TotalCrew, days);
                if (healCount > 0) // Note, various medical supplies can be consumed by other than treating disease.  It's best to adjust this separately.
                    ship.ShipsCargo.ConsumeSupply(SupplyType.Medicine, (ship.DiseasedCrew + 1) * days * -1);
                if (repairCount > 0)
                    ship.ShipsCargo.ConsumeSupply(SupplyType.ShipSupplies, repairCount * days * -1); // TODO: This should scale with ship size.
                ship.ShipsCargo.ConsumeFodder(ship.ShipsCargo.AnimalUnitsAboard * days);
                ship.ShipsCargo.ResetPassengers(ship.TotalCrew);
                ship.ShipsCargo.AgeCargo(days);
            }
        }


    }

}
