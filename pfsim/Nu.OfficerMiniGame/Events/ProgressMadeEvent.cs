namespace Nu.OfficerMiniGame
{
    public class ProgressMadeEvent
    {
        public string ShipName { get; set; }
        public double DaysofProgress { get; set; }

        public override string ToString()
        {
            return $"{DaysofProgress} days of progress have been made.";
        }
    }

}
