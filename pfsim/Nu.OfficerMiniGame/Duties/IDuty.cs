using Nu.OfficerMiniGame.Dal.Dto;

namespace  Nu.OfficerMiniGame
{
    public interface IDuty
    {
        void PerformDuty(Ship ship, ref MiniGameStatus status);
    }
}
