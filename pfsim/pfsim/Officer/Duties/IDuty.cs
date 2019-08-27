namespace pfsim.Officer
{
    public interface IDuty
    {
        void PerformDuty(IShip ship, DailyInput input, ref MiniGameStatus status);
    }
}
