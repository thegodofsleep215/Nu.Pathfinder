﻿namespace  Nu.OfficerMiniGame
{
    public class SicknessEvent : IShipReportEvent
    {
        public string ShipName { get; set; }
        public int NumberAffected { get; set; }

        public override string ToString()
        {
            return $"{NumberAffected} crew members have fallen ill.";
        }
    }

}
