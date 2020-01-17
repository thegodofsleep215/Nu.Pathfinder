using System;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    [Serializable]
    public class Propulsion
    {
        public PropulsionType PropulsionType { get; set; }
        public int ShipSpeed { get; set; }
        public int PropulsionHitPoints { get; set; }
    }
}
