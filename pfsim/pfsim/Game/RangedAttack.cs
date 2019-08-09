namespace pfsim
{
    public class RangedAttack : IWeaponAttack
    {
        public int Bonus { get; set; }
        public int DiceSize { get; set; }
        public int DiceNumber { get; set; }
        public int DamageBonus { get; set; }
        public int StartOfCritRange { get; set; }
        public int CritMultiplier { get; set; }

        public int RollDamage(DiceRoller roller, bool isCrit)
        {
            throw new System.NotImplementedException();
        }

        public int RollToHit(DiceRoller roller)
        {
            throw new System.NotImplementedException();
        }
    }
}
