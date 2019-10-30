using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Nu.OfficerMiniGame
{
    public class PerformedDutyEvent
    {
        private DutyType _duty;
        private string _crew;
        private int _dc;
        private int _roll;
        private int _result;
        private int _assist;
        private int _skill;

        public PerformedDutyEvent(DutyType duty, string crew, int dc, int assist, int skill, int result)
        {
            _duty = duty;
            _crew = crew;
            _dc = dc;
            _assist = assist;
            _skill = skill;
            _result = result;
        }

        public override string ToString()
        {
            return string.Format("{0} performed the duty of {1} which had a difficulty of {2}.  The sailor had a skill of {3} with an assistance of {4}, and the result was {5}.", _crew, _duty.ToString(), _dc, _skill, _assist, _result);
        }
    }
}
