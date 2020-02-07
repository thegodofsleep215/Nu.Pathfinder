using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class EventProcessorBundle
    {
        public Ship Ship { get; set; }

        public List<object> Events { get; set; }
    }

    public static class EventProcessor
    {
        public static VoyageProgress Process(Ship ship, Voyage voyage, List<object> events)
        {
            var voyageProgress = new VoyageProgress
            {
                StartDate = voyage.StartDate,
                ShipName = ship.CrewName
            };
            var openOcean = false;
            var nightStatus = NightStatus.Anchored;
            events.ForEach(evt =>
            {
                switch (evt)
                {
                    case VoyageUpdateEvent vue:
                        voyageProgress.StartDate = vue.StartDate;
                        voyageProgress.ResetProgress();
                        break;
                    case DawnOfANewDayEvent doande:
                        ship.CrewMorale.ClearTemporaryModifiers();
                        openOcean = doande.OpenOcean;
                        nightStatus = doande.NightStatus;
                        AddDaysToVoyage(voyageProgress, ship, 1);
                        break;
                    case EpicCookingFailureEvent ecfe:
                        ship.CrewMorale.AddTemporaryModifier(MoralTypes.Wellbeing, ecfe.WellbeingPenalty);
                        // TODO: Add the penalty for the heal check?
                        break;
                    case UnrulyCrewEvent uce:
                        ship.CrewMorale.AddTemporaryModifier(MoralTypes.Shipshape, -1);
                        break;
                    case SicknessEvent se:
                        ship.DiseasedCrew += se.NumberAffected;
                        voyageProgress.DiseasedCrew = ship.DiseasedCrew;
                        break;
                    case MismanagedSuppliesEvent mse:
                        // TODO: Cargo is not currently being tracked.
                        //if (mse.SupplyType.HasValue)
                        //    ship.ShipsCargo.ConsumeSupply(mse.SupplyType.Value, mse.QuantityLost);
                        // TODO: Apply Penalty?
                        break;
                    case PoorMaintenanceEvent pme:
                        ship.AlterHullDamage(pme.Damage);
                        ship.AlterSailDamage(pme.Damage);
                        break;
                    case PilotFailedEvent pfe:
                        if (pfe.Damage > 0)
                        {
                            if (openOcean)
                                ship.AlterSailDamage(pfe.Damage);
                            else
                                ship.AlterHullDamage(pfe.Damage);
                        }
                        break;
                    case ProgressMadeEvent pme:
                        voyageProgress.ProgressMade += pme.DaysofProgress;
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
            });
            return voyageProgress;
        }

        private static void AddDaysToVoyage(VoyageProgress voyageProgress, Ship ship, int days)
        {
            voyageProgress.AddDaysToVoyage(days);
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
