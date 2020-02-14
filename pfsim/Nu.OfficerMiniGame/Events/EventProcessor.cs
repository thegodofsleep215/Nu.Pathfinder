using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public static class EventProcessor
    {
        public static void Update(ref FleetState fleetState, object evt)
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
                    fleetState.CurrentDate = sc.StartDate;
                    fleetState.DaysPlanned = sc.DaysPlanned;
                    if (sc.InitialShipStates != null)
                    {
                        fleetState.ShipStates = sc.InitialShipStates.ToDictionary(x => x.LoadoutName, x => x);
                    }
                    break;
                case DawnOfANewDayEvent doande:
                    fleetState.NightStatus = doande.NightStatus;
                    fleetState.OpenOcean = doande.OpenOcean;
                    fleetState.NarrowPassage = doande.NarrowPassage;
                    fleetState.ShallowWater = doande.ShallowWater;
                    fleetState.WeatherConditions = doande.WeatherConditions;
                    fleetState.ShipStates = doande.CurrentShipStates.ToDictionary(x => x.LoadoutName, x => new ShipState(x));
                    fleetState.AddDaysToVoyage(1);

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
                    fleetState.ShipStates[ecfe.ShipName].CrewMorale.AddTemporaryModifier(MoralTypes.Wellbeing, ecfe.WellbeingPenalty);
                    // TODO: Add the penalty for the heal check?
                    break;
                case UnrulyCrewEvent uce:
                    fleetState.ShipStates[uce.ShipName].CrewMorale.AddTemporaryModifier(MoralTypes.Shipshape, -1);
                    break;
                case SicknessEvent se:
                    fleetState.ShipStates[se.ShipName].DiseasedCrew += se.NumberAffected;
                    break;
                case ProgressMadeEvent pme:
                    var cd = fleetState.CurrentDate.ToSemanticString();
                    fleetState.ProgressMade += pme.DaysofProgress;
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
            if(evt is IShipReportEvent sre)
            {
                fleetState.ShipStates[sre.ShipName].ShipReportEvents.Add(evt);
            }
        }

        public static FleetState Process(List<object> events)
        {
            var fleetProgress = new FleetState()
            ;
            events.ForEach(evt =>
            {
                Update(ref fleetProgress, evt);
            });
            return fleetProgress;
        }
    }
}
