using System;
using System.Linq;

namespace Nu.Game.Common
{
    public static class DiceRoller
    {
        private static readonly Random rand = new Random(DateTime.Now.Millisecond);

        public static int Roll(string expression)
        {
            throw new NotImplementedException();
        }

        public static int Roll(int sides, int quantity)
        {
            return Enumerable.Repeat(sides + 1, quantity).Sum(x => rand.Next(1, x));
        }

        public static int D20(int quantity)
        {
            return Roll(20, quantity);
        }

        public static int D12(int quantity)
        {
            return Roll(12, quantity);
        }

        public static int D24(int quantity)
        {
            return Roll(24, quantity);
        }

        public static int DPercentile()
        {
            return Roll(100, 1);
        }

        public static int D10(int quantity)
        {
            return Roll(10, quantity);
        }

        public static int D8(int quantity)
        {
            return  Roll(8, quantity);
        }

        public static int D6(int quantity)
        {
            return Roll(6, quantity);
        }

        public static int D4(int quantity)
        {
            return Roll(4, quantity);
        }

        public static int D3(int quantity)
        {
            return Roll(3, quantity);
        }

        public static int D2(int quantity)
        {
            return Roll(2, quantity);
        }
    }
}
