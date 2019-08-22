using System;
using System.Collections.Generic;

namespace pfsim.Officer
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
                        // TODO: Add the penalty for the heal check
                        break;
                    case UnrulyCrewEvent uce:
                        ship.CrewMorale.ShipShape -= 1;
                        break;
                    case SicknessEvent se:
                        // TODO: Make crew sick.
                        break;
                    case MismanagedSuppliesEvent mse:
                        // TODO: Remove supply.
                        // TODO: Apply Penalty
                        break;
                    case PoorMaintenanceEvent pme:
                        // TODO: Add the damage to the ship.
                        break;
                    case OffCourseEvent oce:
                        // TODO: Alter the progress of the voyage.
                        break;
                    case PilotFailedEvent pfe:
                        //TODO: Damage the ship.
                        //TODO: Alter the progress of the voyage.
                        break;
                    case PilotSuccessEvent pse:
                        // TODO: Alter the progress of the voyage.
                        break;
                    default:
                        throw new NotImplementedException(); // This should never happen.
                }
            });
        }
    }

}
