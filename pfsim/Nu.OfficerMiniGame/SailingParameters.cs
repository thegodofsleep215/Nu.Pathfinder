using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class SailingParameters
    {
        public string VoyageName { get; set; }

        public bool NarrowPassage { get; set; }

        public bool ShallowWater { get; set; }

        public bool OpenOcean { get; set; }

        public NightStatus NightStatus { get; set; }

        public List<ShipInput> ShipInputs { get; set; }

        public static SailingParameters FromFleetState(string name, FleetState state)
        {
            var sp = new SailingParameters { VoyageName = name };
            sp.NarrowPassage = state.NarrowPassage;
            sp.ShallowWater = state.ShallowWater;
            sp.OpenOcean = state.OpenOcean;
            sp.NightStatus = state.NightStatus;
            sp.ShipInputs = state.ShipStates.Select(x => {
                return new ShipInput
                {
                    LoadoutName = x.Key,
                    DiseasedCrew = x.Value.DiseasedCrew,
                    Swabbies = x.Value.Swabbies,
                    CrewUnfitForDuty = x.Value.CrewUnfitForDuty,
                    DisciplineStandards = x.Value.DisciplineStandards,
                    DisciplineModifier = x.Value.TemporaryDisciplineModifier,
                    CommandModifier = x.Value.TemporaryCommandModifier,
                };
            }).ToList();
            return sp;
        }

    }


}
