using System;
using System.Linq;

namespace pfsim
{
    public class DiceRoller
    {
        private Random rand = new Random();

        public int D20(int quantity)
        {
            return Roll(20, quantity);
        }

        public int Roll(int sides, int quantity)
        {
            return Enumerable.Repeat(sides + 1, quantity).Sum(x => rand.Next(1, x));
        }
    }
}
