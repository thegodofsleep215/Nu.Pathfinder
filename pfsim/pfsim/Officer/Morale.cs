using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    [Serializable]
    public class Morale
    {
        public int ShipShape
        {
            get
            {
                return _shipShape;
            }
            set
            {
                if (value > 5)
                    _shipShape = 5;
                else if (value < 0)
                    _shipShape = 0;
                else
                    _shipShape = value;
            }
        }
        private int _shipShape;

        public int Wealth
        {
            get
            {
                return _wealth;
            }
            set
            {
                if (value > 5)
                    _wealth = 5;
                else if (value < 0)
                    _wealth = 0;
                else
                    _wealth = value;
            }
        }
        private int _wealth;

        public int WellBeing
        {
            get
            {
                return _wellBeing;
            }
            set
            {
                if (value > 5)
                    _wellBeing = 5;
                else if (value < 0)
                    _wellBeing = 0;
                else
                    _wellBeing = value;
            }
        }
        private int _wellBeing;

        public int Infamy
        {
            get
            {
                return _infamy;
            }
            set
            {
                if (value > 5)
                    _infamy = 5;
                else if (value < 0)
                    _infamy = 0;
                else
                    _infamy = value;
            }
        }
        private int _infamy;

        public int CrewMorale
        {
            get
            {
                return _infamy + _wellBeing + _wealth + _shipShape;
            }
        }

        public int MoraleBonus
        {
            get
            {
                var temp = CrewMorale;
                if (temp <= 5)
                    return -4;
                else if (temp <= 10)
                    return -2;
                else if (temp >= 15)
                    return +2;
                else if (temp >= 20)
                    return +4;
                else
                    return 0;
            }
        }

        public int SocialBonus
        {
            get
            {
                var temp = CrewMorale;
                if (temp <= 5)
                    return -10;
                else if (temp <= 10)
                    return -5;
                else if (temp >= 15)
                    return +5;
                else if (temp >= 20)
                    return +10;
                else
                    return 0;
            }
        }


        public Morale(int infamy, int shipshape, int wealth, int wellbeing)
        {
            _wealth = wealth;
            _infamy = infamy;
            _wellBeing = wellbeing;
            _shipShape = shipshape;
        }

        public Morale()
        {
        }
    }
}
