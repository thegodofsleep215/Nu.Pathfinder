using System.Collections.Generic;

namespace  Nu.OfficerMiniGame
{
    public class UnrulyCrewEvent
    {
        public static readonly Dictionary<int, string> StandardDisciplineResults = new Dictionary<int, string>
        {
            {1, "One crew member was in a particularly black mood today, and spent the whole day surly, insubordinate, and curt with his crewmates. Although the crew member technically did his job, he’s annoyed almost everyone around him. There are no significant problems arising from this other than the loss of morale if the officer in charge of Discipline doesn’t correct the issue in the eyes of the crew." },
            {2, "Ineptness or laziness by a crew member injured another crew member (1d10 damage)."},
            {3, "1d3 crew members were flagrantly derelict of duty - engaging in some leisure activity or sleeping when on duty. They must be punished severely enough to render them unfit for service for 1 day, or the crew will be dissatisfied, and morale will decrease by 1. (This is in addition to the results of a failed Discipline check.)"},
            {4, "A severe argument broke out between two of the crew members, resulting in weapons being drawn and threats made. Either both crew members must be so severely punished that they are left unfit for service for 1d4 days, or else the chance of discipline issues increases by +1 for the next month."},
            {5, "A severe argument broke out between two factions of the crew with 1d4+1 members on each side hurling insults and making threats. Command checks are at +2 DC until the argument is arbitrated successfully, or all of one faction is removed from the crew."},
            {6, "One of the crew members physically assaulted another crew member, resulting in 1d6 damage and 2d6 non-lethal damage. The two crew members are hostile to each other now and command checks are at +1 DC until either one of the crew members leaves the crew or the two are reconciled."},
            {7, "Lax behavior by a crew member has destroyed 1 cargo point worth of supplies."},
            {8, "Lax behavior by a crew member has destroyed 1 cargo point worth of ship’s food."},
            {9, "Lax behavior by a crew member has destroyed 1 cargo point worth of ship’s rum."},
            {10, "One of the crew members has engaged in such odious personal behavior, that it has effected the opinion of the rest of the crew and no one wants to work with that crew member anymore. Command checks are at +1 DC until the crew member either reforms or is removed from the crew."},
            {11, "One of the crew members or junior officers has publicly upbraided and disrespected one of the senior officers. That crew member can no longer engage in assist actions for that officer. Either the crew member must be so harshly punished that they are left unfit for service for 1d4 days, or else all assist actions made to assist that officer are at a -2 penalty for the next month."},
            {12, "A crew member was so negligent that the ship has suffered damage to propulsion, such as an important sail being shredded or blown away."},
            {13, "A crew member was so negligent that the ship has suffered damage to either the hull or propulsion (50% chance of each) through neglect."},
            {14, "A crew member has been persistently negligent for a long time in some duty and gotten away with it, such that the ship has suffered damage to either the hull or propulsion (50% chance of each) amounting to 2d4 times the usual amount of neglect."},
            {15, "A crew member was so negligent that they injured themselves (1d10 damage)."},
            {16, "Negligence by a crew member has rendered an important shipboard system (a siege engine, the ship’s wheel, a capstan, the ship’s stove, a ship’s pump) unfunctional until repairs can be effected. Repairs require one day, a successful repair action, and reduce ship supplies by 1 day."},
            {17, "Negligence by a crew member has put a perishable plunder point at risk! An immediate successful management check is required to prevent further damage, or the plunder point is lost. Morale will be effected accordingly, and the crew will expect that the crew member will be punished severely enough that they are left unfit for service for 1d4 days. Even so, hostility will exist between that crew member and the rest of the crew and command checks are at +1 DC until that crew member leaves the crew or does something to redeem themselves in the eyes of the crew."},
            {18, "A crew member violated the Code in some fashion, accidently or out of negligence. Several Crew members saw the incident, and agreed not to report it but to settle it amongst themselves. However, one of the crew members harbors as secret grudge against the offender, or else particularly wants to ingratiate themselves to one of the Officers and has tipped off the Officers. Until the problem is uncovered and the behavior dealt with, the grudge so sours the crew that there is a +1 chance of discipline problems."},
            {19, "As above, but at least one Ship’s officer was present at the time of the incident, and may at least be partly to blame. Roll again to determine what happened and then concoct a scenario where the officer must make a skill check. If the officer fails, they are at least partly to blame, and the officer must somehow make it right or they will be one step more disliked by 3-6 members of the crew. If the officer doesn’t fail, they must make a diplomacy check modified by ship’s Morale modifier on social checks of DC 12 or they will be blamed anyway, and the same situation then applies."},
        };

        public static readonly Dictionary<int, string> SevereDisciplineResults = new Dictionary<int, string>
        {
            {1, "A Crew member with a less than friendly relationship to that officer attacks the officer with intent to kill, attempting to take them when their back is turned. (Sense Motive or Perception to avoid, depending on the situation.)"},
            {2, "A Crew member responds to an order or chastisement by an officer with blind anger, and attempts to beat down the officer with his fists."},
            {3, "A Crew member responds to an order or chastisement by an officer with blind anger, and attempts to beat down the officer with his fists. But to make matters worse, this provokes a general brawl as 2-5 crew members side with either party."},
            {4, "A murder has occurred! Two crew members with a grudge against each other got in an argument and it escalated quickly. Now the lower level crew member is dead. (If they were the same level, pick the NPC less likely to win or determine randomly)"},
            {5, "A Crew member with a grudge against the officers attempted to steal an item of value from the stores, but has been caught in the act by the Purser."},
            {6, "A Crew member with a grudge against the officers has stolen an item of value from the stores and has hidden it amongst his personal effects."},
            {7, "A cell of 3-6 crew members who are most hostile toward the captain or one of the other officers is overheard plotting mutiny by random member of the crew or officers that is not in on the plot. If that crew member is not at least friendly with the captain, they say nothing and the plot remains undetected. If the crew member is an officer or known friendly with the captain, they must make a DC 15 bluff of stealth check (character’s choice) or the mutinous crew will attempt to kill them to avoid the plot getting out. If the existence of the plot does not come to the attention of the officers, the next time this discipline issue is rolled, the plot will have grown by a further 3-6 members."},
            {8, "A crew member has been carefully plotting to undermine the authority of one of the officers whom they despise through a coordinated campaign of gossip, slander, japes, and sabotage. Each day thereafter, the officer or officer in charge of Discipline must make a Sense Motive check with DC 15 to notice the insubordinate smirks and barely concealed disdain almost all of the crew now has for the officer who has been the target of this campaign. Until the situation is remedied in some fashion that causes the officer to regain the respect of the crew, all skill checks by that officer pertaining to shipboard matters have a -2 penalty."},
        };

        public string ShipName { get; set; }
        public int Roll { get; set; }

        public bool IsSerious { get; set; }

        public string Description
        {
            get
            {
                if(IsSerious && SevereDisciplineResults.ContainsKey(Roll))
                {
                    return SevereDisciplineResults[Roll];
                }
                if (StandardDisciplineResults.ContainsKey(Roll))
                {
                    return StandardDisciplineResults[Roll];
                }
                return "No Description.";
            }
        }

        public override string ToString()
        {
            var type = IsSerious ? "serious" : "regular";
            return $"Unruly Crew Event (Roll:{Roll} IsSerious:{type}): {Description}";
        }
    }

}
