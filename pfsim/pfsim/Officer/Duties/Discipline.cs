using System.Collections.Generic;

namespace pfsim.Officer
{
    /// <summary>
    /// Discipline – If no one takes the Discipline job, there is a +4 chance of discipline 
    /// problems and if a discipline problem occurs the check automatically fails. However,
    /// no job check is required unless discipline problem does occur.  Determine if a 
    /// discipline problem occurs as follows:
    ///
    /// Base chance 6
    /// +4 if no officer is responsible for discipline
    /// +4 if command check failed by 15 or more
    /// +2 if management check failed by 10 or more
    /// +2 if crew is basically pirates and cutthroats 
    /// -2 if crew is basically honorable 
    /// +2 if discipline is lax
    /// -2 if discipline is harsh
    /// +/- the crew’s morale bonus
    ///
    /// Roll a D20.If the result is less than or equal to the calculated number, a 
    /// discipline issue has occurred.Regardless of the discipline issue, the officer 
    /// in charge of discipline must immediately make a successful Intimidate check versus 
    /// DC 10 + 1/10 crew, or the crew shipshape is reduced by 1.  The DC of this check 
    /// is modified by the crew’s morale bonus.
    public class Discipline : IDuty
    {
        public void PerformDuty(IShip ship, DailyInput input, ref MiniGameStatus status)
        {
            var tension = 6;
            tension += (ship.HasDisciplineOfficer ? 0 : 4);
            tension += status.CommandResult <= -15 ? 4 : 0;
            tension += status.ManageResult <= -10 ? 2 : 0;
            tension += input.DisciplineModifier;
            tension += (input.CrewMorale * -1);

            var roll = DiceRoller.D20(1);

            if (roll < tension)
            {
                var dc = 10 + (ship.TotalCrew / 10) - status.CommandModifier; 
                if (DiceRoller.D20(1) + ship.DisciplineSkillBonus < dc || !ship.HasDisciplineOfficer)
                {
                    status.DutyEvents.Add($"The crew is getting out of control resulting the following issues, {string.Join(", ", RollUpDisciplineIssues(tension >= 20 ? 1 : 0))}");
                }
                else
                {
                    status.DutyEvents.Add("The crew is getting restless, but they were things were dealt with before they got out of control.");
                }
            }
            else
            {
                status.DutyEvents.Add("The crew is focused on their duties today.");
            }
        }

        List<string> RollUpDisciplineIssues(int modifier)
        {
            var result = new List<string>();
            var roll = DiceRoller.D20(1) + modifier;
            if (roll == 20)
            {
                result.AddRange(RollUpDisciplineIssues(modifier));
                result.AddRange(RollUpDisciplineIssues(modifier));
            }
            else if (roll == 21)
            {
                result.Add($"Severe({DiceRoller.D8(1)}");
            }
            else
            {
                result.Add($"Regular({roll})");
            }
            return result;
        }
    }
}
