using Nu.OfficerMiniGame.Dal.Dal;
using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame
{
    public class Ammunition : Supplies, IAmmunition
    {
        public SiegeEngineType SeigeEngineType { get; set; }
    }
}
