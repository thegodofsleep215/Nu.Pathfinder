namespace pfsim.Officer
{
    public interface IDuty
    {
        void PerformDuty(Crew crew, DailyInput input, ref MiniGameStatus status);
    }
}
