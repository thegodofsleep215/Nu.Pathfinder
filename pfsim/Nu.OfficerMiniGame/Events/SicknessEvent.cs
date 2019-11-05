namespace  Nu.OfficerMiniGame
{
    public class SicknessEvent
    {
        public int NumberAffected { get; set; }

        public override string ToString()
        {
            return $"{NumberAffected} crew members have fallen ill.";
        }
    }

}
