namespace pfsim.Officer
{
    public interface IDuty
    {
        void PerformDuty(IShip crew, DailyInput input, ref MiniGameStatus status);
    }
}
