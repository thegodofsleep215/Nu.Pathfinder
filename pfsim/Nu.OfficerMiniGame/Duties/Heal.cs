﻿using Nu.Game.Common;
using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    /// Heal – Treat illness and help contain it and prevent its spread.   In practice, the normal application 
    /// of this job works much like Discipline.   No one has to take it, but if no one takes it then it is 
    /// more likely that problems develop.   If no one takes the Heal job, there is a +4 chance of a medical problem.   
    ///
    ///    Base chance 2
    /// +4 if no one takes the heal job
    /// +4 if the Cook check failed by 15 or more the prior day.
    /// +2 if wellbeing is 2.
    /// +4 if wellbeing is 1 or less.
    /// +4 if someone is already sick aboard the ship
    /// +4 if there is insufficient crew to clean the ship, or the ship is otherwise kept in a slovenly state.
    ///
    /// Roll a D20.If the number is less than or equal to the resulting number, 1 member of the crew has come down 
    /// with an illness, and a Heal check to treat disease (at the diseases DC) is required. If this check’s DC is 
    /// beaten by an amount equal to the ship’s difficulty modifier, and there are open berths in the sick bay, 
    /// then the healer also avoids the +4 chance of illness resulting from there being someone already sick aboard
    /// the ship via preventative measures.  Otherwise, the penalty applies until the sick person is healed. If the
    /// total base chance is above 20, then 1d3 members of the crew (rather than 1) automatically become ill.
    ///
    /// In addition to providing treatment up to 8 sick patients, a healer may do 1 of the following while
    /// performing the duties of a Healer
    ///
    /// 1.	Maintain long term care on 6 patients.
    /// 2.	Heal lethal wounds on up to 8 patients.
    /// 3.	Bind wounds on up to 32 injured patients.
    /// 4.	Treat disease on up to 24 additional sick patients.
    ///
    /// The healer can attempt to do 2 or more of those jobs, but if they do then the normal penalties for 
    /// performing multiple jobs apply.  A healer may have 1 assistant.  In addition to the normal benefits of an 
    /// assist check, a successful assist check increases by 2 the number of patients a Healer may maintain in 
    /// long term care or treat for lethal wounds, and by 8 the number of sick or injured patients that the Healer 
    /// can provide basic care for.
    ///
    /// Infection aboard ship is common and serious.  If a healer does not wash and provide basic dressing for 
    /// injured characters (such wounds as in battle, or lashes received from punishment) a process roughly as 
    /// difficult as providing first aid, then each injured character should be checked for a chance of disease.  
    /// If disease is indicated, that character acquires a serious infection 1d4 days after receiving the injury.  
    public class Heal : IDuty
    {
        public void PerformDuty(Ship ship, bool verbose, ref MiniGameStatus status)
        {
            var santitation = ship.TotalCrew / 10;
            santitation += ship.HasHealer ? 0 : 4;
            santitation += status.CookResult <= -15 ? 4 : 0;
            santitation += ship.CrewMorale.WellBeing == 2 ? 2 : 0;
            santitation += ship.CrewMorale.WellBeing <= 1 ? 4 : 0;
            santitation += ship.DiseaseAboardShip ? 4 : 0;

            var assistBonus = PerformAssists(ship.GetAssistance(DutyType.Heal));
            var job = ship.HealerJob;
            var result = DiceRoller.D20(1) + job.SkillBonus + assistBonus - santitation;
            if(verbose)
                status.GameEvents.Add(new PerformedDutyEvent(DutyType.Heal, job.CrewName, santitation, assistBonus, job.SkillBonus, result));

            if (result < 0 || !ship.HasHealer)
            {
                // The number of sick should scale better with the size of the crew.
                var sickCount = santitation >= 20 ? DiceRoller.D3(1) * DiceRoller.Roll(ship.TotalCrew / 10, 1) : DiceRoller.Roll(ship.TotalCrew / 10, 1);
                status.GameEvents.Add(new SicknessEvent { NumberAffected = sickCount });
            }
        }

        private int PerformAssists(List<JobMessage> list)
        {
            int retval = 0;

            foreach (var assist in list)
            {
                retval += (DiceRoller.D20(1) + assist.SkillBonus >= 10) ? 2 : 0;
            }

            return retval;
        }
    }

}
