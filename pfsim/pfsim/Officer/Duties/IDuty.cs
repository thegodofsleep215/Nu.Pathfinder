namespace pfsim.Officer
{
    public interface IDuty
    {
        void PerformDuty(Ship ship, ref MiniGameStatus status);
    }
}
