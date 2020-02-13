using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class SailingParameters
    {
        public string VoyageName { get; set; }

        public bool NarrowPassage { get; set; }

        public bool ShallowWater { get; set; }

        public bool OpenOcean { get; set; }

        public NightStatus NightStatus { get; set; }

        public List<ShipModifiers> ShipModifiers { get; set; }

        public int PilotingModifier
        {
            get
            {
                int dc = 0;

                dc -= ShallowWater ? 5 : 0;
                dc -= NarrowPassage ? 5 : 0;

                switch (NightStatus)
                {
                    case NightStatus.Underweigh:
                        dc -= 5;
                        break;
                    case NightStatus.Drifting:
                        dc -= 2;
                        break;
                }

                return dc;
            }
        }

        public int NavigationModifier
        {
            get
            {
                int dc = 12;

                dc += OpenOcean ? 5 : 0;

                if (NightStatus == NightStatus.Underweigh)
                    dc += 5;

                return dc;
            }
        }

    }


}
