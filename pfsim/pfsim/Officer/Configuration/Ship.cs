﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    [Serializable]
    public class Ship : IShip
    {
        private List<Job> _assignedJobs;
        private List<Propulsion> _propulsionTypes;
        private List<CrewMember> _shipsCrew;
        private Morale _shipsMorale;

        private int MaxPilotAssistants
        {
            get
            {
                switch (ShipSize)
                {
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
        }
        private int MaxCookAssistants
        {
            get
            {
                return 1;
            }
        }
        private int MaxClerks
        {
            get
            {
                return 2;
            }
        }

        public string CrewName { get; set; }
        public ShipType ShipType { get; set; }
        public ShipSize ShipSize { get; set; }
        public List<Propulsion> PropulsionTypes
        {
            get
            {
                if (_propulsionTypes == null)
                    _propulsionTypes = new List<Propulsion>();

                return _propulsionTypes;
            }
            set
            {
                _propulsionTypes = value;
            }
        }
        public int HullHitPoints { get; set; }
        public int CrewSize { get; set; }
        [JsonIgnore]
        public int MinimumCrewSize
        {
            get
            {
                return CrewSize / 2;
            }
        }
        public int ShipDc { get; set; }
        public int ShipPilotingBonus { get; set; }
        public int ShipQuality { get; set; } // TODO: Place holder.

        public List<CrewMember> ShipsCrew
        {
            get
            {
                if (_shipsCrew == null)
                    _shipsCrew = new List<CrewMember>();

                return _shipsCrew;
            }
            set
            {
                _shipsCrew = value;
            }
        }

        public Morale ShipsMorale
        {
            get
            {
                if (_shipsMorale == null)
                    _shipsMorale = new Morale();

                return _shipsMorale;
            }
            set
            {
                _shipsMorale = value;
            }
        }  // TODO: Should morale be part of the voyage?
        [JsonIgnore]
        public int CrewQuality
        {
            get
            {
                double retval = 0;
                var namedCrew = ShipsCrew.Where(a => a.CountsAsCrew).ToList();

                if(namedCrew != null && namedCrew.Count > 0)
                    retval = namedCrew.Select(a => a.ProfessionSailorSkill).Sum();

                retval = Math.Floor(((retval * namedCrew.Count) + (Convert.ToDouble(AverageSwabbieQuality) * Swabbies)) / (namedCrew.Count + Swabbies)) - 4;

                if (retval > 4)
                    return 4;
                else if (retval < -4)
                    return -4;
                else
                    return (int)retval;
            }
        }
        [JsonIgnore]
        public bool HasDisciplineOfficer
        {
            get
            {
                return AssignedJobs.Exists(a => a.DutyType == DutyType.Discipline);
            }
        }
        [JsonIgnore]
        public bool HasHealer
        {
            get
            {
                return AssignedJobs.Exists(a => a.DutyType == DutyType.Heal);
            }
        }
        public DisciplineStandards DisciplineStandards { get; set; }
        public Alignment ShipsAlignment { get; set; }
        public int Marines { get; set; }
        public int Passengers { get; set; }
        public int Swabbies { get; set; }
        public decimal AverageSwabbieQuality { get; set; }
        [JsonIgnore]
        public int TotalCrew
        {
            get
            {
                return ShipsCrew.Count + Swabbies + Marines + Passengers;
            }
        }
        [JsonIgnore]
        public int AvailableCrew
        {
            get
            {
                return ShipsCrew.Count(a => a.CountsAsCrew) + Swabbies - CurrentVoyage.DiseasedCrew - CurrentVoyage.CrewUnfitForDuty;
            }
        }
        [JsonIgnore]
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
        [JsonIgnore]
        public bool HasMinimumCrew
        {
            get
            {
                return AvailableCrew - MinimumCrewSize > 0;
            }
        }

        [JsonIgnore]
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

            if(ShipsCrew.Count != ShipsCrew.Select(a => a.Name).Distinct().Count())
            {
                retval.Messages.Add("Everyone in the crew must have a different name!");
            }
            // This is a bit of a simplification, as I can see situations where you could have different numbers of these, but for now
            // this is complex enough.
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Command && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two commanders!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Command && !a.IsAssistant) > 2)
            {
                retval.Messages.Add("Too many leutinants!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Manage && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two managers!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Watch && !a.IsAssistant) > 3)
            {
                retval.Messages.Add("Too many helmsmen.  There are only three watches per day!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Manage && a.IsAssistant) > MaxClerks)
            {
                retval.Messages.Add("Too many clerks!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Pilot && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two pilots!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Pilot && a.IsAssistant) > MaxPilotAssistants)
            {
                retval.Messages.Add("Too many hands for the ship's wheel!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Navigate && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many Master's Mates!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Navigate && a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many navigator's mates!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Cook && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many cooks spoil the pot!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Cook && a.IsAssistant) > MaxCookAssistants)
            {
                retval.Messages.Add("Too many cook's mates!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Discipline && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Can't have two discipline officers!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Discipline && a.IsAssistant) > 0)
            {
                retval.Messages.Add("Can't have an assistant discipline officer!");
            }

            foreach (var matey in ShipsCrew)
            {
                retval.Messages.AddRange(matey.ValidateJobs());
            }

            if (retval.Messages.Count == 0)
                retval.Success = true;

            return retval;
        }

        public List<Assists> GetAssistance(DutyType duty)
        {
            List<Assists> assists = new List<Assists>();
            var jobs = AssignedJobs.Where(a => a.DutyType == duty && a.IsAssistant);

            foreach (var job in jobs)
            {
                Assists assistance = new Assists();

                assistance.Duty = duty;

                var mate = ShipsCrew.FirstOrDefault(a => a.Name == job.CrewName);

                if (mate != null)
                {
                    switch (duty)
                    {
                        case DutyType.Command:
                            assistance.SkillBonus = mate.CommanderSkillBonus;
                            break;
                        case DutyType.Cook:
                            assistance.SkillBonus = mate.CookSkillBonus;
                            break;
                        case DutyType.Discipline:
                            assistance.SkillBonus = mate.DisciplineSkillBonus;
                            break;
                        case DutyType.Heal:
                            assistance.SkillBonus = mate.HealerSkillBonus;
                            break;
                        case DutyType.Maintain:
                            assistance.SkillBonus = mate.DisciplineSkillBonus;
                            break;
                        case DutyType.Manage:
                            assistance.SkillBonus = mate.ManagerSkillBonus;
                            break;
                        case DutyType.Ministrel:
                            assistance.SkillBonus = mate.MinistrelSkillBonus;
                            break;
                        case DutyType.Navigate:
                            assistance.SkillBonus = mate.NavigatorSkillBonus;
                            break;
                        case DutyType.Pilot:
                            assistance.SkillBonus = mate.PilotSkillBonus;
                            break;
                        case DutyType.Procure:
                            assistance.SkillBonus = mate.ProcureSkillBonus;
                            break;
                        case DutyType.RepairHull: 
                            assistance.SkillBonus = mate.RepairSkillBonus;
                            break;
                        case DutyType.RepairSails:
                            assistance.SkillBonus = mate.RepairSkillBonus;
                            break;
                        case DutyType.RepairSeigeEngine:
                            assistance.SkillBonus = mate.RepairSkillBonus;
                            break;
                        case DutyType.Stow:
                            assistance.SkillBonus = mate.StowSkillBonus;
                            break;
                        case DutyType.Unload:
                            assistance.SkillBonus = mate.UnloadSkillBonus;
                            break;
                        case DutyType.Watch:
                            assistance.SkillBonus = mate.WatchSkillBonus;
                            break;
                    }
                }
            }

            return assists;
        }

        [JsonIgnore]
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

        /// <summary>
        /// In the engine, this adds to DC so the result needs to be inverse of a modification to a dice role.
        /// </summary>
        [JsonIgnore]
        public int CrewPilotModifier
        {
            get
            {
                // TODO: Is the voyage better part of the 'minigame'?
                return (SkeletonCrewPenalty + ShipPilotingBonus + CrewQuality + CurrentVoyage.PilotingModifier) * -1;
            }
        }

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
        public int FirstWatchBonus
        {
            get
            {
                return WatchBonuses[0];
            }
        }

        [JsonIgnore]
        public List<int> WatchBonuses
        {
            get
            {
                int[] watchBonuses = new int[3] { -5, -5, -5 };

                var day = AssignedJobs.Where(a => a.DutyType == DutyType.Watch);
                var i = 0;

                foreach(var watch in day)
                {
                    var lookout = ShipsCrew.FirstOrDefault(a => a.Name == watch.CrewName);

                    if(lookout != null)
                    {
                        watchBonuses[i] = lookout.WatchSkillBonus;
                        i++;
                    }

                    if (i >= 3)
                        break;
                }

                return watchBonuses.ToList();
            }
        }

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
        public int HealerSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Heal))
                {
                    string healerName = AssignedJobs.First(a => a.DutyType == DutyType.Heal).CrewName;

                    var healer = ShipsCrew.FirstOrDefault(a => a.Name == healerName);

                    if (healer != null)
                        retval = healer.HealerSkillBonus;
                }

                return retval;
            }
        }

        [JsonIgnore]
        public int CookSkillBonus
        {
            get
            {
                int retval = -5;

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Cook))
                {
                    string cookName = AssignedJobs.First(a => a.DutyType == DutyType.Cook).CrewName;

                    var cook = ShipsCrew.FirstOrDefault(a => a.Name == cookName);

                    if (cook != null)
                        retval = cook.CookSkillBonus;
                }

                return retval;
            }
        }

        /// <summary>
        /// TODO: Does the ship need a voyage?
        /// </summary>
        public Voyage CurrentVoyage
        {
            get
            {
                if (_currentVoyage == null)
                    _currentVoyage = new Voyage();

                return _currentVoyage;
            }
        }
        private Voyage _currentVoyage;

        public Ship()
        {

        }
    }
}
