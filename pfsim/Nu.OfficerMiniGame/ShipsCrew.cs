using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class ShipsCrew : List<CrewMember>
    {
        public ShipsCrew(IEnumerable<CrewMember> crew) : base(crew)
        {
        }


        public int CountAsCrew => this.Count(x => x.CountsAsCrew);

        public int CountJobs(Func<Job, bool> predicate)
        {
            return this.SelectMany(x => x.Jobs).Count(predicate);
        }

        public List<int> GetAssistanceBonuses(DutyType duty)
        {
            return this.Where(a => a.Jobs.Any(b => b.DutyType == duty && b.IsAssistant)).Select(x => x.GetDutyBonus(duty)).ToList();
        }

        public List<int> GetWatchBonuses()
        {
            return this.Where(a => a.Jobs.Any(b => b.DutyType == DutyType.Watch)).Select(x => x.GetDutyBonus(DutyType.Watch)).ToList();
        }

        public int GetDutyBonus(DutyType duty)
        {
            return this.First(a => a.Jobs.Any(b => b.DutyType == duty && !b.IsAssistant)).GetDutyBonus(duty);
        }

        public bool JobHasAssignedCrewMember(DutyType duty, out string name)
        {
            var member = this.First(c => c.Jobs.Any(x => x.DutyType == duty && !x.IsAssistant));
            if (member == null)
            {
                name = "No onne";
                return false;
            }
            name = $"{member.Title} {member.Name}";
            return true;
        }
        public List<int> WatchBonuses
        {
            get
            {
                int[] watchBonuses = new int[3] { -5, -5, -5 };

                var day = this.SelectMany(x => x.Jobs).Where(a => a.DutyType == DutyType.Watch);
                var i = 0;

                foreach (var watch in day)
                {
                    var lookout = this.FirstOrDefault(a => a.Name == watch.CrewName);

                    if (lookout != null)
                    {
                        watchBonuses[i] = lookout.WatchSkillBonus;
                        i++;
                    }

                    if (i >= 3)
                        break;
                }

                return watchBonuses.ToList();
            }
        }


        public Dictionary<string, int> Ministrels => this.Where(a => a.Jobs.Any(b => b.DutyType == DutyType.Ministrel))
            .ToDictionary(x => x.Name, x => x.MinistrelSkillBonus);

    }
}
