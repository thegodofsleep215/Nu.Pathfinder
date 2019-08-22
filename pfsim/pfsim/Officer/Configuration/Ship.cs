using System;
using System.Collections.Generic;
using System.Linq;

namespace pfsim.Officer
{
    [Serializable]
    public class Ship
    {
        private List<Job> _assignedJobs;

        public string Name { get; set; }

        public ShipType ShipType { get; set; }

        public ShipSize ShipSize { get; set; }

        public Morale CrewMorale { get; set; } = new Morale();

        public List<Propulsion> PropulsionTypes { get; set; } = new List<Propulsion>();

        public int HullHitPoints { get; set; }

        public int CrewSize { get; set; }

        public int MinimumCrewSize
        {
            get
            {
                return CrewSize / 2;
            }
        }

        public int ShipDc { get; set; }

        public int ShipPilotingBonus { get; set; }

        public int ShipQuality { get; set; }

        public int CrewQuality
        {
            get
            {
                var retval = ShipsCrew.Where(a => a.CountsAsCrew).Select(a => a.ProfessionSailorSkill).Average();

                retval = Math.Floor(retval + Convert.ToDouble(AverageSwabbieQuality)) - 4;

                if (retval > 4)
                    return 4;
                else if (retval < -4)
                    return -4;
                else
                    return (int)retval;
            }
        }

        public bool HasDisciplineOfficer
        {
            get
            {
                return AssignedJobs.Exists(a => a.DutyType == DutyType.Discipline);
            }
        }

        public bool HasShipsDoctor
        {
            get
            {
                return AssignedJobs.Exists(a => a.DutyType == DutyType.Heal);
            }
        }

        public bool HasMinimumCrew
        {
            get
            {
                return AvailableCrew - MinimumCrewSize > 0;
            }
        }

        public List<CrewMember> ShipsCrew { get; set; } = new List<CrewMember>();

        public int Marines { get; set; }

        public int Passengers { get; set; }

        public int Swabbies { get; set; }

        public decimal AverageSwabbieQuality { get; set; }

        public int TotalCrew
        {
            get
            {
                return ShipsCrew.Count + Swabbies + Marines + Passengers;
            }
        }

        public int AvailableCrew
        {
            get
            {
                return ShipsCrew.Count(a => a.CountsAsCrew) + Swabbies;
            }
        }

        public int SkeletonCrewPenalty
        {
            get
            {
                int retval = AvailableCrew - CrewSize;

                if (retval > 0)
                    return 0;
                else
                    return retval < -10 ? -10 : retval;
            }
        }

        public List<Job> AssignedJobs
        {
            get
            {
                if (_assignedJobs == null)
                    _assignedJobs = CollectAssignedJobs();

                return _assignedJobs;
            }
        }

        private List<Job> CollectAssignedJobs()
        {
            _assignedJobs = new List<Job>();

            foreach (var matey in ShipsCrew)
            {
                _assignedJobs.AddRange(matey.Jobs);
            }

            return _assignedJobs;
        }

        public BaseResponse ValidateAssignedJobs()
        {
            BaseResponse retval = new BaseResponse();

            // This is a bit of a simplification, as I can see situations where you could have more than 1 of these, but for now
            // this is complex enough.
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Command && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two commanders!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Manage && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two managers!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Pilot && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two pilots!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Navigate && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two navigators!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Cook && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many cooks spoil the pot!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Discipline && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two discipline officers!");
            }

            foreach (var matey in ShipsCrew)
            {
                retval.Messages.AddRange(matey.ValidateJobs());
            }

            return retval;
        }

        public int CommanderSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Command))
                {
                    string commanderName = AssignedJobs.First(a => a.DutyType == DutyType.Command).CrewName;

                    var commander = ShipsCrew.FirstOrDefault(a => a.Name == commanderName);

                    if (commander != null)
                        retval = commander.CommanderSkillBonus;
                }

                return retval;
            }
        }

        public int CrewPilotModifier
        {
            get
            {
                return SkeletonCrewPenalty; // TODO: What all goes into this?
            }
        }

        public int PilotSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Pilot))
                {
                    string pilotName = AssignedJobs.First(a => a.DutyType == DutyType.Pilot).CrewName;

                    var pilot = ShipsCrew.FirstOrDefault(a => a.Name == pilotName);

                    if (pilot != null)
                        retval = pilot.PilotSkillBonus;
                }

                return retval;
            }
        }

        public int DisciplineSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Discipline))
                {
                    string bosunName = AssignedJobs.First(a => a.DutyType == DutyType.Discipline).CrewName;

                    var bosun = ShipsCrew.FirstOrDefault(a => a.Name == bosunName);

                    if (bosun != null)
                        retval = bosun.DisciplineSkillBonus;
                }

                return retval;
            }
        }

        public int FirstWatchBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Watch))
                {
                    string deckOfficerName = AssignedJobs.First(a => a.DutyType == DutyType.Watch).CrewName;

                    var deckOfficer = ShipsCrew.FirstOrDefault(a => a.Name == deckOfficerName);

                    if (deckOfficer != null)
                        retval = deckOfficer.WatchBonus;
                }

                return retval;
            }
        }

        public int MaintainSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Maintain))
                {
                    string fixitFelixName = AssignedJobs.First(a => a.DutyType == DutyType.Maintain).CrewName;

                    var fixitFelix = ShipsCrew.FirstOrDefault(a => a.Name == fixitFelixName);

                    if (fixitFelix != null)
                        retval = fixitFelix.MaintainSkillBonus;
                }

                return retval;
            }
        }

        public int ManagerSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Manage))
                {
                    string pointyHairedOneName = AssignedJobs.First(a => a.DutyType == DutyType.Manage).CrewName;

                    var pointyHairedOne = ShipsCrew.FirstOrDefault(a => a.Name == pointyHairedOneName);

                    if (pointyHairedOne != null)
                        retval = pointyHairedOne.ManagerSkillBonus;
                }

                return retval;
            }
        }

        public int NavigatorSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Navigate))
                {
                    string navigatorName = AssignedJobs.First(a => a.DutyType == DutyType.Navigate).CrewName;

                    var navigator = ShipsCrew.FirstOrDefault(a => a.Name == navigatorName);

                    if (navigator != null)
                        retval = navigator.NavigatorSkillBonus;
                }

                return retval;
            }
        }

        public int CookingSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Cook))
                {
                    string name = AssignedJobs.First(a => a.DutyType == DutyType.Cook).CrewName;

                    var crewMembert = ShipsCrew.FirstOrDefault(a => a.Name == name);

                    if (crewMembert != null)
                        retval = crewMembert.CookingSkillBonus;
                }

                return retval;

            }

        }
        public int HealSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Heal))
                {
                    string name = AssignedJobs.First(a => a.DutyType == DutyType.Heal).CrewName;

                    var crewMember = ShipsCrew.FirstOrDefault(a => a.Name == name);

                    if (crewMember != null)
                        retval = crewMember.HealSkillBonus;
                }

                return retval;

            }

        }



        public Voyage CurrentVoyage { get; private set; } = new Voyage();

        public Ship()
        {

        }
    }
}
