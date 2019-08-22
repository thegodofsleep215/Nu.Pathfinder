namespace pfsim.Officer
{
    public interface IDuty
    {
        void PerformDuty(Ship ship, DailyInput input, ref MiniGameStatus status);
    }
}
