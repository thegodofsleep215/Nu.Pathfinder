namespace pfsim.Officer
{
    /// <summary>
    /// Manage – Allocate resources, tools and supplies from the ship’s stock in order to perform the ship’s daily
    /// necessities such as cooking food, making repairs, etc.   Always the second check of the day.   DC is 
    /// 5 + Ship’s difficulty modifier + 1 / 10 crew.   Failure by means some resources are mismanaged, resulting 
    /// in waste, and the potential loss of ship supplies.   Roll a D20.  If the results are less than or equal to 
    /// the amount that the check was failed by, randomly (1d4) deduct one of food (1), rum (2), water (3), or ship’s 
    /// supplies (4).  Additionally, failure by 10 or more results in obstruction and confusion, resulting in a -4 on 
    /// Cook, Maintain, and Repair checks during that day (including attempts to assist those checks), and a +2 
    /// chance of Discipline problems.  You may have up to two assistants.
    /// </summary>
    public class Manage : IDuty
    {
        public void PerformDuty(IShip crew, DailyInput input, ref MiniGameStatus status)
        {
            var dc = 5 + crew.ShipDc + (crew.CrewSize / 10) + status.CommandModifier;
            status.ManageResult = (DiceRoller.D20(1) + crew.ManagerSkillBonus) - dc;

            if (status.ManageResult >= 0) status.ActionResults.Add("Resources managed.");
            if (status.ManageResult < 0)
            {
                switch (DiceRoller.D4(1))
                {
                    case 1:
                        status.ActionResults.Add("Resources mismanaged resulting in the loss of a rum ration.");
                        break;
                    case 2:
                        status.ActionResults.Add("Resources mismanaged resulting in the loss of a water ration.");
                        break;
                    case 3:
                        status.ActionResults.Add("Resources mismanaged resulting in the loss of a food ration.");
                        break;
                    case 4:
                        status.ActionResults.Add("Resources mismanaged resulting in the loss of ship's supplies.");
                        break;
                }
            }
            if(status.ManageResult <= -10)
            {
                status.ActionResults.Add("The crew is upset at how the resources are being poorly managed."); 
            }
        }
    }
}
