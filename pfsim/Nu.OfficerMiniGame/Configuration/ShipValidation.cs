using Nu.OfficerMiniGame.Dal.Enums;
using System.Collections.Generic;
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
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Command && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two commanders!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Command && !a.IsAssistant) > 2)
            {
                retval.Messages.Add("Too many leutinants!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Manage && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two pursurs!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Watch && !a.IsAssistant) > 3)
            {
                retval.Messages.Add("Too many helmsmen.  There are only three watches per day!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Manage && a.IsAssistant) > MaxClerks())
            {
                retval.Messages.Add("Too many clerks!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Pilot && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two masters!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Pilot && a.IsAssistant) > MaxPilotAssistants(ship.ShipSize))
            {
                retval.Messages.Add("Too many hands for the ship's wheel!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Navigate && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many navigators!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Navigate && a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many quartermasters!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Cook && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many cooks spoil the pot!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Cook && a.IsAssistant) > MaxCookAssistants())
            {
                retval.Messages.Add("Too many cook's mates!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Discipline && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two discipline officers!");
            }
            if (ship.ShipsCrew.CountJobs(a => a.DutyType == DutyType.Discipline && a.IsAssistant) > 0)
            {
                retval.Messages.Add("Can't have an assistant discipline officer!");
            }

            retval.Messages.AddRange(ship.ShipsCrew.SelectMany(x => ValidateJobs(x)));

            if (retval.Messages.Count == 0)
                retval.Success = true;

            return retval;
        }

        private static IEnumerable<string> ValidateJobs(CrewMember crewMember)
        {
            List<string> messages = new List<string>();

            if (crewMember.Jobs.Count(a => a.DutyType == DutyType.RepairHull || a.DutyType == DutyType.RepairSails || a.DutyType == DutyType.RepairSeigeEngine) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to repair everything!", crewMember.Title, crewMember.Name));
            }
            if (crewMember.Jobs.Count(a => a.DutyType == DutyType.Stow) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to put it all away!", crewMember.Title, crewMember.Name));
            }
            if (crewMember.Jobs.Count(a => a.DutyType == DutyType.Heal) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to heal everyone!", crewMember.Title, crewMember.Name));
            }
            if (crewMember.Jobs.Count(a => a.DutyType == DutyType.Cook) > 1)
            {
                messages.Add(string.Format("{0} {1} forgot that cooking twice is just cooking once, in smaller batches!", crewMember.Title, crewMember.Name));
            }
            if (crewMember.Jobs.Count(a => a.DutyType == DutyType.Ministrel) > 2)
            {
                messages.Add(string.Format("{0} {1}'s voice would get tired!", crewMember.Title, crewMember.Name));
            }
            if (crewMember.Jobs.Count(a => a.DutyType == DutyType.Procure) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to catch all the fish!", crewMember.Title, crewMember.Name));
            }
            if (crewMember.Jobs.Count(a => a.DutyType == DutyType.Procure ||
                                a.DutyType == DutyType.Ministrel ||
                                a.DutyType == DutyType.Heal ||
                                a.DutyType == DutyType.Stow ||
                                a.DutyType == DutyType.Unload ||
                                a.DutyType == DutyType.RepairHull ||
                                a.DutyType == DutyType.RepairSails ||
                                a.DutyType == DutyType.RepairSeigeEngine) > 2)
            {
                messages.Add(string.Format("{0} {1} can't do all the work by himself!", crewMember.Title, crewMember.Name));
            }

            return messages;
        }

    }
}
