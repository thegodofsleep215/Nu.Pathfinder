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

        [TypedCommand("omgTest", "Rolls one day of the officer mini game.")]
        public string OmgRoll2(string crew, int crewMorale, int wellbeing, int sailingModifier, int navigateDc, int disciplineModifier, int healModifier, int weatherModifier)
        {
            var ship = LoadAssets2(crew);
            if (ship.CrewName != crew)
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
                HealModifier = healModifier,
                WeatherModifier = weatherModifier
            };
            var game = new OfficerEngine(ship, input);
            var result = game.Run();
            return string.Join(Environment.NewLine, result);
        }

        [TypedCommand("omgTest2", "Rolls one day of the officer mini game.")]
        public string OmgRoll2(string crew, string[] args) // Message passing system can't seem to handle array of strings. 
        {
            var errors = new List<string>();
            // TODO: Persist ship between rolls.
            var ship = LoadAssets2(crew);

            if (ship.CrewName != crew)
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover.

            // TODO - Voyage modifiers from args (all args optional, ei state is unchanged).
            var input = new DailyInput
            {
                CrewMorale = ship.ShipsMorale.MoraleBonus,
                Wellbeing = ship.ShipsMorale.WellBeing,
                SailingModifier = ship.CurrentVoyage.PilotingModifier,
                NavigateDc = ship.CurrentVoyage.NavigationDC,
                DisciplineModifier = 0, //
                HealModifier = ship.CurrentVoyage.DiseaseAboardShip ? 4 : 0,
                WeatherModifier = ship.CurrentVoyage.GetWeatherModifier(DutyType.Pilot) // TODO: One step at a time.
            };
            if (errors.Count == 0)
            {
                var game = new OfficerEngine(ship, input);
                var result = game.Run();
                var response = WriteAsset(ship);  // TODO - Could persistance be as simple as writing out the new ship state?
                if (!response.Success) 
                    result.AddRange(response.Messages);
                return string.Join(Environment.NewLine, result);
            }
            else
            {
                return string.Join(Environment.NewLine, errors);
            }
        }

        private void ProcessOMGArguments(string[] args, ref Ship ship, ref List<string> errors)
        {
            foreach(var arg in args)
            {
                string term = null;
                string value = null;
                if(arg.Contains(':'))
                {
                    var parts = arg.Split(':');
                    term = parts[0];
                    value = parts[1];
                }
                else
                {
                    term = arg;
                }

                // Argument to change crew morale factors (piracy, infamy, wellbeing, shipshape, wealth).
                // Argument to adjust available crew through 'CrewUnfitForDuty'.
                // Argument to adjust diseased crew through 'DiseasedCrew'.
                // Argument to change weather through 'WeatherConditions'.
                // Argument to adjust piloting conditions through 'NarrowPassage', 'ShallowWater', and 'NightConditions'.
                // Argument to adjust navigation conditions through 'OpenWater'.
                // Argument to add damage to the ship.
                // Argument to resupply.
                // Argument to refit.
                // Argument to add days at sea.  TODO: Add non-voyaging option?
                // TODO - Ship modifiers from args
                // Argument to change number of swabbies in crew.
                // Assign job?
                // Kill named crew member?
                // Load named crew member from file?
                // TODO - Command that only processes arguments and doesn't run minigame.
                switch (term.ToUpper())
                {
                    case "m":
                    case "morale":
                        // Argument to set temporary morale penalty through 'TemporaryMoralePenalty'.
                        if(value != null)
                        {
                            if(int.TryParse(value, out int temp))
                            {
                                ship.ShipsMorale.TemporaryMoralePenalty = temp;
                            }
                            else
                            {
                                errors.Add("'Morale' modifier value must be integer.  Use 'm:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Morale' modifier requires value.  Use 'm:#'.");
                        }
                    break:
                    case "d":
                    case "discipline":
                        // Argument to set temporary discipline modifier through 'DisciplineModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.DisciplineModifier = temp;
                            }
                            else
                            {
                                errors.Add("'Discipline' modifier value must be integer.  Use 'm:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Discipline' modifier requires value.  Use 'm:#'.");
                        }
                        break;
                }
            }
        }

        private Dictionary<string, Ship> LoadAssets2()
        {
            var folder = ".\\Ships";
            if (!Directory.Exists(folder))
            {
                return new Dictionary<string, Ship>();
            }
            var charFiles = Directory.GetFiles(folder, "*.json");
            return charFiles.Select(cf => JsonConvert.DeserializeObject<Ship>(File.ReadAllText(cf))).ToDictionary(x => x.CrewName, x => x);
        }

        private Ship LoadAssets2(string shipName)
        {
            var folder = ".\\Ships";
            if (!Directory.Exists(folder))
            {
                return new Ship();
            }
            string file = Directory.GetFiles(folder, string.Format("{0}.json", shipName)).First();
            return JsonConvert.DeserializeObject<Ship>(File.ReadAllText(file));
        }

        private BaseResponse WriteAsset(Ship ship, string filename = null)
        {
            BaseResponse retval = new BaseResponse();
            if (filename == null)
                filename = ship.CrewName;
            try
            {
                string contents = JsonConvert.SerializeObject(ship);
                var folder = ".\\Ships";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                File.WriteAllText(string.Format("{0}{1}.json", folder, filename), contents);
            }
            catch (Exception ex)
            {
                retval.Messages.Add(string.Format("ERROR: {0}", ex.Message));
            }

            return retval;
        }
    }
}
