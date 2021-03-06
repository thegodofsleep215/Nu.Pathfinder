﻿using Newtonsoft.Json;
using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame
{
    public class CrewMember
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public CrewSkills Skills { get; set; } = new CrewSkills();

        
        public int PerformSkill => Skills.Perform + WorkModifier + ExternalModifiers.Perform;

        
        public int ProfessionSailorSkill => Skills.ProfessionSailor + WorkModifier + ExternalModifiers.ProfessionSailor;

        
        public int DiplomacySkill => Skills.Diplomacy + WorkModifier + ExternalModifiers.Diplomacy;

        
        public int KnowledgeEngineeringSkill => Skills.KnowledgeEngineering + WorkModifier + ExternalModifiers.KnowledgeEngineering;

        
        public int IntimidateSkill => Skills.Intimidate + WorkModifier + ExternalModifiers.Intimidate;

        
        public int PerceptionSkill => Skills.Perception + WorkModifier + ExternalModifiers.Perception;

        
        public int CraftCarpentrySkill => Skills.CraftCarpentry + WorkModifier + ExternalModifiers.CraftCarpentry;

        
        public int CraftShipSkill => Skills.CraftShip + WorkModifier + ExternalModifiers.CraftShip;

        
        public int CraftCookingSkill => Skills.CraftCooking + WorkModifier + ExternalModifiers.CraftCooking;

        
        public int HealSkill => Skills.Heal + WorkModifier + ExternalModifiers.Heal;

        
        public int SurvivalSkill => Skills.Survival + WorkModifier + ExternalModifiers.Survival;

        
        public int ProfessionMerchantSkill => Skills.ProfessionMerchant + WorkModifier + ExternalModifiers.ProfessionMerchant;

        
        public int CommanderSkillBonus => ProfessionSailorSkill > DiplomacySkill ? ProfessionSailorSkill : DiplomacySkill;

        
        public int CrewPilotModifier => ProfessionSailorSkill;

        
        public int DisciplineSkillBonus => IntimidateSkill;

        
        public int WatchSkillBonus => PerceptionSkill;

        
        public int MaintainSkillBonus => CraftCarpentrySkill > CraftShipSkill ? CraftCarpentrySkill : CraftShipSkill;

        
        public int ManagerSkillBonus
        {
            get
            {
                int retval = KnowledgeEngineeringSkill > ProfessionMerchantSkill ? KnowledgeEngineeringSkill : ProfessionMerchantSkill;

                retval = ProfessionSailorSkill > retval ? ProfessionSailorSkill : retval;

                return retval;
            }
        }

        
        public int NavigatorSkillBonus => ProfessionSailorSkill > SurvivalSkill ? ProfessionSailorSkill : SurvivalSkill;

        
        public int PilotSkillBonus => ProfessionSailorSkill;

        
        public int CookSkillBonus => CraftCookingSkill;

        
        public int HealerSkillBonus => HealSkill;

        
        public int MinistrelSkillBonus => PerformSkill;

        
        public int ProcureSkillBonus => SurvivalSkill;

        // TODO: RepairSail, RepairSiegeEngine
        
        public int RepairSkillBonus => CraftShipSkill > CraftCarpentrySkill ? CraftShipSkill : CraftCarpentrySkill;

        
        public int RepairHullSkillBonus => CraftShipSkill > CraftCarpentrySkill ? CraftShipSkill : CraftCarpentrySkill;

        
        public int StowSkillBonus => ProfessionSailorSkill > KnowledgeEngineeringSkill ? ProfessionSailorSkill : KnowledgeEngineeringSkill;

        
        public int UnloadSkillBonus => ProfessionSailorSkill > KnowledgeEngineeringSkill ? ProfessionSailorSkill : KnowledgeEngineeringSkill;

        public List<Job> Jobs { get; set; } = new List<Job>();

        
        public bool CountsAsCrew
        {
            get
            {
                if (Jobs.Count == 0)
                    return true;
                else if (Jobs.Count(a => !a.IsAssistant || a.DutyType == DutyType.Ministrel) > 0)
                    return false;
                else
                    return Jobs.Count == 1;
            }
        }

        
        public int NumberOfJobs
        {
            get
            {
                if (Jobs.Exists(a => a.DutyType == DutyType.Pilot) && Jobs.Exists(a => a.DutyType == DutyType.Watch))
                    return Jobs.Count - 1;
                else
                    return Jobs.Count;
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

        public CrewSkills ExternalModifiers { get; set; } = new CrewSkills();

        public void AddJob(DutyType duty, bool isAssistant)
        {
            Job job = new Job
            {
                CrewName = Name,
                DutyType = duty,
                IsAssistant = isAssistant
            };

            Jobs.Add(job);
        }

        public void AddJob(Job job)
        {
            job.CrewName = Name;
            Jobs.Add(job);
        }

        public bool RemoveJob(DutyType duty, bool isAssistant)
        {
            var job = Jobs.FirstOrDefault(a => a.DutyType == duty && a.IsAssistant == isAssistant);

            if (job != null)
            {
                return Jobs.Remove(job);
            }
            else
            {
                return false;
            }
        }

        internal IEnumerable<string> ValidateJobs()
        {
            List<string> messages = new List<string>();

            if (Jobs.Count(a => a.DutyType == DutyType.RepairHull || a.DutyType == DutyType.RepairSails || a.DutyType == DutyType.RepairSeigeEngine) > 2)
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
            if (Jobs.Count(a => a.DutyType == DutyType.Cook) > 1)
            {
                messages.Add(string.Format("{0} {1} forgot that cooking twice is just cooking once, in smaller batches!", Title, Name));
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
                                a.DutyType == DutyType.Unload ||
                                a.DutyType == DutyType.RepairHull ||
                                a.DutyType == DutyType.RepairSails ||
                                a.DutyType == DutyType.RepairSeigeEngine) > 2)
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
