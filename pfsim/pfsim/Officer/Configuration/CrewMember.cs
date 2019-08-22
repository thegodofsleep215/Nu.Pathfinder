using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    [Serializable]
    public class CrewMember
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public CrewSkills Skills
        {
            get
            {
                if (_skills == null)
                    _skills = new CrewSkills();

                return _skills;
            }
            set
            {
                _skills = value;
            }
        }
        private CrewSkills _skills;
        public int ProfessionSailorSkill
        {
            get
            {
                return Skills.ProfessionSailor + WorkModifier + ExternalModifiers.ProfessionSailor;
            }
        }
        public int DiplomacySkill
        {
            get
            {
                return Skills.Diplomacy + WorkModifier + ExternalModifiers.Diplomacy;
            }
        }
        public int KnowledgeEngineeringSkill
        {
            get
            {
                return Skills.KnowledgeEngineering + WorkModifier + ExternalModifiers.KnowledgeEngineering;
            }
        }
        public int IntimidateSkill
        {
            get
            {
                return Skills.Intimidate + WorkModifier + ExternalModifiers.Intimidate;
            }
        }
        public int PerceptionSkill
        {
            get
            {
                return Skills.Perception + WorkModifier + ExternalModifiers.Perception;
            }
        }
        public int PerformSkill
        {
            get
            {
                return Skills.Perform + WorkModifier + ExternalModifiers.Perform;
            }
        }
        public int CraftCarpentrySkill
        {
            get
            {
                return Skills.CraftCarpentry + WorkModifier + ExternalModifiers.CraftCarpentry;
            }
        }
        public int CraftShipSkill
        {
            get
            {
                return Skills.CraftShip + WorkModifier + ExternalModifiers.CraftShip;
            }
        }
        public int CraftCookingSkill
        {
            get
            {
                return Skills.CraftCooking + WorkModifier + ExternalModifiers.CraftCooking;
            }
        }
        public int HealSkill
        {
            get
            {
                return Skills.Heal + WorkModifier + ExternalModifiers.Heal;
            }
        }
        public int SurvivalSkill
        {
            get
            {
                return Skills.Survival + WorkModifier + ExternalModifiers.Survival;
            }
        }
        public int ProfessionMerchantSkill
        {
            get
            {
                return Skills.ProfessionMerchant + WorkModifier + ExternalModifiers.ProfessionMerchant;
            }
        }

        public int CommanderSkillBonus
        {
            get
            {
                return ProfessionSailorSkill > DiplomacySkill ? ProfessionSailorSkill : DiplomacySkill;
            }
        }

        public int DisciplineSkillBonus
        {
            get
            {
                return IntimidateSkill;
            }
        }

        public int HealerSkillBonus
        {
            get
            {
                return HealSkill;
            }
        }

        public int WatchSkillBonus
        {
            get
            {
                return PerceptionSkill;
            }
        }

        public int MaintainSkillBonus
        {
            get
            {
                return CraftCarpentrySkill > CraftShipSkill ? CraftCarpentrySkill : CraftShipSkill;
            }
        }

        public int ManagerSkillBonus
        {
            get
            {
                int retval = KnowledgeEngineeringSkill > ProfessionMerchantSkill ? KnowledgeEngineeringSkill : ProfessionMerchantSkill;

                retval = ProfessionSailorSkill > retval ? ProfessionSailorSkill : retval;

                return retval;
            }
        }

        public int MinistrelSkillBonus
        {
            get
            {
                return PerformSkill;
            }
        }

        public int NavigatorSkillBonus
        {
            get
            {
                return ProfessionSailorSkill > SurvivalSkill ? ProfessionSailorSkill : SurvivalSkill;
            }
        }

        public int PilotSkillBonus
        {
            get
            {
                return ProfessionSailorSkill;
            }
        }

        public int ProcureSkillBonus
        {
            get
            {
                return SurvivalSkill;
            }
        }

        /// <summary>
        /// TODO: This is a simplification.  Probably need to track other repair types separately.
        /// </summary>
        public int RepairSkillBonus
        {
            get
            {
                return CraftShipSkill > CraftCarpentrySkill ? CraftShipSkill : CraftCarpentrySkill;
            }
        }

        public int StowSkillBonus
        {
            get
            {
                return ProfessionSailorSkill > KnowledgeEngineeringSkill ? ProfessionSailorSkill : KnowledgeEngineeringSkill;
            }
        }

        public int UnloadSkillBonus
        {
            get
            {
                return ProfessionSailorSkill > KnowledgeEngineeringSkill ? ProfessionSailorSkill : KnowledgeEngineeringSkill;
            }
        }

        public int CookSkillBonus
        {
            get
            {
                return CraftCookingSkill;
            }
        }

        public List<Job> Jobs
        {
            get
            {
                if (_jobs == null)
                    _jobs = new List<Job>();

                return _jobs;
            }
            set
            {
                _jobs = value;
            }
        }
        private List<Job> _jobs;

        public bool CountsAsCrew
        {
            get
            {
                if (_jobs.Count == 0)
                    return true;
                else if (_jobs.Count(a => !a.IsAssistant || a.DutyType == DutyType.Ministrel) > 0)
                    return false;
                else
                    return Jobs.Count == 1;
            }
        }

        public int NumberOfJobs
        {
            get
            {
                if (_jobs.Exists(a => a.DutyType == DutyType.Pilot) && _jobs.Exists(a => a.DutyType == DutyType.Watch))
                    return _jobs.Count - 1;
                else
                    return _jobs.Count;
            }
        }

        public int WorkModifier
        {
            get
            {
                var temp = NumberOfJobs;
                if (temp <= 1)
                    return 0;
                else if (temp == 2)
                    return -2;
                else if (temp == 3)
                    return -5;
                else
                    return -9;
            }
        }

        public CrewSkills ExternalModifiers
        {
            get
            {
                if (_externalModifiers == null)
                    _externalModifiers = new CrewSkills();

                return _externalModifiers;
            }
            set
            {
                _externalModifiers = value;
            }
        }
        private CrewSkills _externalModifiers;

        public void AddJob(Job job)
        {
            job.CrewName = Name;
            Jobs.Add(job);
        }

        internal IEnumerable<string> ValidateJobs()
        {
            List<string> messages = new List<string>();

            if (Jobs.Count(a => a.DutyType == DutyType.Repair) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to repair everything!", Title, Name));
            }
            if (Jobs.Count(a => a.DutyType == DutyType.Stow) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to put it all away!", Title, Name));
            }
            if (Jobs.Count(a => a.DutyType == DutyType.Heal) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to heal everyone!", Title, Name));
            }
            if (Jobs.Count(a => a.DutyType == DutyType.Ministrel) > 2)
            {
                messages.Add(string.Format("{0} {1}'s voice would get tired!", Title, Name));
            }
            if (Jobs.Count(a => a.DutyType == DutyType.Procure) > 2)
            {
                messages.Add(string.Format("Not enough time in the day for {0} {1} to catch all the fish!", Title, Name));
            }
            if (Jobs.Count(a => a.DutyType == DutyType.Procure ||
                                a.DutyType == DutyType.Ministrel ||
                                a.DutyType == DutyType.Heal ||
                                a.DutyType == DutyType.Stow ||
                                a.DutyType == DutyType.Repair) > 2)
            {
                messages.Add(string.Format("{0} {1} can't do all the work by himself!", Title, Name));
            }

            return messages;
        }

        public CrewMember()
        {

        }
    }
}
