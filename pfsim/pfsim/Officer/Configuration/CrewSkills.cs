using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class CrewSkills
    {
        public int ProfessionSailor { get; set; }
        public int Diplomacy { get; set; }
        public int KnowledgeEngineering { get; set; }
        public int Intimidate { get; set; }
        public int Perception { get; set; }
        public int CraftCarpentry { get; set; }
        public int CraftShip { get; set; }
        public int CraftSails { get; set; }
        public int Survival { get; set; }
        public int ProfessionMerchant { get; set; }
        public int CraftCooking { get; set; }
        public int Heal { get; set; }
        public int Perform { get; set; }

        public CrewSkills()
        {

        }

        public CrewSkills(int universal)
        {
            ProfessionSailor = universal;
            Diplomacy = universal;
            KnowledgeEngineering = universal;
            Intimidate = universal;
            Perception = universal;
            CraftCarpentry = universal;
            CraftShip = universal;
            CraftSails = universal;
            Survival = universal;
            ProfessionMerchant = universal;
            CraftCooking = universal;
            Heal = universal;
            Perform = universal;
        }
    }
}
