using Nu.Game.Common;
using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nu.OfficerMiniGame
{
    /// <summary>
    /// This class helps sthe <see cref="GameEngine"/> run.
    /// </summary>
    public class Ship
    {
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

        public List<string> SpecialFeatures { get; set; } = new List<string>();

        public List<CrewMember> ShipsCrew { get; set; } = new List<CrewMember>();

        public Morale CrewMorale { get; set; } = new Morale();

        public int CrewQuality
        {
            get
            {
                double retval = 0;
                List<CrewMember> namedCrew;
                // For ships boats, everyone counts as crew
                switch (ShipSize)
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

                if (namedCrew != null && namedCrew.Count > 0)
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

        public bool HasDisciplineOfficer
        {
            get
            {
                return AssignedJobs.Exists(a => a.DutyType == DutyType.Discipline);
            }
        }

        public bool HasHealer
        {
            get
            {
                return AssignedJobs.Exists(a => a.DutyType == DutyType.Heal);
            }
        }

        public DisciplineStandards DisciplineStandards { get; set; }

        public int CrewDisciplineModifier
        {
            get
            {
                int retval = TemporaryDisciplineModifier;

                switch (DisciplineStandards)
                {
                    case DisciplineStandards.Lax:
                        retval += 2;
                        break;
                    case DisciplineStandards.Strict:
                        retval -= 2;
                        break;
                }

                switch (ShipsAlignment)
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
                switch (this.ShipSize)
                {
                    case ShipSize.Medium:
                    case ShipSize.Large:
                    case ShipSize.Huge:
                        return ShipsCrew.Count + Swabbies - DiseasedCrew - CrewUnfitForDuty;  // Allow officers to serve as crew on boats.
                    default:
                        return ShipsCrew.Count(a => a.CountsAsCrew) + Swabbies - DiseasedCrew - CrewUnfitForDuty;
                }
            }
        }

        public int CrewUnfitForDuty { get; set; }

        public bool DiseaseAboardShip
        {
            get
            {
                return DiseasedCrew > 0;
            }
        }

        public int DiseasedCrew { get; set; }

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

        public bool HasMinimumCrew
        {
            get
            {
                return AvailableCrew - MinimumCrewSize >= 0;
            }
        }

        private List<Job> assignedJobs;
        public List<Job> AssignedJobs
        {
            get
            {
                if (assignedJobs == null)
                {
                    assignedJobs = new List<Job>();

                    foreach (var matey in ShipsCrew)
                    {
                        assignedJobs.AddRange(matey.Jobs);
                    }

                }
                return assignedJobs;
            }
        }

        public string GetRandomCrewName(int count = 1)
        {
            var swabCount = Swabbies + ShipsCrew.Count(a => a.CountsAsCrew);
            var mates = ShipsCrew.Where(a => a.CountsAsCrew).ToList();
            HashSet<int> picked = new HashSet<int>();
            StringBuilder sb = new StringBuilder();
            int result;

            if (count > swabCount)
                count = swabCount;

            for (int i = 0; i < count; i++)
            {
                do
                {
                    result = DiceRoller.Roll(swabCount, 1);
                }
                while (picked.Contains(result));

                picked.Add(result);

                if (result > Swabbies)
                {
                    var mate = mates[result - (Swabbies + 1)];

                    sb.AppendLine(string.Format("{0} {1}", mate.Title, mate.Name).Trim());
                }
                else
                {
                    sb.AppendLine(string.Format("Swabby #{0}", result));
                }
            }

            return sb.ToString();
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

                assists.Add(assistance);
            }

            return assists;
        }

        public JobMessage CommanderJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Command, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Command))
                {
                    string commanderName = AssignedJobs.First(a => a.DutyType == DutyType.Command && !a.IsAssistant).CrewName;

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
        public int CrewPilotModifier
        {
            get
            {
                return (SkeletonCrewPenalty + ShipPilotingBonus + CrewQuality + TemporaryPilotingModifier);
            }
        }

        public int TemporaryDisciplineModifier { get; set; }

        public int TemporaryCommandModifier { get; set; }

        public int TemporaryPilotingModifier { get; set; }

        public int TemporaryNavigationModifier { get; set; }

        public int HullDamageSinceRefit { get; set; }

        public int CurrentHullDamage { get; set; }

        public int SailDamageSinceRefit { get; set; }

        public int CurrentSailDamage { get; set; }

        public void AlterHullDamage(int damage)
        {
            if (damage > 0)
            {
                CurrentHullDamage += damage;
                HullDamageSinceRefit += damage;
            }
            else
            {
                if ((CurrentHullDamage - Math.Ceiling(HullDamageSinceRefit * .1)) >= Math.Abs(damage))
                    CurrentHullDamage += damage;
                else
                    CurrentHullDamage = Convert.ToInt32((Math.Ceiling(HullDamageSinceRefit * .1)));
            }
        }

        public void AlterSailDamage(int damage)
        {
            if (damage > 0)
            {
                CurrentSailDamage += damage;
                SailDamageSinceRefit += damage;
            }
            else
            {
                if ((CurrentSailDamage - Math.Ceiling(SailDamageSinceRefit * .1)) >= Math.Abs(damage))
                    CurrentSailDamage += damage;
                else
                    CurrentSailDamage = Convert.ToInt32((Math.Ceiling(SailDamageSinceRefit * .1)));
            }
        }


        public JobMessage PilotJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Pilot, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Pilot))
                {
                    string pilotName = AssignedJobs.First(a => a.DutyType == DutyType.Pilot && !a.IsAssistant).CrewName;

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


        public JobMessage DisciplineJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Pilot, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Discipline))
                {
                    string bosunName = AssignedJobs.First(a => a.DutyType == DutyType.Discipline && !a.IsAssistant).CrewName;

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


        public List<int> WatchBonuses
        {
            get
            {
                int[] watchBonuses = new int[3] { -5, -5, -5 };

                var day = AssignedJobs.Where(a => a.DutyType == DutyType.Watch);
                var i = 0;

                foreach (var watch in day)
                {
                    var lookout = ShipsCrew.FirstOrDefault(a => a.Name == watch.CrewName);

                    if (lookout != null)
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


        public List<int> MinistrelBonuses
        {
            get
            {
                List<int> shantyBonuses = new List<int>();

                var day = AssignedJobs.Where(a => a.DutyType == DutyType.Ministrel);

                foreach (var watch in day)
                {
                    var ministrel = ShipsCrew.FirstOrDefault(a => a.Name == watch.CrewName);

                    if (ministrel != null)
                    {
                        shantyBonuses.Add(ministrel.MinistrelSkillBonus);
                    }
                }

                return shantyBonuses.ToList();
            }
        }


        public JobMessage MaintainJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Maintain, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Maintain))
                {
                    string fixitFelixName = AssignedJobs.First(a => a.DutyType == DutyType.Maintain && !a.IsAssistant).CrewName;

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


        public JobMessage ManagerJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Manage, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Manage))
                {
                    string pointyHairedOneName = AssignedJobs.First(a => a.DutyType == DutyType.Manage && !a.IsAssistant).CrewName;

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


        public JobMessage NavigatorJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Navigate, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Navigate))
                {
                    string navigatorName = AssignedJobs.First(a => a.DutyType == DutyType.Navigate && !a.IsAssistant).CrewName;

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


        public JobMessage HealerJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Heal, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Heal))
                {
                    string healerName = AssignedJobs.First(a => a.DutyType == DutyType.Heal && !a.IsAssistant).CrewName;

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


        public JobMessage CookJob
        {
            get
            {
                JobMessage retval = new JobMessage() { DutyType = DutyType.Cook, SkillBonus = -5, IsAssistant = false, CrewName = "No one" };

                if (AssignedJobs.Exists(a => a.DutyType == DutyType.Cook))
                {
                    string cookName = AssignedJobs.First(a => a.DutyType == DutyType.Cook && !a.IsAssistant).CrewName;

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

        public int CargoPoints { get; set; }

        public CargoHold ShipsCargo { get; private set; } = new CargoHold();

        public bool IsShipOverburdened
        {
            get
            {
                return CargoPoints < ShipsCargo.TotalCargoPointsUsed;
            }
        }

        public decimal OverburdenedFactor
        {
            get
            {
                return 1; // Not tracking cargo for now.
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
    }
}
