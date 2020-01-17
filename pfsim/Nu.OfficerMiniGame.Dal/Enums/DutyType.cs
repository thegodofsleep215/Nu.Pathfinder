using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DutyType
    {
        Command,
        Manage,
        Pilot,
        Watch,
        Navigate,
        Maintain,
        Discipline,
        Cook,
        Heal,
        Stow,
        Unload,
        RepairHull,
        RepairSails,
        RepairSeigeEngine,
        Procure,
        Ministrel,
        Drill
    }
}
