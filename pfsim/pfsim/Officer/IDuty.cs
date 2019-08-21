namespace pfsim.Officer
{
    public interface IDuty
    {
        void PerformDuty(IShip crew, ref MiniGameStatus status);
    }
}
