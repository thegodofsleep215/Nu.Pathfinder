using Newtonsoft.Json;
using Nu.CommandLine.Attributes;
using Nu.Messaging;
using pfsim.Officer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pfsim.ActionContainers
{
    public class OfficerMiniGameCommands
    {
        public OfficerMiniGameCommands()
        {
        }

        [TypedCommand("omg", "Rolls one day of the officer mini game.")]
        public string OmgRoll(string crew, int crewMorale, int wellbeing, int sailingModifier, int navigateDc, int disciplineModifier, int healModifier)
        {
            var crews = LoadAssets();
            if (!crews.ContainsKey(crew))
            {
                return "Crew not found.";
            }
            var input = new DailyInput
            {
                CrewMorale = crewMorale,
                Wellbeing = wellbeing,
                SailingModifier = sailingModifier,
                NavigateDc = navigateDc,
                DisciplineModifier = disciplineModifier,
                HealModifier = healModifier
            };
            var game = new OfficerEngine(crews[crew], input);
            var result = game.Run();
            return string.Join(Environment.NewLine, result);
        }

        private Dictionary<string, Crew> LoadAssets()
        {
            var folder = ".\\Crews";
            if (!Directory.Exists(folder))
            {
                return new Dictionary<string, Crew>();
            }
            var charFiles = Directory.GetFiles(folder, "*.json");
            return charFiles.Select(cf => JsonConvert.DeserializeObject<Crew>(File.ReadAllText(cf))).ToDictionary(x => x.CrewName, x => x);
        }

    }
}
