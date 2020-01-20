namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class PlanVoyage
    {
        public string Name { get; set; }

        public string Port { get; set; }

        public string DestinationPort { get; set; }

        public string[] ShipLoadouts { get; set; }

        public int DaysPlanned { get; set; }

        public NightStatus NightStatus { get; set; }

        public bool OpenOcean { get; set; }

        public bool ShallowWater { get; set; }

        public bool NarrowPassage { get; set; }

        public bool Underweigh { get; set; }
    }
}
