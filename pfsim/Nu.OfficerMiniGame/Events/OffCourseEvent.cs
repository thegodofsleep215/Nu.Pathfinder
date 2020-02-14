namespace  Nu.OfficerMiniGame
{
    public class OffCourseEvent : IShipReportEvent
    {
        public string ShipName { get; set; }
        private bool _lost = false;
        public OffCourseEvent()
        {

        }

        public OffCourseEvent(bool lost)
        {
            _lost = lost;
        }

        public override string ToString()
        {
            if (_lost)
                return "The navigator has lead the ship badly off course, and now the ship's position is uncertain and it is further from its destination than when it began the day.";
            else
                return "The navigator has lead the ship off course resulting in reduced progress for the day.";
        }
    }
}
