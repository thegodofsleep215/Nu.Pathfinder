namespace  Nu.OfficerMiniGame
{
    public interface IDuty
    {
        void PerformDuty(Ship ship, bool verbose, ref MiniGameStatus status);
    }
}
