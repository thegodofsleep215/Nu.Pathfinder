using System;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    public class EventProcessor
    {
        public void Process(ref Ship currentShip, List<object> events)
        {
            var ship = currentShip;
            events.ForEach(evt =>
            {
                switch (evt)
                {
                    case DailyEvent de:
                        ProcessDailyEvent(ref ship, de);
                        break;
                    default:
                        throw new NotImplementedException(); // This should never happen.
                }
            });
        }

        private void ProcessDailyEvent(ref Ship currentShip, DailyEvent de)
        {
            var ship = currentShip;
            ship.CrewMorale.ClearTemporaryModifiers();

            de.DutyResults.ForEach(evt => 
            {
                switch (evt)
                {
                    case EpicCookingFailureEvent ecfe:
                        ship.CrewMorale.AddTemporaryModifier(MoralTypes.Wellbeing, ecfe.WellbeingPenalty);
                        // TODO: Add the penalty for the heal check?
                        break;
                    case UnrulyCrewEvent uce:
                        ship.CrewMorale.ShipShape -= 1;
                        break;
                    case SicknessEvent se:
                        ship.CurrentVoyage.DiseasedCrew += se.NumberAffected;
                        break;
                    case MismanagedSuppliesEvent mse:
                        if(mse.SupplyType.HasValue)
                            ship.ShipsCargo.ConsumeSupply(mse.SupplyType.Value, mse.QuantityLost);
                        // TODO: Apply Penalty?
                        break;
                    case PoorMaintenanceEvent pme:
                        ship.CurrentVoyage.AlterHullDamage(pme.Damage);
                        ship.CurrentVoyage.AlterSailDamage(pme.Damage);
                        break;
                    case OffCourseEvent oce:
                        // TODO: Alter the progress of the voyage.
                        // TODO: How do you want to track progress?  Do we need to calculate ship speed?
                        break;
                    case PilotFailedEvent pfe:
                        //TODO: Damage the ship.
                        if(pfe.Damage > 0)
                        {
                            if (ship.CurrentVoyage.OpenOcean)
                                ship.CurrentVoyage.AlterSailDamage(pfe.Damage);
                            else
                                ship.CurrentVoyage.AlterHullDamage(pfe.Damage);
                        }
                        //TODO: Alter the progress of the voyage.
                        //TODO: How do you want to track progress?  Do we need to calculate ship speed?
                        break;
                    case PilotSuccessEvent pse:
                        // TODO: Alter the progress of the voyage.
                        // TODO: How do you want to track progress?  Do we need to calculate ship speed?
                        break;
                    case SeaShantyEvent sse:
                    case SuppliesExhaustedEvent see:
                    case PerformedDutyEvent pde:
                        // General information events.
                        break;
                    default:
                        throw new NotImplementedException(); // This should never happen.
                }
            });

            ship.AddDaysToVoyage(1);
        }
    }

}
