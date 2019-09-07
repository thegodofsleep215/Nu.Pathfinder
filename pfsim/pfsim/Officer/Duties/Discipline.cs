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
        public void PerformDuty(Ship ship, ref MiniGameStatus status)
        {
            var tension = 6;
            tension += (ship.HasDisciplineOfficer ? 0 : 4);
            tension += status.CommandResult <= -15 ? 4 : 0;
            tension += status.ManageResult <= -10 ? 2 : 0;
            tension += ship.CrewDisciplineModifier;
            tension += (ship.CrewMorale.MoraleBonus * -1);

            var roll = DiceRoller.D20(1);

            if (roll < tension)
            {
                var dc = 10 + (ship.TotalCrew / 10) - status.CommandModifier;
                var job = ship.DisciplineJob;
                var result = DiceRoller.D20(1) + job.SkillBonus - dc;
                if(result < 0 && result >= -2)
                {
                    // Use a ministrel.
                    int ministrel = status.MinistrelResults.Count;

                    if (ship.MinistrelBonuses.Count > ministrel)
                    {
                        dc = 10 + (ship.TotalCrew / 10) > 15 ? 10 + (ship.TotalCrew / 10) : 15;
                        var shanty = DiceRoller.D20(1) + ship.MinistrelBonuses[ministrel] - dc;
                        status.MinistrelResults.Add(shanty);
                        if (shanty >= 0)
                        {
                            result += 2;
                            status.DutyEvents.Add(new SeaShantyEvent(DutyType.Discipline));
                        }
                    }
                }
                if(SettingsManager.Verbose)
                    status.DutyEvents.Add(new PerformedDutyEvent(DutyType.Discipline, job.CrewName, dc, 0, job.SkillBonus, result));
                if (result < 0 || !ship.HasDisciplineOfficer)
                {
                    status.DutyEvents.AddRange(RollUpDisciplineIssues(tension >= 20 ? 1 : 0));
                }
            }
        }

        List<UnrulyCrewEvent> RollUpDisciplineIssues(int modifier)
        {
            var result = new List<UnrulyCrewEvent>();
            var roll = DiceRoller.D20(1) + modifier;
            if (roll == 20)
            {
                result.AddRange(RollUpDisciplineIssues(modifier));
                result.AddRange(RollUpDisciplineIssues(modifier));
            }
            else if (roll == 21)
            {
                result.Add(new UnrulyCrewEvent
                {
                    Roll = DiceRoller.D8(1),
                    IsSerious = true
                });
            }
            else
            {
                result.Add(new UnrulyCrewEvent
                {
                    Roll = roll,
                    IsSerious = false
                });
            }
            return result;
        }
    }
}
