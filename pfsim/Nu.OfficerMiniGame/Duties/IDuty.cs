using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    public interface IDuty
    {
        List<object> PerformDuty(Ship ship, FleetState state);
    }
}
