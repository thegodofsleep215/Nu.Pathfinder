using Nu.OfficerMiniGame.Dal.Enums;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public static class ShipValidation
    {
        private static int MaxPilotAssistants(ShipSize shipSize)
        {
            switch (shipSize)
            {
                case ShipSize.Medium:
                    return 1;
                case ShipSize.Large:
                case ShipSize.Huge:
                    return 3;
                case ShipSize.Gargantuan:
                case ShipSize.Colossal:
                    return 6;
                default:
                    return 6;
            }
        }

        private static int MaxCookAssistants()
        {
                return 1;
        }

        private static int MaxClerks()
        {
                return 2;
        }
        
        public static BaseResponse ValidateShip(Ship ship)
        {
            BaseResponse retval = new BaseResponse();

            if (ship.ShipsCrew.Count != ship.ShipsCrew.Select(a => a.Name).Distinct().Count())
            {
                retval.Messages.Add("Everyone in the crew must have a different name!");
            }
            // This is a bit of a simplification, as I can see situations where you could have different numbers of these, but for now
            // this is complex enough.
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Command && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two commanders!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Command && !a.IsAssistant) > 2)
            {
                retval.Messages.Add("Too many leutinants!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Manage && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two pursurs!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Watch && !a.IsAssistant) > 3)
            {
                retval.Messages.Add("Too many helmsmen.  There are only three watches per day!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Manage && a.IsAssistant) > MaxClerks())
            {
                retval.Messages.Add("Too many clerks!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Pilot && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two masters!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Pilot && a.IsAssistant) > MaxPilotAssistants(ship.ShipSize))
            {
                retval.Messages.Add("Too many hands for the ship's wheel!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Navigate && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many navigators!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Navigate && a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many quartermasters!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Cook && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many cooks spoil the pot!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Cook && a.IsAssistant) > MaxCookAssistants())
            {
                retval.Messages.Add("Too many cook's mates!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Discipline && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two discipline officers!");
            }
            if (ship.AssignedJobs.Count(a => a.DutyType == DutyType.Discipline && a.IsAssistant) > 0)
            {
                retval.Messages.Add("Can't have an assistant discipline officer!");
            }

            foreach (var matey in ship.ShipsCrew)
            {
                retval.Messages.AddRange(matey.ValidateJobs());
            }

            if (retval.Messages.Count == 0)
                retval.Success = true;

            return retval;
        }



    }
}
