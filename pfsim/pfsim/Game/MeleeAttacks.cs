namespace pfsim
{
    public class MeleeAttacks : IWeaponAttack
    {
        public int Bonus { get; set; }

        public int DiceSize { get; set; }

        public int DiceNumber { get; set; }

        public int DamageBonus { get; set; }

        public int StartOfCritRange { get; set; }
    }
}
