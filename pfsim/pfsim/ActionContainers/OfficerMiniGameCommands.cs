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
            var ship = LoadShip(crew);
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

        [TypedCommand("Sail", "Rolls one day of the officer mini game.")]
        public string Sail(string crew, string term) // Message passing system can't seem to handle array of strings. 
        {
            var args = term.Split(','); // TODO: Until we can handle this better.
            var errors = new List<string>();
            var result = new List<string>();
            // TODO: Persist ship between rolls.
            var ship = LoadShip(crew);

            if (ship.CrewName != crew)
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover.

            // Voyage modifiers from args (all args optional, ei state is unchanged).
            result.AddRange(ProcessOMGArguments(args, ref ship, ref errors));
            var input = new DailyInput
            {
                CrewMorale = ship.ShipsMorale.MoraleBonus,
                Wellbeing = ship.ShipsMorale.WellBeing,
                SailingModifier = ship.CurrentVoyage.PilotingModifier,
                NavigateDc = ship.CurrentVoyage.NavigationDC,
                DisciplineModifier = ship.CurrentVoyage.DisciplineModifier, //
                HealModifier = ship.CurrentVoyage.DiseaseAboardShip ? 4 : 0,
                WeatherModifier = ship.CurrentVoyage.GetWeatherModifier(DutyType.Pilot) // TODO: One step at a time.
            };
            if (errors.Count == 0)
            {
                var game = new OfficerEngine(ship, input);
                result.AddRange(game.Run());
                var response = WriteAsset(ship);  
                if (!response.Success) 
                    result.AddRange(response.Messages);
                return string.Join(Environment.NewLine, result);
            }
            else
            {
                return string.Join(Environment.NewLine, errors);
            }
        }

        [TypedCommand("AdjustShip", "Sets new parameters on the named ship, but does not advance the officer minigame.")]
        public string AdjustShip(string crew, string term) // Message passing system can't seem to handle array of strings. 
        {
            var args = term.Split(','); // TODO: Until we can handle this better.
            var errors = new List<string>();
            var result = new List<string>();

            var ship = LoadShip(crew);

            if (ship.CrewName != crew)
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover.

            // Voyage modifiers from args (all args optional, ei state is unchanged).
            result.AddRange(ProcessOMGArguments(args, ref ship, ref errors));

            if (errors.Count == 0)
            {
                var response = WriteAsset(ship);
                if (!response.Success)
                    errors.AddRange(response.Messages);
                return string.Join(Environment.NewLine, result);
            }

            return string.Join(Environment.NewLine, errors);
        }

        private List<string> ProcessOMGArguments(string[] args, ref Ship ship, ref List<string> errors)
        {
            List<string> messages = new List<string>();
            foreach(var arg in args)
            {
                string term = null;
                string value = null;
                if(arg.Contains(':'))
                {
                    var parts = arg.Split(':');
                    term = parts[0].Trim();
                    value = parts[1].Trim();
                }
                else
                {
                    term = arg;
                }
                
                // Argument to refit.
                // Argument to add days at sea.  TODO: Add non-voyaging option?
                // TODO - Ship modifiers from args
                // Argument to change number of swabbies in crew.
                // Assign job?
                // Remove job?
                // Kill named crew member?
                // Load named crew member from file?
                // TODO - Command that only processes arguments and doesn't run minigame.
                switch (term.ToUpper())
                {
                    case "M":
                    case "MORALE":
                        // Argument to set temporary morale penalty through 'TemporaryMoralePenalty'.
                        if(value != null)
                        {
                            if(int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Overall morale penalty changed to {0}. (Positive is bad.)", temp));
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
                        break;
                    case "D":
                    case "DISCPLINE":
                        // Argument to set temporary discipline modifier through 'DisciplineModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Disciplined penalty changed to {0}. (Positive is bad.)", temp));
                                ship.CurrentVoyage.DisciplineModifier = temp;
                            }
                            else
                            {
                                errors.Add("'Discipline' modifier value must be integer.  Use 'd:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Discipline' modifier requires value.  Use 'd:#'.");
                        }
                        break;
                    case "P":
                    case "PIRACY":
                        // Argument to change crew morale factors - 'Piracy'
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Piracy changed to {0}.", temp));
                                ship.ShipsMorale.Piracy = temp;
                            }
                            else
                            {
                                errors.Add("'Piracy' value must be integer.  Use 'p:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Piracy' value requires value.  Use 'p:#'.");
                        }
                        break;
                    case "W":
                    case "WELL":
                    case "WELLBEING":
                        // Argument to change crew morale factors - 'Wellbeing'
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Wellbeing changed to {0}.", temp));
                                ship.ShipsMorale.WellBeing = temp;
                            }
                            else
                            {
                                errors.Add("'Wellbeing' value must be integer.  Use 'w:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Wellbeing' value requires value.  Use 'w:#'.");
                        }
                        break;
                    case "WP":
                    case "WELLPEN":
                    case "WELLPENALTY":
                        // Argument to change temporary 'Wellbeing' penalty
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Wellbeing penalty changed to {0}.", temp));
                                ship.ShipsMorale.WellBeing = temp;
                            }
                            else
                            {
                                errors.Add("'Wellbeing' modifier must be integer.  Use 'wp:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Wellbeing' modifier requires value.  Use 'wp:#'.");
                        }
                        break;
                    case "WT":
                    case "WEAL":
                    case "WEALTH":
                        // Argument to change crew morale factors - 'Wealth'
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Wealth changed to {0}.", temp));
                                ship.ShipsMorale.Wealth = temp;
                            }
                            else
                            {
                                errors.Add("'Wealth' value must be integer.  Use 'wt:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Wealth' value requires value.  Use 'wt:#'.");
                        }
                        break;
                    case "I":
                    case "INFAMY":
                        // Argument to change crew morale factors - 'Infamy'
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Infamy changed to {0}.", temp));
                                ship.ShipsMorale.Wealth = temp;
                            }
                            else
                            {
                                errors.Add("'Infamy' value must be integer.  Use 'i:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Infamy' value requires value.  Use 'i:#'.");
                        }
                        break;
                    case "SS":
                    case "SHIPSHAPE":
                        // Argument to change crew morale factors - 'Shipshape'
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Shipshape changed to {0}.", temp));
                                ship.ShipsMorale.ShipShape = temp;
                            }
                            else
                            {
                                errors.Add("'ShipShape' value must be integer.  Use 'ss:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'ShipShape' value requires value.  Use 'ss:#'.");
                        }
                        break;
                    case "U":
                    case "UNFIT":
                        // Argument to adjust available crew through 'CrewUnfitForDuty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Crew unfit for duty changed to {0}.", temp));
                                ship.CurrentVoyage.CrewUnfitForDuty = temp;
                            }
                            else
                            {
                                errors.Add("'Unfit' crew value must be integer.  Use 'u:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Unfit' crew requires value.  Use 'u:#'.");
                        }
                        break;
                    case "U+":
                    case "UNFIT+":
                        // Argument to adjust available crew through 'CrewUnfitForDuty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.CrewUnfitForDuty += temp;
                                messages.Add(string.Format("Added {0} crew unfit for duty.  Value changed to {1}.", temp, ship.CurrentVoyage.CrewUnfitForDuty));
                            }
                            else
                            {
                                errors.Add("'Unfit' crew modifier must be integer.  Use 'u:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Unfit' crew modifier requires value.  Use 'u:#'.");
                        }
                        break;
                    case "U-":
                    case "UNFIT-":
                        // Argument to adjust available crew through 'CrewUnfitForDuty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.CrewUnfitForDuty -= temp;
                                messages.Add(string.Format("Removed {0} crew from unfit for duty roster.  Value changed to {1}.", temp, ship.CurrentVoyage.CrewUnfitForDuty));
                            }
                            else
                            {
                                errors.Add("'Unfit' crew modifier must be integer.  Use 'u:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Unfit' crew modifier requires value.  Use 'u:#'.");
                        }
                        break;
                    case "DS":
                    case "DISEASE":
                    case "DISEASED":
                        // Argument to adjust available crew through 'CrewUnfitForDuty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Crew in sick bay changed from {0} to {1}.", ship.CurrentVoyage.DiseasedCrew, temp));
                                ship.CurrentVoyage.DiseasedCrew = temp;
                            }
                            else
                            {
                                errors.Add("'Diseased' crew value must be integer.  Use 'ds:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Diseased' crew requires value.  Use 'ds:#'.");
                        }
                        break;
                    case "DS+":
                    case "DISEASE+":
                    case "DISEASED+":
                        // Argument to adjust available crew through 'CrewUnfitForDuty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.DiseasedCrew += temp;
                            }
                            else
                            {
                                errors.Add("Add 'Diseased' crew value must be integer.  Use 'ds+:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Add 'Diseased' crew requires value.  Use 'ds+:#'.");
                        }
                        break;
                    case "DS-":
                    case "DISEASE-":
                    case "DISEASED-":
                        // Argument to adjust available crew through 'CrewUnfitForDuty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp >= ship.CurrentVoyage.DiseasedCrew)
                                    ship.CurrentVoyage.DiseasedCrew = 0;
                                else
                                    ship.CurrentVoyage.DiseasedCrew -= temp;
                            }
                            else
                            {
                                errors.Add("Remove 'Diseased' crew value must be integer.  Use 'ds+:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Remove 'Diseased' crew requires value.  Use 'ds+:#'.");
                        }
                        break;
                    case "WE":
                    case "WEATHER":
                        // Argument to change weather through 'WeatherConditions'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.WeatherConditions = (WeatherConditions)temp;
                            }
                            else if (Enum.TryParse<WeatherConditions>(value, true, out WeatherConditions result))
                            {
                                ship.CurrentVoyage.WeatherConditions = result;
                            }
                            else
                            {
                                errors.Add("Unrecognized weather condition. Use 'wt:#' or spell out condition such as 'wt:calm'.");
                            }
                        }
                        else
                        {
                            errors.Add("'Weather' requires value.  Use 'wt:#' or spell out condition such as 'wt:calm'.");
                        }
                        break;
                    case "NP":
                    case "NARROW":
                    case "NARROWPASS":
                    case "NARROWPASSAGE":
                        // Argument to adjust piloting conditions through 'NarrowPassage'.
                        if (value != null)
                        {
                            switch(value)
                            {
                                case "-":
                                    ship.CurrentVoyage.NarrowPassage = false;
                                    break;
                                case "+":
                                    ship.CurrentVoyage.NarrowPassage = true;
                                    break;
                                default:
                                    errors.Add("Unrecognized value for 'NarrowPassage'.  Use np+ or np-.");
                                    break;
                            }
                        }
                        else
                        {
                            errors.Add("Got 'NarrowPassage' parameter, but not value.  Use np+ or np-.");
                        }
                        break;
                    case "NP+":
                    case "NARROW+":
                    case "NARROWPASS+":
                    case "NARROWPASSAGE+":
                        ship.CurrentVoyage.NarrowPassage = true;
                        break;
                    case "NP-":
                    case "NARROW-":
                    case "NARROWPASS-":
                    case "NARROWPASSAGE-":
                        ship.CurrentVoyage.NarrowPassage = false;
                        break;
                    case "SW":
                    case "SHALLOW":
                    case "SHALLOWWATER":
                        // Argument to adjust piloting conditions through 'ShallowWater'.
                        if (value != null)
                        {
                            switch (value)
                            {
                                case "-":
                                    ship.CurrentVoyage.ShallowWater = false;
                                    break;
                                case "+":
                                    ship.CurrentVoyage.ShallowWater = true;
                                    break;
                                default:
                                    errors.Add("Unrecognized value for 'ShallowWater'.  Use sw+ or sw-.");
                                    break;
                            }
                        }
                        else
                        {
                            errors.Add("Got 'ShallowWater' parameter, but not value.  Use sw+ or sw-.");
                        }
                        break;
                    case "SW+":
                    case "SHALLOW+":
                    case "SHALLOWWATER+":
                        ship.CurrentVoyage.ShallowWater = true;
                        break;
                    case "SW-":
                    case "SHALLOW-":
                    case "SHALLOWWATER-":
                        ship.CurrentVoyage.ShallowWater = false;
                        break;
                    case "O":
                    case "OPEN":
                    case "OPENOCEAN":
                        // Argument to adjust navigation conditions through 'OpenWater'.
                        if (value != null)
                        {
                            switch (value)
                            {
                                case "-":
                                    ship.CurrentVoyage.OpenOcean = false;
                                    break;
                                case "+":
                                    ship.CurrentVoyage.OpenOcean = true;
                                    break;
                                default:
                                    errors.Add("Unrecognized value for 'OpenOcean'.  Use o+ or o-.");
                                    break;
                            }
                        }
                        else
                        {
                            errors.Add("Got 'OpenOcean' parameter, but not value.  Use o+ or o-.");
                        }
                        break;
                    case "O+":
                    case "OPEN+":
                    case "OPENOCEAN+":
                        ship.CurrentVoyage.OpenOcean = true;
                        break;
                    case "O-":
                    case "OPEN-":
                    case "OPENOCEAN-":
                        ship.CurrentVoyage.OpenOcean = false;
                        break;
                    case "NS":
                    case "NIGHT":
                    case "NIGHTSTATUS":
                        // Argument to adjust piloting conditions through 'NightStatus'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.NightStatus = (NightStatus)temp;
                            }
                            else if (Enum.TryParse<NightStatus>(value, true, out NightStatus result))
                            {
                                ship.CurrentVoyage.NightStatus = result;
                            }
                            else
                            {
                                errors.Add("Unrecognized night status. Use 'NS:#' or spell out condition such as 'NS:Underweigh'.");
                            }
                        }
                        else
                        {
                            errors.Add("'NightStatus' requires value.  Use 'NS:#' or spell out condition such as 'NS:Underweigh'.");
                        }
                        break;
                    case "DM+":
                    case "DAMAGE+":
                    case "HULL+":
                        // Arguement to add damage to the hull.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if(temp < 0)
                                    ship.CurrentVoyage.AlterHullDamage(temp);
                                else
                                    errors.Add("Add 'HullDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                            else
                            {
                                errors.Add("Add 'HullDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Add 'Diseased' crew requires value.  Use 'dm+:#'.");
                        }
                        break;
                    case "DM-":
                    case "DAMAGE-":
                    case "HULL-":
                        // Arguement to remove damage from the hull.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp < 0)
                                    ship.CurrentVoyage.AlterHullDamage(temp);
                                else
                                    errors.Add("Remove 'HullDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                            else
                            {
                                errors.Add("Remove 'HullDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Remove 'HullDamage' requires value.  Use 'dm-:#'.");
                        }
                        break;
                    case "S+":
                    case "SAIL+":
                        // Arguement to add damage to the Sail.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp < 0)
                                    ship.CurrentVoyage.AlterSailDamage(temp);
                                else
                                    errors.Add("Add 'SailDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                            else
                            {
                                errors.Add("Add 'SailDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Add 'SailDamage' value requires value.  Use 'dm+:#'.");
                        }
                        break;
                    case "S-":
                    case "SAIL-":
                        // Arguement to remove damage from the Sail.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp < 0)
                                    ship.CurrentVoyage.AlterSailDamage(temp);
                                else
                                    errors.Add("Remove 'SailDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                            else
                            {
                                errors.Add("Remove 'SailDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                        }
                        else
                        {
                            errors.Add("Remove 'SailDamage' requires value.  Use 'dm-:#'.");
                        }
                        break;
                    case "HARBOR":
                    case "PORT":
                    case "RESUPPLY":
                    case "RS":
                        messages.Add("Ship has made landfall and taken on supplies.");
                        ship.CurrentVoyage.DaysSinceResupply = 0;
                        ship.CurrentVoyage.VariedFoodSupplies = true;
                        break;
                    default:
                        if(value != null)
                        {
                            errors.Add(string.Format("Unrecognized argument '{0}:{1}'", term, value));
                        }
                        else
                        {
                            errors.Add(string.Format("Unrecognized argument '{0}'", term));
                        }
                        break;
                }
            }

            return messages;
        }

        private Dictionary<string, Ship> LoadShips()
        {
            var folder = ".\\Ships";
            if (!Directory.Exists(folder))
            {
                return new Dictionary<string, Ship>();
            }
            var charFiles = Directory.GetFiles(folder, "*.json");
            return charFiles.Select(cf => JsonConvert.DeserializeObject<Ship>(File.ReadAllText(cf))).ToDictionary(x => x.CrewName, x => x);
        }

        private Ship LoadShip(string shipName)
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
                string contents = JsonConvert.SerializeObject(ship, Formatting.Indented);
                var folder = ".\\Ships";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                File.WriteAllText(string.Format("{0}\\{1}.json", folder, filename), contents);
            }
            catch (Exception ex)
            {
                retval.Messages.Add(string.Format("ERROR: {0}", ex.Message));
            }

            return retval;
        }
    }
}
