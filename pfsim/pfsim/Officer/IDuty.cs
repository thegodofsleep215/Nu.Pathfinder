namespace pfsim.Officer
{
    public interface IDuty
    {
        void PerformDuty(Crew crew, ref MiniGameStatus status);
    }
}
