using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public enum MoralTypes
    {
        Piracy,
        Infamy,
        Shipshape,
        Wellbeing,
        Wealth
    }

    public class MoralStat
    {
        public int Value
        {
            get
            {
                var result = TemporaryModifiers.Sum() + _value;
                if (result > 5)
                    return 5;
                else if (result < 0)
                    return 0;
                return result;
            }
            set
            {
                if (value > 5)
                    _value = 5;
                else if (value < 0)
                    _value = 0;
                else
                    _value = value;

            }
        }
        private int _value;

        public List<int> TemporaryModifiers { get; set; } = new List<int>();


    }

    [Serializable]
    public class Morale
    {
        public Dictionary<MoralTypes, MoralStat> MoralStats { get; set; }

        public int ShipShape
        {
            get
            {
                return MoralStats[MoralTypes.Shipshape].Value;
            }
            set
            {
                MoralStats[MoralTypes.Shipshape].Value = value;
            }
        }

        public int Wealth
        {
            get
            {
                return MoralStats[MoralTypes.Wealth].Value;
            }
            set
            {
                MoralStats[MoralTypes.Wealth].Value = value;
            }
        }

        public int WellBeing
        {
            get
            {
                return MoralStats[MoralTypes.Wellbeing].Value;
            }
            set
            {
                MoralStats[MoralTypes.Wellbeing].Value = value;
            }
        }

        public int Infamy
        {
            get
            {
                return MoralStats[MoralTypes.Infamy].Value;
            }
            set
            {
                MoralStats[MoralTypes.Infamy].Value = value;
            }
        }

        public int Piracy
        {
            get
            {
                return MoralStats[MoralTypes.Piracy].Value;
            }
            set
            {
                MoralStats[MoralTypes.Piracy].Value = value;
            }
        }

        public int CrewMorale
        {
            get
            {
                return MoralStats.Values.Sum(x => x.Value);
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


        public Morale(int infamy, int shipshape, int wealth, int wellbeing, int piracy)
        {
            MoralStats = new Dictionary<MoralTypes, MoralStat>
            {
                {MoralTypes.Shipshape, new MoralStat{ Value = shipshape } },
                {MoralTypes.Wellbeing, new MoralStat{ Value = wellbeing } },
                {MoralTypes.Wealth, new MoralStat{ Value = wealth } },
                {MoralTypes.Infamy, new MoralStat{ Value = infamy } },
                {MoralTypes.Piracy, new MoralStat{ Value = piracy } }
            };

        }

        public Morale()
        {
            MoralStats = new Dictionary<MoralTypes, MoralStat>
            {
                {MoralTypes.Shipshape, new MoralStat() },
                {MoralTypes.Wellbeing, new MoralStat() },
                {MoralTypes.Wealth, new MoralStat() },
                {MoralTypes.Infamy, new MoralStat() },
                {MoralTypes.Piracy, new MoralStat() }
            };
        }

        public void ClearTemporaryModifiers()
        {
            MoralStats.Values.ToList().ForEach(x => x.TemporaryModifiers.Clear());
        }

        public void AddTemporaryModifier(MoralTypes moralType, int value)
        {
            MoralStats[moralType].TemporaryModifiers.Add(value);
        }

    }
}
