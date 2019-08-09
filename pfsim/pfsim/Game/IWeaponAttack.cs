namespace pfsim
{
    public interface IWeaponAttack
    {
        int Bonus { get; set; }

        int DiceSize { get; set; }

        int DiceNumber { get; set; }

        int DamageBonus { get; set; }

        int StartOfCritRange { get; set; }

        int CritMultiplier { get; set; }

        int RollDamage(DiceRoller roller, bool isCrit);


        int RollToHit(DiceRoller roller);
    }
}
