using pfsim;
using System.Collections.Generic;

namespace pfsim
{

    public class Character
    {
        public string Name { get; set; }

        public int AC { get; set; }

        public int Dexterity { get; set; }

        public string Affiliation { get; set; }

        public MeleeAttacks[] MeleeAttacks { get; set; }

        public int MaxHitPoints { get; set; }

        public int InitiativeBonus { get; set; }
    }
}
