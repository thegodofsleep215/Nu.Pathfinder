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

        [TypedCommand("omg", "Rolls one day of the officer mini game (Crew).")]
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
            return string.Join(Environment.NewLine, result.Messages);
        }

        private Dictionary<string, Ship> LoadAssets()
        {
            var folder = ".\\Crews";
            if (!Directory.Exists(folder))
            {
                return new Dictionary<string, Ship>();
            }
            var charFiles = Directory.GetFiles(folder, "*.json");
            return charFiles.Select(cf => JsonConvert.DeserializeObject<Ship>(File.ReadAllText(cf))).ToDictionary(x => x.CrewName, x => x);
        }

        [TypedCommand("omgTest", "Rolls one day of the officer mini game (Ship).")]
        public string OmgRoll2(string crew, int crewMorale, int wellbeing, int sailingModifier, int navigateDc, int disciplineModifier, int healModifier, int weatherModifier, int commandModifier)
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
                CommandModifier = commandModifier,
                DisciplineModifier = disciplineModifier,
                HealModifier = healModifier,
                WeatherModifier = weatherModifier
            };
            var game = new OfficerEngine(ship, input);
            var result = game.Run();
            return string.Join(Environment.NewLine, result.Messages);
        }

        [TypedCommand("Sail", "Rolls one day of the officer mini game.")]
        public string Sail(string crew) // Message passing system can't seem to handle array of strings. 
        {
            return Sail(crew, string.Empty);
        }

        [TypedCommand("Sail", "Rolls one day of the officer mini game.")]
        public string Sail(string crew, string term) // Message passing system can't seem to handle array of strings. 
        {
            var args = term.Split(','); // TODO: Until we can handle this better.
            var result = new List<string>();
            var ship = LoadShip(crew);

            if (ship.CrewName != crew)
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover from bad command.

            // Voyage modifiers from args (all args optional, ei state is unchanged).
            var pResponse = ProcessOMGArguments(args, ref ship, ref result);
            var input = new DailyInput  // TODO: Can we do without the daily input now?
            {
                CrewMorale = ship.ShipsMorale.MoraleBonus,
                Wellbeing = ship.ShipsMorale.WellBeing,
                CommandModifier = ship.CurrentVoyage.CommandModifier,
                SailingModifier = ship.CurrentVoyage.PilotingModifier,
                NavigateDc = ship.CurrentVoyage.NavigationDC,
                DisciplineModifier = ship.CrewDisciplineModifier, //
                HealModifier = ship.CurrentVoyage.DiseaseAboardShip ? 4 : 0,
                WeatherModifier = ship.CurrentVoyage.GetWeatherModifier(DutyType.Pilot) // TODO: One step at a time.
            };
            if (pResponse.Success)
            {
                var game = new OfficerEngine(ship, input);
                var gResponse = game.Run(); // Distinguish between successful and unsuccessful game.
                
                if (gResponse.Success)
                {
                    var sResponse = WriteAsset(ship);
                    if (!sResponse.Success)
                        result.AddRange(sResponse.Messages);
                }
                else
                {
                    gResponse.Messages.Insert(0, "Game not run because of the following errors in ship configuration:");
                }

                result.AddRange(gResponse.Messages);
                return string.Join(Environment.NewLine, result);
            }
            else
            {
                pResponse.Messages.Insert(0, "Game not run because of errors in parsing the parameters.");
                return string.Join(Environment.NewLine, pResponse.Messages);
            }
        }

        [TypedCommand("AdjustShip", "Sets new parameters on the named ship, but does not advance the officer minigame.")]
        public string AdjustShip(string crew, string term) // Message passing system can't seem to handle array of strings. 
        {
            var args = term.Split(','); // TODO: Until we can handle this better.
            var result = new List<string>();

            var ship = LoadShip(crew);

            if (ship.CrewName != crew)
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover.

            // Voyage modifiers from args (all args optional, ei state is unchanged).
            var pResponse = ProcessOMGArguments(args, ref ship, ref result);

            if (pResponse.Success)
            {
                var sResponse = WriteAsset(ship);
                if (!sResponse.Success)
                    result.AddRange(sResponse.Messages);
                return string.Join(Environment.NewLine, result);
            }

            return string.Join(Environment.NewLine, pResponse.Messages);
        }

        private BaseResponse ProcessOMGArguments(string[] args, ref Ship ship, ref List<string> messages)
        {
            BaseResponse retval = new BaseResponse();

            foreach(var arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                    continue;

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
                // TODO: Add non-voyaging option?
                // TODO - Ship modifiers from args
                // Adjust ship discpline level
                // Argument to change number of swabbies in crew.
                // Assign job?
                // Remove job?
                // Kill named crew member?
                // Load named crew member from file?
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
                                retval.Messages.Add("'Morale' modifier value must be integer.  Use 'm:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Morale' modifier requires value.  Use 'm:#'.");
                        }
                        break;
                    case "M+":
                    case "MORALE+":
                        // Argument to set temporary morale modifier through 'TemporaryMoralePenalty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.ShipsMorale.TemporaryMoralePenalty += temp;
                                messages.Add(string.Format("Added {1} to temporary morale penalty. New value is {1}. (Positive is bad.)", temp, ship.CurrentVoyage.CommandModifier));
                            }
                            else
                            {
                                retval.Messages.Add("Adding to temporary morale penalty modifier requires value. Use 'm+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Adding to temporary morale penalty modifier requires value.  Use 'm+:#'.");
                        }
                        break;
                    case "M-":
                    case "MORALE-":
                        // Argument to set temporary morale modifier through 'TemporaryMoralePenalty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (ship.ShipsMorale.TemporaryMoralePenalty >= temp)
                                    ship.ShipsMorale.TemporaryMoralePenalty = 0;
                                else
                                    ship.ShipsMorale.TemporaryMoralePenalty -= temp;
                                messages.Add(string.Format("Removed {1} from temporary morale penalty. New value is {1}. (Positive is bad.)", temp, ship.ShipsMorale.TemporaryMoralePenalty));
                            }
                            else
                            {
                                retval.Messages.Add("Removing temporary morale penalty value must be integer.  Use 'm-:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Removing temporary morale penalty requires value.  Use 'm-:#'.");
                        }
                        break;
                    case "D":
                    case "DISCPLINE":
                        // Argument to set temporary discipline modifier through 'DisciplineModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Discipline penalty changed to {0}. (Positive is bad.)", temp));
                                ship.CurrentVoyage.DisciplineModifier = temp;
                            }
                            else
                            {
                                retval.Messages.Add("'Discipline' modifier value must be integer.  Use 'd:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Discipline' modifier requires value.  Use 'd:#'.");
                        }
                        break;
                    case "D+":
                    case "DISCPLINE+":
                        // Argument to set temporary discipline modifier through 'DisciplineModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.DisciplineModifier += temp;
                                messages.Add(string.Format("Added {1} to temporary discipline penalty. New value is {1}. (Positive is bad.)", temp, ship.CurrentVoyage.DisciplineModifier));
                            }
                            else
                            {
                                retval.Messages.Add("Adding to discipline modifier requires positive integer. Use 'd+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Adding to discipline modifier requires value.  Use 'd+:#'.");
                        }
                        break;
                    case "D-":
                    case "DISCPLINE-":
                        // Argument to set temporary discipline modifier through 'DisciplineModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (ship.CurrentVoyage.DisciplineModifier >= temp)
                                    ship.CurrentVoyage.DisciplineModifier = 0;
                                else
                                    ship.CurrentVoyage.DisciplineModifier -= temp;
                                messages.Add(string.Format("Removed {1} from temporary discipline penalty. New value is {1}. (Positive is bad.)", temp, ship.CurrentVoyage.DisciplineModifier));
                            }
                            else
                            {
                                retval.Messages.Add("Removing discipline modifier value must be integer.  Use 'd-:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Removing discipline modifier requires value.  Use 'd-:#'.");
                        }
                        break;
                    case "C":
                    case "COMMAND":
                        // Argument to set temporary command modifier through 'CommandModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Command penalty changed to {0}. (Positive is bad.)", temp));
                                ship.CurrentVoyage.CommandModifier = temp;
                            }
                            else
                            {
                                retval.Messages.Add("'Command' modifier value must be integer.  Use 'c:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Command' modifier requires value.  Use 'c:#'.");
                        }
                        break;
                    case "C+":
                    case "COMMAND+":
                        // Argument to set temporary command modifier through 'CommandModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.CommandModifier += temp;
                                messages.Add(string.Format("Added {1} to temporary command penalty. New value is {1}. (Positive is bad.)", temp, ship.CurrentVoyage.CommandModifier));
                            }
                            else
                            {
                                retval.Messages.Add("Adding to 'command' modifier requires value. Use 'c+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Adding to 'command' modifier requires value.  Use 'c+:#'.");
                        }
                        break;
                    case "C-":
                    case "COMMAND-":
                        // Argument to set temporary command modifier through 'CommandModifier'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (ship.CurrentVoyage.CommandModifier >= temp)
                                    ship.CurrentVoyage.CommandModifier = 0;
                                else
                                    ship.CurrentVoyage.CommandModifier -= temp;
                                messages.Add(string.Format("Removed {1} from temporary command penalty. New value is {1}. (Positive is bad.)", temp, ship.CurrentVoyage.CommandModifier));
                            }
                            else
                            {
                                retval.Messages.Add("Removing 'command' modifier value must be integer.  Use 'c-:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Removing 'command' modifier requires value.  Use 'c-:#'.");
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
                                retval.Messages.Add("'Piracy' value must be integer.  Use 'p:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Piracy' value requires value.  Use 'p:#'.");
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
                                retval.Messages.Add("'Wellbeing' value must be integer.  Use 'w:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Wellbeing' value requires value.  Use 'w:#'.");
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
                                retval.Messages.Add("'Wellbeing' modifier must be integer.  Use 'wp:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Wellbeing' modifier requires value.  Use 'wp:#'.");
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
                                retval.Messages.Add("'Wealth' value must be integer.  Use 'wt:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Wealth' value requires value.  Use 'wt:#'.");
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
                                retval.Messages.Add("'Infamy' value must be integer.  Use 'i:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Infamy' value requires value.  Use 'i:#'.");
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
                                retval.Messages.Add("'ShipShape' value must be integer.  Use 'ss:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'ShipShape' value requires value.  Use 'ss:#'.");
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
                                retval.Messages.Add("'Unfit' crew value must be integer.  Use 'u:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Unfit' crew requires value.  Use 'u:#'.");
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
                                retval.Messages.Add("'Unfit' crew modifier must be integer.  Use 'u:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Unfit' crew modifier requires value.  Use 'u:#'.");
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
                                retval.Messages.Add("'Unfit' crew modifier must be integer.  Use 'u:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Unfit' crew modifier requires value.  Use 'u:#'.");
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
                                retval.Messages.Add("'Diseased' crew value must be integer.  Use 'ds:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Diseased' crew requires value.  Use 'ds:#'.");
                        }
                        break;
                    case "DS+":
                    case "DISEASE+":
                    case "DISEASED+":
                        // Argument to adjust available crew through 'DiseasedCrew'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.DiseasedCrew += temp;
                                messages.Add(string.Format("Added {0} crew to sick bay.  Value changed to {1}.", temp, ship.CurrentVoyage.DiseasedCrew));
                            }
                            else
                            {
                                retval.Messages.Add("Add 'Diseased' crew value must be integer.  Use 'ds+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Add 'Diseased' crew requires value.  Use 'ds+:#'.");
                        }
                        break;
                    case "DS-":
                    case "DISEASE-":
                    case "DISEASED-":
                        // Argument to adjust available crew through 'DiseasedCrew'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp >= ship.CurrentVoyage.DiseasedCrew)
                                    ship.CurrentVoyage.DiseasedCrew = 0;
                                else
                                    ship.CurrentVoyage.DiseasedCrew -= temp;
                                messages.Add(string.Format("Removed {0} crew from sick bay.  Value changed to {1}.", temp, ship.CurrentVoyage.DiseasedCrew));
                            }
                            else
                            {
                                retval.Messages.Add("Remove 'Diseased' crew value must be integer.  Use 'ds+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Remove 'Diseased' crew requires value.  Use 'ds+:#'.");
                        }
                        break;
                    case "WE":
                    case "WEATHER":
                        // Argument to change weather through 'WeatherConditions'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.CurrentVoyage.WeatherConditions = (WeatherConditions)temp;  // TODO: Make this safe.
                            }
                            else if (Enum.TryParse<WeatherConditions>(value, true, out WeatherConditions result))
                            {
                                ship.CurrentVoyage.WeatherConditions = result;
                            }
                            else
                            {
                                retval.Messages.Add("Unrecognized weather condition. Use 'wt:#' or spell out condition such as 'wt:calm'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Weather' requires value.  Use 'wt:#' or spell out condition such as 'wt:calm'.");
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
                                    retval.Messages.Add("Unrecognized value for 'NarrowPassage'.  Use np+ or np-.");
                                    break;
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Got 'NarrowPassage' parameter, but not value.  Use np+ or np-.");
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
                                    retval.Messages.Add("Unrecognized value for 'ShallowWater'.  Use sw+ or sw-.");
                                    break;
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Got 'ShallowWater' parameter, but not value.  Use sw+ or sw-.");
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
                                    retval.Messages.Add("Unrecognized value for 'OpenOcean'.  Use o+ or o-.");
                                    break;
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Got 'OpenOcean' parameter, but not value.  Use o+ or o-.");
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
                                retval.Messages.Add("Unrecognized night status. Use 'NS:#' or spell out condition such as 'NS:Underweigh'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'NightStatus' requires value.  Use 'NS:#' or spell out condition such as 'NS:Underweigh'.");
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
                                if(temp > 0)
                                    ship.CurrentVoyage.AlterHullDamage(temp);
                                else
                                    retval.Messages.Add("Add 'HullDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                            else
                            {
                                retval.Messages.Add("Add 'HullDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Add 'Diseased' crew requires value.  Use 'dm+:#'.");
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
                                if (temp > 0)
                                    ship.CurrentVoyage.AlterHullDamage(temp);
                                else
                                    retval.Messages.Add("Remove 'HullDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                            else
                            {
                                retval.Messages.Add("Remove 'HullDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Remove 'HullDamage' requires value.  Use 'dm-:#'.");
                        }
                        break;
                    case "S+":
                    case "SAIL+":
                        // Arguement to add damage to the Sail.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp > 0)
                                {
                                    ship.CurrentVoyage.AlterSailDamage(temp);
                                    messages.Add(string.Format("Added {0} damage to sails.", temp));
                                }
                                else
                                    retval.Messages.Add("Add 'SailDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                            else
                            {
                                retval.Messages.Add("Add 'SailDamage' value must be a positive integer.  Use 'dm+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Add 'SailDamage' value requires value.  Use 'dm+:#'.");
                        }
                        break;
                    case "S-":
                    case "SAIL-":
                        // Arguement to remove damage from the Sail.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp > 0)
                                {
                                    ship.CurrentVoyage.AlterSailDamage(temp);
                                    messages.Add(string.Format("Removed {0} damage from sails.", temp));
                                }
                                else
                                    retval.Messages.Add("Remove 'SailDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                            else
                            {
                                retval.Messages.Add("Remove 'SailDamage' value must be a positive integer.  Use 'dm-:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Remove 'SailDamage' requires value.  Use 'dm-:#'.");
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
                    case "T+":
                    case "TIME+":
                        // Argument to add days to the journey.
                        if (value != null)
                        {
                            if(int.TryParse(value, out int temp))
                            {
                                if (temp > 0)
                                {
                                    ship.CurrentVoyage.AddDaysOfVoyage(temp);
;                                   messages.Add(string.Format("Extended voyage by {0} days.", temp));
                                }
                                else
                                    retval.Messages.Add("Adding days to voyage requires positive integer.  Use 't+:#'.");
                            }
                            else
                            {
                                retval.Messages.Add("Adding days to voyage requires positive integer.  Use 't+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Adding days to voyage requires value.  Use 't+:#'.");
                        }
                        break;
                    default:
                        if(value != null)
                        {
                            retval.Messages.Add(string.Format("Unrecognized argument '{0}:{1}'", term, value));
                        }
                        else
                        {
                            retval.Messages.Add(string.Format("Unrecognized argument '{0}'", term));
                        }
                        break;
                }
            }

            if (retval.Messages.Count == 0)
                retval.Success = true;

            return retval;
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
                retval.Success = true;
            }
            catch (Exception ex)
            {
                retval.Messages.Add(string.Format("ERROR: {0}", ex.Message));
            }

            return retval;
        }
    }
}
