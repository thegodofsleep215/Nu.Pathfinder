using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pfsim.Officer
{
    [Serializable]
    public class Ship
    {
        private List<Job> _assignedJobs;
        private List<CrewMember> _shipsCrew;
        private Morale _shipsMorale;

        private int MaxPilotAssistants
        {
            get
            {
                switch (ShipSize)
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
        public List<Propulsion> PropulsionTypes { get; set; } = new List<Propulsion>();
        public int HullHitPoints { get; set; }
        public int CrewSize { get; set; }

        // Below this number, the ship cannot be piloted successfully.
        public int MinimumCrewSize
        {
            get
            {
                if (CrewSize >= 6)
                    return CrewSize / 2;
                else
                    return 1; // Allow boats to be controlled 
            }
        }

        public int ShipDc { get; set; } // General modifier to the difficulty to sail that increases with the size of the ship.
        public int ShipPilotingBonus { get; set; } // Bonus or penalty that the ship recieves for being especially easy to sail.
        public int ShipQuality { get; set; } // Place holder for generic bonus.

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

        public Morale CrewMorale { get; set; } = new Morale();
        
        [JsonIgnore]
        public int CrewQuality
        {
            get
            {
                double retval = 0;
                List<CrewMember> namedCrew;
                // TODO: For ships boats, everyone counts as crew?
                switch(ShipSize)
                {
                    case ShipSize.Medium:
                    case ShipSize.Large:
                    case ShipSize.Huge:
                        namedCrew = ShipsCrew.ToList();
                        break;
                    default:
                        namedCrew = ShipsCrew.Where(a => a.CountsAsCrew).ToList();
                        break;
                }

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
        [JsonIgnore]
        public int CrewDisciplineModifier
        {
            get
            {
                int retval = CurrentVoyage.DisciplineModifier;

                switch(DisciplineStandards)
                {
                    case DisciplineStandards.Lax:
                        retval +=2;
                        break;
                    case DisciplineStandards.Strict:
                        retval -=2;
                        break;
                }

                switch(ShipsAlignment)
                {
                    case Alignment.Chaotic:
                        retval += 2;
                        break;
                    case Alignment.Lawful:
                        retval -= 2;
                        break;
                }

                return retval;
            }
        }
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
                switch(this.ShipSize)
                {
                    case ShipSize.Medium:
                    case ShipSize.Large:
                    case ShipSize.Huge:
                        return ShipsCrew.Count + Swabbies - CurrentVoyage.DiseasedCrew - CurrentVoyage.CrewUnfitForDuty;  // Allow officers to serve as crew on boats.
                    default:
                        return ShipsCrew.Count(a => a.CountsAsCrew) + Swabbies - CurrentVoyage.DiseasedCrew - CurrentVoyage.CrewUnfitForDuty;
                }
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
                return AvailableCrew - MinimumCrewSize >= 0;
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

        public BaseResponse AssignJob(string crewname, DutyType duty, bool isAssistant)
        {
            BaseResponse retval = new BaseResponse();
            retval.Success = false;
            if(ShipsCrew.Exists(a => a.Name == crewname))
            {
                var mate = ShipsCrew.FirstOrDefault(a => a.Name == crewname);

                mate.AddJob(duty, isAssistant);

                _assignedJobs = null;
                retval.Success = true;
            }
            else
            {
                retval.Messages.Add(string.Format("Can't find crewmember '{0}'.", crewname));
            }

            return retval;
        }

        public BaseResponse RemoveJob(string crewname, DutyType duty, bool isAssistant)
        {
            BaseResponse retval = new BaseResponse();
            retval.Success = false;
            if (ShipsCrew.Exists(a => a.Name == crewname))
            {
                var mate = ShipsCrew.FirstOrDefault(a => a.Name == crewname);

                var response = mate.RemoveJob(duty, isAssistant);

                if (response)
                {
                    _assignedJobs = null;
                    retval.Success = true;
                } 
                else
                {
                    retval.Messages.Add(string.Format("Crewmeber {0} already doesn't have duty {1}.", crewname, duty.ToString()));
                }
            }
            else
            {
                retval.Messages.Add(string.Format("Can't find crewmember '{0}'.", crewname));
            }

            return retval;
        }

        public BaseResponse ValidateAssignedJobs(bool sailing = true)
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
                retval.Messages.Add("Can't have two pursurs!");
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
                retval.Messages.Add("Can't have two masters!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Pilot && a.IsAssistant) > MaxPilotAssistants)
            {
                retval.Messages.Add("Too many hands for the ship's wheel!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Navigate && !a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many navigators!");
            }
            if (AssignedJobs.Count(a => a.DutyType == DutyType.Navigate && a.IsAssistant) > 1)
            {
                retval.Messages.Add("Too many quartermasters!");
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

        public List<JobMessage> GetAssistance(DutyType duty)
        {
            List<JobMessage> assists = new List<JobMessage>();
            var jobs = AssignedJobs.Where(a => a.DutyType == duty && a.IsAssistant);

            foreach (var job in jobs)
            {
                JobMessage assistance = new JobMessage();

                assistance.DutyType = duty;

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
        public JobMessage CommanderJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Command, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Command))
                {
                    string commanderName = AssignedJobs.First(a => a.DutyType == DutyType.Command).CrewName;

                    var commander = ShipsCrew.FirstOrDefault(a => a.Name == commanderName);

                    if (commander != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", commander.Title, commander.Name).Trim();
                        retval.SkillBonus = commander.CommanderSkillBonus;
                    }
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
                return (SkeletonCrewPenalty + ShipPilotingBonus + CrewQuality + CurrentVoyage.PilotingModifier);
            }
        }

        [JsonIgnore]
        public JobMessage PilotJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Pilot, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Pilot))
                {
                    string pilotName = AssignedJobs.First(a => a.DutyType == DutyType.Pilot).CrewName;

                    var pilot = ShipsCrew.FirstOrDefault(a => a.Name == pilotName);

                    if (pilot != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", pilot.Title, pilot.Name).Trim();
                        retval.SkillBonus = pilot.PilotSkillBonus;
                    }
                }

                return retval;
            }
        }

        [JsonIgnore]
        public JobMessage DisciplineJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Pilot, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Discipline))
                {
                    string bosunName = AssignedJobs.First(a => a.DutyType == DutyType.Discipline).CrewName;

                    var bosun = ShipsCrew.FirstOrDefault(a => a.Name == bosunName);

                    if (bosun != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", bosun.Title, bosun.Name).Trim();
                        retval.SkillBonus = bosun.DisciplineSkillBonus;
                    }
                }

                return retval;
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
        public List<int> MinistrelBonuses
        {
            get
            {
                List<int> shantyBonuses = new List<int>();

                var day = AssignedJobs.Where(a => a.DutyType == DutyType.Ministrel);
                var i = 0;

                foreach (var watch in day)
                {
                    var ministrel = ShipsCrew.FirstOrDefault(a => a.Name == watch.CrewName);

                    if (ministrel != null)
                    {
                        shantyBonuses[i] = ministrel.MinistrelSkillBonus;
                        i++;
                    }
                }

                return shantyBonuses.ToList();
            }
        }

        [JsonIgnore]
        public JobMessage MaintainJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Maintain, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Maintain))
                {
                    string fixitFelixName = AssignedJobs.First(a => a.DutyType == DutyType.Maintain).CrewName;

                    var fixitFelix = ShipsCrew.FirstOrDefault(a => a.Name == fixitFelixName);

                    if (fixitFelix != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", fixitFelix.Title, fixitFelix.Name).Trim();
                        retval.SkillBonus = fixitFelix.MaintainSkillBonus;
                    }
                }

                return retval;
            }
        }

        [JsonIgnore]
        public JobMessage ManagerJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Manage, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Manage))
                {
                    string pointyHairedOneName = AssignedJobs.First(a => a.DutyType == DutyType.Manage).CrewName;

                    var pointyHairedOne = ShipsCrew.FirstOrDefault(a => a.Name == pointyHairedOneName);

                    if (pointyHairedOne != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", pointyHairedOne.Title, pointyHairedOne.Name).Trim();
                        retval.SkillBonus = pointyHairedOne.ManagerSkillBonus;
                    }
                }

                return retval;
            }
        }

        [JsonIgnore]
        public JobMessage NavigatorJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Navigate, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Navigate))
                {
                    string navigatorName = AssignedJobs.First(a => a.DutyType == DutyType.Navigate).CrewName;

                    var navigator = ShipsCrew.FirstOrDefault(a => a.Name == navigatorName);

                    if (navigator != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", navigator.Title, navigator.Name).Trim();
                        retval.SkillBonus = navigator.NavigatorSkillBonus;
                    }
                }

                return retval;
            }
        }

        [JsonIgnore]
        public JobMessage HealerJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Heal, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Heal))
                {
                    string healerName = AssignedJobs.First(a => a.DutyType == DutyType.Heal).CrewName;

                    var healer = ShipsCrew.FirstOrDefault(a => a.Name == healerName);

                    if (healer != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", healer.Title, healer.Name).Trim();
                        retval.SkillBonus = healer.HealerSkillBonus;
                    }
                }

                return retval;
            }
        }

        [JsonIgnore]
        public JobMessage CookJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Cook, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Cook))
                {
                    string cookName = AssignedJobs.First(a => a.DutyType == DutyType.Cook).CrewName;

                    var cook = ShipsCrew.FirstOrDefault(a => a.Name == cookName);

                    if (cook != null)
                    {
                        retval.CrewName = string.Format("{0} {1}", cook.Title, cook.Name).Trim();
                        retval.SkillBonus = cook.CookSkillBonus;
                    }
                }

                return retval;
            }
        }

        public void AddDaysToVoyage(int days)
        {
            CurrentVoyage.AddDaysToVoyage(days);
            var healCount = AssignedJobs.Count(a => a.DutyType == DutyType.Heal && !a.IsAssistant);
            var repairCount = AssignedJobs.Count(a => (a.DutyType == DutyType.Maintain && !a.IsAssistant)
                            || (a.DutyType == DutyType.RepairHull && !a.IsAssistant)
                            || (a.DutyType == DutyType.RepairSails && !a.IsAssistant)
                            || (a.DutyType == DutyType.RepairSeigeEngine && !a.IsAssistant));

            ShipsCargo.ConsumeSupplies(TotalCrew, days);
            if(healCount > 0) // Note, various medical supplies can be consumed by other than treating disease.  It's best to adjust this separately.
                ShipsCargo.ConsumeSupply(SupplyType.Medicine, (CurrentVoyage.DiseasedCrew + 1) * days);
            if (repairCount > 0)
                ShipsCargo.ConsumeSupply(SupplyType.ShipSupplies, repairCount * days);
            ShipsCargo.ConsumeFodder(ShipsCargo.AnimalUnitsAboard * days);
        }

        public Voyage CurrentVoyage { get; private set; } = new Voyage();

        public int CargoPoints { get; set; }

        public CargoHold ShipsCargo { get; private set; } = new CargoHold();
        
        [JsonIgnore]
        public bool IsShipOverburdened
        {
            get
            {
                return CargoPoints < ShipsCargo.TotalCargoPointsUsed;
            }
        }

        [JsonIgnore]
        public decimal OverburdenedFactor
        {
            get
            {
                if (CargoPoints > ShipsCargo.TotalCargoPointsUsed)
                    return 1;
                else if (CargoPoints == 0 && ShipsCargo.TotalCargoPointsUsed == 0)
                    return 1;
                else if (CargoPoints == 0)
                    return 1 + (((ShipsCargo.TotalCargoPointsUsed * 2) - CargoPoints + 1) / (decimal)CargoPoints + 1);
                else
                    return 1 + ((ShipsCargo.TotalCargoPointsUsed - CargoPoints) / (decimal)CargoPoints);
            }
        }

        public Ship()
        {

        }
    }
}
