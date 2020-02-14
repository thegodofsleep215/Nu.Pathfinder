using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public static class EventProcessor
    {
        public static void Update(ref FleetState fleetState, Dictionary<string, Ship> ships, object evt)
        {
            switch (evt)
            {
                case AddShipLoadoutToVoyageEvent asltve:
                    fleetState.ShipStates[asltve.InitialState.LoadoutName] = asltve.InitialState;
                    break;
                case RemoveShipLoadoutFromVoyageEvent rslfve:
                    fleetState.ShipStates.Remove(rslfve.ShipLoadoutName);
                    break;
                case SetCourseEvent sc:
                    fleetState.ResetProgress();
                    fleetState.StartDate = sc.StartDate;
                    fleetState.DaysPlanned = sc.DaysPlanned;
                    fleetState.ShipStates = sc.InitialShipStates.ToDictionary(x => x.LoadoutName, x => x);
                    break;
                case DawnOfANewDayEvent doande:
                    fleetState.OpenOcean = doande.OpenOcean;
                    fleetState.WeatherConditions = doande.WeatherConditions;
                    fleetState.ShipStates = doande.CurrentShipStates.ToDictionary(x => x.LoadoutName, x => x);
                    AddDayToVoyage(fleetState, ships);
                    break;
                case WeatherConditions wc:
                    break;

                case PerformedDutyEvent pde:
                    switch (pde.Duty)
                    {
                        case DutyType.Command:
                            fleetState.ShipStates[pde.ShipName].CommandResult = pde.Result;
                            break;
                        case DutyType.Cook:
                            fleetState.ShipStates[pde.ShipName].CookResult = pde.Result;
                            break;
                        case DutyType.Maintain:
                            fleetState.ShipStates[pde.ShipName].MaintainResult = pde.Result;
                            break;
                        case DutyType.Manage:
                            fleetState.ShipStates[pde.ShipName].ManageResult = pde.Result;
                            break;
                        case DutyType.Navigate:
                            fleetState.ShipStates[pde.ShipName].NavigateResult = pde.Result;
                            break;
                        case DutyType.Pilot:
                            fleetState.ShipStates[pde.ShipName].PilotResult = pde.Result;
                            break;
                    }
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
                    fleetState.ShipStates[se.ShipName].DiseasedCrew = ships[se.ShipName].DiseasedCrew;
                    break;
                case ProgressMadeEvent pme:
                    var cd = fleetState.CurrentDate.ToSemanticString();
                    if (fleetState.ProgressForEachDay.ContainsKey(cd))
                    {
                        var old = fleetState.ProgressForEachDay[cd];
                        if (pme.DaysofProgress < old)
                        {
                            fleetState.ProgressMade -= old;
                            fleetState.ProgressMade += pme.DaysofProgress;
                            fleetState.ProgressForEachDay[cd] = pme.DaysofProgress;
                        }
                    }
                    else
                    {
                        fleetState.ProgressMade += pme.DaysofProgress;
                        fleetState.ProgressForEachDay[cd] = pme.DaysofProgress;
                    }
                    break;
                case PilotFailedEvent _:
                case MismanagedSuppliesEvent _:
                case PoorMaintenanceEvent _:
                case PilotSuccessEvent _:
                case OffCourseEvent _:
                case SeaShantyEvent _:
                case SuppliesExhaustedEvent _:
                case WatchResultEvent _:
                    // General information events. Avoiding the default case.
                    break;

                default:
                    throw new NotImplementedException(); // This should never happen.
            }

        }

        public static FleetState Process(Dictionary<string, Ship> ships, List<object> events)
        {
            var fleetProgress = new FleetState()
            ;
            events.ForEach(evt =>
            {
                Update(ref fleetProgress, ships, evt);
            });
            return fleetProgress;
        }

        private static void AddDayToVoyage(FleetState fleetProgress, Dictionary<string, Ship> ships)
        {
            var days = 1;
            fleetProgress.AddDaysToVoyage(days);
            ships.Values.ToList().ForEach(x => AddDaysToVoyage(x));
            void AddDaysToVoyage(Ship ship)
            {
                ship.CrewMorale.ClearTemporaryModifiers();
                //var healCount = ship.AssignedJobs.Count(a => a.DutyType == DutyType.Heal && !a.IsAssistant);
                //var repairCount = ship.AssignedJobs.Count(a => (a.DutyType == DutyType.Maintain && !a.IsAssistant)
                //                || (a.DutyType == DutyType.RepairHull && !a.IsAssistant)
                //                || (a.DutyType == DutyType.RepairSails && !a.IsAssistant)
                //                || (a.DutyType == DutyType.RepairSeigeEngine && !a.IsAssistant));

                //ship.ShipsCargo.ConsumeSupplies(ship.TotalCrew, days);
                //if (healCount > 0) // Note, various medical supplies can be consumed by other than treating disease.  It's best to adjust this separately.
                //    ship.ShipsCargo.ConsumeSupply(SupplyType.Medicine, (ship.DiseasedCrew + 1) * days * -1);
                //if (repairCount > 0)
                //    ship.ShipsCargo.ConsumeSupply(SupplyType.ShipSupplies, repairCount * days * -1); // TODO: This should scale with ship size.
                //ship.ShipsCargo.ConsumeFodder(ship.ShipsCargo.AnimalUnitsAboard * days);
                //ship.ShipsCargo.ResetPassengers(ship.TotalCrew);
                //ship.ShipsCargo.AgeCargo(days);
            }
        }


    }

}
