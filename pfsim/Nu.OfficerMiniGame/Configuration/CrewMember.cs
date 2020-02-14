using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class CrewMember
    {
        public CrewMember(string name, string title, CrewSkills skills)
        {
            Name = name;
            this.skills = skills;
            this.Title = title;
        }

        public string Name { get; private set; }

        public string Title { get; private set; }

        public List<Job> Jobs { get; set; } = new List<Job>();

        private CrewSkills skills;

        public int GetDutyBonus(DutyType duty)
        {
            switch (duty)
            {
                case DutyType.Command:
                    return CommanderSkillBonus;
                case DutyType.Cook:
                   return CookSkillBonus;
                case DutyType.Discipline:
                    return DisciplineSkillBonus;
                case DutyType.Heal:
                    return HealerSkillBonus;
                case DutyType.Maintain:
                    return DisciplineSkillBonus;
                case DutyType.Manage:
                    return ManagerSkillBonus;
                case DutyType.Ministrel:
                    return MinistrelSkillBonus;
                case DutyType.Navigate:
                    return NavigatorSkillBonus;
                case DutyType.Pilot:
                    return PilotSkillBonus;
                case DutyType.Procure:
                    return ProcureSkillBonus;
                case DutyType.RepairHull:
                    return RepairSkillBonus;
                case DutyType.RepairSails:
                    return RepairSkillBonus;
                case DutyType.RepairSeigeEngine:
                    return RepairSkillBonus;
                case DutyType.Stow:
                    return StowSkillBonus;
                case DutyType.Unload:
                    return UnloadSkillBonus;
                case DutyType.Watch:
                    return WatchSkillBonus;
            }
            throw new NotImplementedException();
        }

        public int CommanderSkillBonus => (skills.ProfessionSailor > skills.Diplomacy ? skills.ProfessionSailor : skills.Diplomacy) + WorkModifier;

        public int CrewPilotModifier => skills.ProfessionSailor + WorkModifier;

        public int DisciplineSkillBonus => skills.Intimidate + WorkModifier;

        public int WatchSkillBonus => skills.Perception + WorkModifier;

        public int MaintainSkillBonus => (skills.CraftCarpentry > skills.CraftShip ? skills.CraftCarpentry : skills.CraftShip) + WorkModifier;

        public int ManagerSkillBonus
        {
            get
            {
                int retval = skills.KnowledgeEngineering > skills.ProfessionMerchant ? skills.KnowledgeEngineering : skills.ProfessionMerchant;

                retval = skills.ProfessionSailor > retval ? skills.ProfessionSailor : retval;

                return retval + WorkModifier;
            }
        }

        public int NavigatorSkillBonus => (skills.ProfessionSailor > skills.Survival ? skills.ProfessionSailor : skills.Survival) + WorkModifier;

        public int PilotSkillBonus => skills.ProfessionSailor + WorkModifier;

        public int CookSkillBonus => skills.CraftCooking + WorkModifier;

        public int HealerSkillBonus => skills.Heal + WorkModifier;

        public int MinistrelSkillBonus => skills.Perform + WorkModifier;

        public int ProcureSkillBonus => skills.Survival + WorkModifier;

        public int RepairSkillBonus => (skills.CraftShip > skills.CraftCarpentry ? skills.CraftShip : skills.CraftCarpentry) + WorkModifier;

        public int RepairHullSkillBonus => (skills.CraftShip > skills.CraftCarpentry ? skills.CraftShip : skills.CraftCarpentry) + WorkModifier;

        public int StowSkillBonus => (skills.ProfessionSailor > skills.KnowledgeEngineering ? skills.ProfessionSailor : skills.KnowledgeEngineering) + WorkModifier;

        public int UnloadSkillBonus => (skills.ProfessionSailor > skills.KnowledgeEngineering ? skills.ProfessionSailor : skills.KnowledgeEngineering) + WorkModifier;

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
    }
}
