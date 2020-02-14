﻿using Nu.OfficerMiniGame.Dal.Dal;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class MiniGameDal
    {
        private readonly IShipLoadoutDal shipLoadoutDal;
        private readonly IShipStatsDal shipStatsDal;
        private readonly ICrewMemberStatsDal crewMemberStatsDal;

        public MiniGameDal(IShipLoadoutDal shipLoadoutDal, IShipStatsDal shipStatsDal,
            ICrewMemberStatsDal crewMemberStatsDal)
        {
            this.shipLoadoutDal = shipLoadoutDal;
            this.shipStatsDal = shipStatsDal;
            this.crewMemberStatsDal = crewMemberStatsDal;
        }

        public Ship GetLoadout(string name)
        {
            var sl = shipLoadoutDal.Get(name);
            var ss = shipStatsDal.Get(sl.ShipName);
            var ship = new Ship()
            {
                Name = name,
                ShipSize = ss.ShipSize,
                CrewSize = ss.CrewSize,
                ShipDc = ss.ShipDc,
                ShipPilotingBonus = ss.ShipPilotingBonus,
                ShipQuality = ss.ShipQuality,
            };
            var crewNames = sl.CrewMembers.Select(x => x.Name.Replace("_", " ")).Distinct().ToList();
            ship.ShipsCrew = new ShipsCrew(crewNames.Select(x => crewMemberStatsDal.Get(x)).Select(cm => {
                return new CrewMember(cm.Name, cm.Title, cm.Skills)
                {
                    Jobs = sl.CrewMembers.Where(x => x.Name.Replace("_", " ") == cm.Name).Select(x => new Job
                    {
                        CrewName = cm.Name,
                        DutyType = x.DutyType,
                        IsAssistant = x.IsAssistant
                    }).ToList()
                };
           }).ToList());
            return ship;
        }
    }
}
