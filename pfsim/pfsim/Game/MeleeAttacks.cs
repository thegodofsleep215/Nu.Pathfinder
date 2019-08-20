using System.Linq;

namespace pfsim
{
    public class MeleeAttacks : IWeaponAttack
    {
        public int Bonus { get; set; }

        public int DiceSize { get; set; }

        public int DiceNumber { get; set; }

        public int DamageBonus { get; set; }

        public int StartOfCritRange { get; set; }

        public int CritMultiplier { get; set; }

        public int RollDamage(bool isCrit)
        {
            return isCrit ? DiceRoller.Roll(DiceSize, DiceNumber * CritMultiplier) + DamageBonus * CritMultiplier :
                DiceRoller.Roll(DiceSize, DiceNumber) + DamageBonus;
        }

        public int RollToHit()
        {
            return DiceRoller.D20(1) + Bonus;
        }
    }
}
