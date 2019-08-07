using pfsim;

namespace pfsim
{
    public class Character
    {
        public string Name { get; set; }

        public int AC { get; set; }

        public MeleeAttacks[] MeleeAttacks { get; set; }

        public int MaxHitPoints { get; set; }

    }
}
