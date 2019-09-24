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

        [TypedCommand("Sail", "Rolls one day of the officer mini game.")]
        public string Sail(string crew)
        {
            return Sail(crew, new List<string>());
        }

        [TypedCommand("Sail", "Rolls one day of the officer mini game.")]
        public string Sail(string crew, List<string> args) // usage> Sail Dogfish [put, your, args, in, "like this"]
        {
            var result = new List<string>();
            var ship = LoadShip(crew);

            if (string.IsNullOrEmpty(ship.CrewName))
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover from bad command.

            // Voyage modifiers from args (all args optional, ei state is unchanged).
            var pResponse = ProcessOMGArguments(args.ToArray(), ref ship, ref result);

            if (pResponse.Success)
            {
                var game = new SailingEngine(ship);
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
        public string AdjustShip(string crew, List<string> args) // Uses a list formatted like > AdjustShip Dogfish [comma, seprated, "list"]
        {
            var result = new List<string>();

            var ship = LoadShip(crew);

            if (string.IsNullOrEmpty(ship.CrewName))
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover.

            // Voyage modifiers from args (all args optional, ei state is unchanged).
            var pResponse = ProcessOMGArguments(args.ToArray(), ref ship, ref result);

            if (pResponse.Success)
            {
                var sResponse = WriteAsset(ship);
                if (!sResponse.Success)
                    result.AddRange(sResponse.Messages);
                return string.Join(Environment.NewLine, result);
            }

            return string.Join(Environment.NewLine, pResponse.Messages);
        }

        [TypedCommand("Rest", "Rolls one day of the officer mini game, but doesn't sail the boat anywhere.")]
        public string Anchor(string crew)
        {
            return Anchor(crew, new List<string>());
        }

        [TypedCommand("Rest", "Rolls one day of the officer mini game, but doesn't sail the boat anywhere.")]
        public string Anchor(string crew, List<string> args)
        {
            var result = new List<string>();
            var ship = LoadShip(crew);

            if (string.IsNullOrEmpty(ship.CrewName))
            {
                return "Crew not found.";
            }
            WriteAsset(ship, string.Format("{0}.old", ship.CrewName)); // Store the current ship in case need to recover from bad command.

            // Voyage modifiers from args (all args optional, ei state is unchanged).
            var pResponse = ProcessOMGArguments(args.ToArray(), ref ship, ref result);

            if (pResponse.Success)
            {
                var game = new AnchoredEngine(ship);
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

        [TypedCommand("Plunder", "Testing: Makes some example plunder.")]
        public string Plunder()
        {
            return Plunder(DiceRoller.D6(3));
        }

        [TypedCommand("Plunder", "Testing: Makes some example plunder.")]
        public string Plunder(int amount)
        {
            List<Plunder> plunder = new List<Plunder>();

            for (int i = 0; i < amount; i++)
            {
                plunder.Add((Plunder)CargoFactory.Instance.ProduceCargo(CargoType.Plunder));
            }

            var response = WriteAsset(plunder.ToArray());

            List<string> retval = new List<string>();
            retval.AddRange(response.Messages);
            retval.AddRange(plunder.Select(a => a.ToString()).ToList());
            retval.Add(string.Format("Total: {0} Plunder Points occupying {1} Cargo Points.", plunder.Count, plunder.Sum(a => a.CargoPoints)));

            return string.Join(Environment.NewLine, retval);
        }

        [TypedCommand("LoadCargo", "Puts some plunder on a ship.")]
        public string LoadCargo(string shipName, string cargoName)
        {
            var ship = LoadShip(shipName);
            var cargo = LoadCargo(cargoName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName)
                && cargo != null && cargo.Length > 0)
            {
                ship.ShipsCargo.AddRange(cargo);

                var response = WriteAsset(ship);

                if (response.Success)
                {
                    List<string> messages = new List<string>();
                    messages.Add("Added cargo to ship...");
                    cargo.ToList().ForEach(a => messages.Add(a.ToString()));
                    return string.Join(Environment.NewLine, messages);
                }
                else
                {
                    return string.Join(Environment.NewLine, response.Messages);
                }
            }
            else if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                return "Can't find ship.";
            }
            else
            {
                return "Can't find cargo.";
            }
        }

        [TypedCommand("Disembark", "Removes a crew member from a ship.")]
        public string Disembark(string shipName, string crewName)
        {
            var ship = LoadShip(shipName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                var crew = ship.ShipsCrew.FirstOrDefault(a => a.Name == crewName);

                if (crew != null && !string.IsNullOrEmpty(crew.Name))
                {
                    var response = WriteAsset(crew);

                    if (response.Success)
                    {
                        if (ship.ShipsCrew.Remove(crew))
                        {
                            return string.Format("{0} left the ship.", crewName);
                        }
                        else
                        {
                            return "Can't remove crew.";
                        }
                    }
                    else
                    {
                        return string.Join(Environment.NewLine, response.Messages);
                    }
                }
                else
                {
                    return string.Format("{0} is not aboard the {1}.", crewName, shipName);
                }
            }
            else
            {
                return "Can't find ship.";
            }
        }

        [TypedCommand("Embark", "Loads crew onto a ship.")]
        public string Embark(string shipName, string crewName)
        {
            var ship = LoadShip(shipName);
            var crew = LoadCrew(crewName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName)
                && crew != null && crew.Length > 0)
            {
                foreach (var mate in crew)
                {
                    var existing = ship.ShipsCrew.FirstOrDefault(a => a.Name == mate.Name);

                    if (existing != null && existing.Name == mate.Name)
                    {
                        ship.ShipsCrew.Remove(existing);
                        ship.ShipsCrew.Add(mate);
                    }
                    else
                    {
                        ship.ShipsCrew.Add(mate);
                    }
                }

                var response = WriteAsset(ship);

                if (response.Success)
                    return "Added crew to ship.";
                else
                    return string.Join(Environment.NewLine, response.Messages);
            }
            else if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                return "Can't find ship.";
            }
            else
            {
                return "Can't find cargo.";
            }
        }

        [TypedCommand("AddBoat", "Adds a boat to a ship.")]
        public string AddBoat(string shipName, string boatClass)
        {
            var ship = LoadShip(shipName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                if (Enum.TryParse<BoatClasses>(boatClass, true, out BoatClasses result))
                {
                    var boat = CargoFactory.Instance.ProduceCargo(CargoType.ShipsBoat, boatClass);

                    ship.ShipsCargo.Add(boat);

                    var response = WriteAsset(ship);

                    if (response.Success)
                        return "Added boat to ship.";
                    else
                        return string.Join(Environment.NewLine, response.Messages);
                }
                else
                {
                    return "Unrecognized boat class.";
                }
            }
            else
            {
                return "Can't find ship.";
            }
        }

        [TypedCommand("AddAmmunition", "Adds ammunition to a ship.")]
        public string AddAmmunition(string shipName, string siegeEngine)
        {
            var ship = LoadShip(shipName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                if (Enum.TryParse<SiegeEngineType>(siegeEngine, true, out SiegeEngineType result))
                {
                    var ammo = CargoFactory.Instance.ProduceCargo(CargoType.Ammunition, siegeEngine);

                    ship.ShipsCargo.Add(ammo);

                    var response = WriteAsset(ship);

                    if (response.Success)
                        return "Added ammunition to ship.";
                    else
                        return string.Join(Environment.NewLine, response.Messages);
                }
                else
                {
                    return "Unrecognized siege engine type.";
                }
            }
            else
            {
                return "Can't find ship.";
            }
        }

        [TypedCommand("AddSeigeEngine", "Adds a boat to a ship.")]
        public string AddSeigeEngine(string shipName, string siegeEngine)
        {
            var ship = LoadShip(shipName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                if (Enum.TryParse<SiegeEngineType>(siegeEngine, true, out SiegeEngineType result))
                {
                    var gun = CargoFactory.Instance.ProduceCargo(CargoType.SeigeWeapon, siegeEngine);

                    ship.ShipsCargo.Add(gun);

                    var response = WriteAsset(ship);

                    if (response.Success)
                        return "Added siege engine to ship.";
                    else
                        return string.Join(Environment.NewLine, response.Messages);
                }
                else
                {
                    return "Unrecognized siege engine type.";
                }
            }
            else
            {
                return "Can't find ship.";
            }
        }
        [TypedCommand("AddSupplies", "Adds supplies to a ship.")]
        public string AddSuppplies(string shipName, string supplyType)
        {
            var ship = LoadShip(shipName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                if (Enum.TryParse<SupplyType>(supplyType, true, out SupplyType result))
                {
                    var s = CargoFactory.Instance.ProduceSupplies(result);

                    ship.ShipsCargo.Add(s);

                    var response = WriteAsset(ship);

                    if (response.Success)
                        return "Added supplies to ship.";
                    else
                        return string.Join(Environment.NewLine, response.Messages);
                }
                else
                {
                    return "Unrecognized supply type.";
                }
            }
            else
            {
                return "Can't find ship.";
            }
        }
        [TypedCommand("RandomCrewName", "Adds supplies to a ship.")]
        public string RandomCrewName(string shipName)
        {
            var ship = LoadShip(shipName);

            if (ship != null && !string.IsNullOrEmpty(ship.CrewName))
            {
                return ship.GetRandomCrewName();
            }
            else
            {
                return "Can't find ship.";
            }
        }
        [TypedCommand("TestRandom", "How random is that dice roller anyway?")]
        public string TestRandom(int tries)
        {
            int[] list;
            if (tries > 0)
                list = new int[tries];
            else
                return "Tries must be a positive integer.";

            for(int i = 0; i < tries; i++)
            {
                list[i] = DiceRoller.D20(1);
            }

            var mean = list.ToList().Average();
            var mode = list.ToList().GroupBy(a => a).Select(b => new { key = b, count = b.Count() }).ToDictionary(c => c.key, c => c.count);
            var stddev = list.ToList().Sum(a => Math.Pow(a - mean, 2)) / tries;
            stddev = Math.Sqrt(stddev);

            return string.Format("Average: {0}", mean);
        }

        private BaseResponse ProcessOMGArguments(string[] args, ref Ship ship, ref List<string> messages)
        {
            BaseResponse retval = new BaseResponse();

            foreach (var arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                    continue;

                string[] parts;
                string term = null;
                string value = null;
                if (arg.Contains(':'))
                {
                    parts = arg.Split(':');
                    term = parts[0].Trim();
                    value = parts[1].Trim();
                }
                else
                {
                    parts = new string[1] { arg };
                    term = arg;
                }

                // Argument to refit.
                switch (term.ToUpper())
                {
                    case "M":
                    case "MORALE":
                        // Argument to set temporary morale penalty through 'TemporaryMoralePenalty'.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                messages.Add(string.Format("Overall morale penalty changed to {0}. (Positive is bad.)", temp));
                                ship.CrewMorale.TemporaryMoralePenalty = temp;
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
                                ship.CrewMorale.TemporaryMoralePenalty += temp;
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
                                if (ship.CrewMorale.TemporaryMoralePenalty >= temp)
                                    ship.CrewMorale.TemporaryMoralePenalty = 0;
                                else
                                    ship.CrewMorale.TemporaryMoralePenalty -= temp;
                                messages.Add(string.Format("Removed {1} from temporary morale penalty. New value is {1}. (Positive is bad.)", temp, ship.CrewMorale.TemporaryMoralePenalty));
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
                                ship.CrewMorale.Piracy = temp;
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
                                ship.CrewMorale.WellBeing = temp;
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
                                ship.CrewMorale.ClearTemporaryModifiers(MoralTypes.Wellbeing);
                                ship.CrewMorale.AddTemporaryModifier(MoralTypes.Wellbeing, temp);
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
                                ship.CrewMorale.Wealth = temp;
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
                                ship.CrewMorale.Wealth = temp;
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
                                ship.CrewMorale.ShipShape = temp;
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
                            switch (value)
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
                                if (temp > 0)
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
                    case "DAYS+":
                        // Argument to add days to the journey.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp > 0)
                                {
                                    ship.AddDaysToVoyage(temp);
                                    ; messages.Add(string.Format("Extended voyage by {0} days.", temp));
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
                    case "WHIP":
                    case "DL":
                    case "DISCIPLINESTANDARDS":
                        // Adjust ship discipline standards
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                ship.DisciplineStandards = (DisciplineStandards)temp;
                            }
                            else if (Enum.TryParse<DisciplineStandards>(value, true, out DisciplineStandards result))
                            {
                                ship.DisciplineStandards = result;
                            }
                            else
                            {
                                retval.Messages.Add("Unrecognized discipline standard. Use 'whip:#' or spell out condition such as 'whip:Strict'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("'Discipline Standard' requires value.  Use 'whip:#' or spell out condition such as 'whip:Strict'.");
                        }
                        break;
                    case "SWAB":
                    case "SWABBIES":
                    case "CREW":
                        // Change the count of non-named crew.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp < 0)
                                {
                                    retval.Messages.Add("Setting count of general crew requires positive integer.  Use 'swab:#'.");
                                }
                                else
                                {
                                    ship.Swabbies = temp;
                                    retval.Messages.Add(string.Format("Swabbie count now {0}.", temp));
                                }
                            }
                            else
                            {
                                retval.Messages.Add("Setting count of general crew requires positive integer.  Use 'swab:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Setting count of general crew requires value.  Use 'swab:#'.");
                        }
                        break;
                    case "SWAB+":
                    case "SWABBIES+":
                    case "CREW+":
                        // Change the count of non-named crew.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp < 0)
                                {
                                    retval.Messages.Add("Increasing count of general crew requires a positive integer.  Use 'swab+:#'.");
                                }
                                else
                                {
                                    ship.Swabbies += temp;
                                    retval.Messages.Add(string.Format("Adding {0} crew. Swabbie count now {1}.", temp, ship.Swabbies));
                                }
                            }
                            else
                            {
                                retval.Messages.Add("Increasing count of general crew requires a positive integer.  Use 'swab+:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Increasing count of general crew crew requires value.  Use 'swab+:#'.");
                        }
                        break;
                    case "SWAB-":
                    case "SWABBIES-":
                    case "CREW-":
                        // Change the count of non-named crew.
                        if (value != null)
                        {
                            if (int.TryParse(value, out int temp))
                            {
                                if (temp < 0)
                                {
                                    retval.Messages.Add("Reducing count of general crew requires a positive integer.  Use 'swab-:#'.");
                                }
                                else
                                {
                                    ship.Swabbies += temp;
                                    retval.Messages.Add(string.Format("Removing {0} crew. Swabbie count now {1}.", temp, ship.Swabbies));
                                }
                            }
                            else
                            {
                                retval.Messages.Add("Reducing count of general crew requires a positive integer.  Use 'swab-:#'.");
                            }
                        }
                        else
                        {
                            retval.Messages.Add("Reducing count of general crew crew requires value.  Use 'swab-:#'.");
                        }
                        break;
                    case "J":
                    case "J+":
                    case "JOB":
                    case "JOB+":
                    case "DUTY":
                    case "DUTY+":
                        // Four part command to assign job j:<CrewMember>:<Duty>:<Assitant>, where <assistant> is optional and assumed false.
                        if (parts.Length < 3)
                        {
                            retval.Messages.Add("To assign job, use 'j:<CrewMember>:<Duty>:<IsAssitant>'.");
                        }
                        else
                        {
                            string crewname = parts[1];
                            string duty = parts[2];
                            string assistant = null;
                            if (parts.Length > 3)
                                assistant = parts[3];

                            DutyType? foundDuty = null;

                            if (int.TryParse(duty, out int temp))
                            {
                                if (Enum.IsDefined(typeof(DutyType), temp))
                                    foundDuty = (DutyType)temp;
                                else
                                    retval.Messages.Add(string.Format("Unrecognized duty '{0}' when adding job.", duty));
                            }
                            else if (Enum.TryParse<DutyType>(value, true, out DutyType result))
                            {
                                foundDuty = result;
                            }
                            else
                            {
                                retval.Messages.Add(string.Format("Unrecognized duty '{0}' when adding job.", duty));
                            }

                            if (foundDuty.HasValue)
                            {
                                BaseResponse response = null;
                                if (assistant != null)
                                {
                                    if (bool.TryParse(assistant, out bool result))
                                    {
                                        response = ship.AssignJob(crewname, foundDuty.Value, result);
                                    }
                                    else
                                    {
                                        retval.Messages.Add(string.Format("Unrecognized assistant flag '{0}' when adding job.  Use 'true', 'false', '0' or '1'.", duty));
                                    }
                                }
                                else
                                {
                                    response = ship.AssignJob(crewname, foundDuty.Value, false);
                                }

                                if (response != null && !response.Success)
                                    retval.Messages.AddRange(response.Messages);
                            }
                        }
                        break;
                    case "J-":
                    case "JOB-":
                    case "DUTY-":
                        // Four part command to remove job j:<CrewMember>:<Duty>:<Assitant>, where <assistant> is optional and assumed false.
                        if (parts.Length < 3)
                        {
                            retval.Messages.Add("To remove job, use 'j-:<CrewMember>:<Duty>:<IsAssitant>'.");
                        }
                        else
                        {
                            string crewname = parts[1];
                            string duty = parts[2];
                            string assistant = null;
                            if (parts.Length > 3)
                                assistant = parts[3];

                            DutyType? foundDuty = null;

                            if (int.TryParse(duty, out int temp))
                            {
                                if (Enum.IsDefined(typeof(DutyType), temp))
                                    foundDuty = (DutyType)temp;
                                else
                                    retval.Messages.Add(string.Format("Unrecognized duty '{0}' when removing job.", duty));
                            }
                            else if (Enum.TryParse<DutyType>(value, true, out DutyType result))
                            {
                                foundDuty = result;
                            }
                            else
                            {
                                retval.Messages.Add(string.Format("Unrecognized duty '{0}' when removing job.", duty));
                            }

                            if (foundDuty.HasValue)
                            {
                                BaseResponse response = null;
                                if (assistant != null)
                                {
                                    if (bool.TryParse(assistant, out bool result))
                                    {
                                        response = ship.RemoveJob(crewname, foundDuty.Value, result);
                                    }
                                    else
                                    {
                                        retval.Messages.Add(string.Format("Unrecognized assistant flag '{0}' when removing job.  Use 'true', 'false', '0' or '1'.", duty));
                                    }
                                }
                                else
                                {
                                    response = ship.RemoveJob(crewname, foundDuty.Value, false);
                                }

                                if (response != null && !response.Success)
                                    retval.Messages.AddRange(response.Messages);
                            }
                        }
                        break;
                    default:
                        if (value != null)
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

        /// <summary>
        /// IMPORTANT: Don't deserialize ships from an untrusted source.
        /// </summary>
        /// <returns>All the ships in a dictionary by name.</returns>
        private Dictionary<string, Ship> LoadShips()
        {
            var folder = ".\\Ships";
            if (!Directory.Exists(folder))
            {
                return new Dictionary<string, Ship>();
            }
            var charFiles = Directory.GetFiles(folder, "*.json");
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            return charFiles.Select(cf => JsonConvert.DeserializeObject<Ship>(File.ReadAllText(cf), settings)).ToDictionary(x => x.CrewName, x => x);
        }

        /// <summary>
        /// IMPORTANT: Don't deserialize ships from an untrusted source.
        /// </summary>
        /// <param name="shipName">The name of the .json file in the 'Ships' folder to load.</param>
        /// <returns>A ship.</returns>
        private Ship LoadShip(string shipName)
        {
            var folder = ".\\Ships";
            if (!Directory.Exists(folder))
            {
                return new Ship();
            }
            string file = Directory.GetFiles(folder, string.Format("{0}.json", shipName.Replace(' ', '_'))).First();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            return JsonConvert.DeserializeObject<Ship>(File.ReadAllText(file), settings);
        }

        private Cargo[] LoadCargo(string fileName)
        {
            var folder = ".\\Cargo";
            if (!Directory.Exists(folder))
            {
                return new List<Cargo>().ToArray();
            }
            string file = Directory.GetFiles(folder, string.Format("{0}.json", fileName.Replace(' ', '_'))).FirstOrDefault();
            if (file != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                return JsonConvert.DeserializeObject<Cargo[]>(File.ReadAllText(file), settings);
            }
            else
            {
                return new List<Cargo>().ToArray();
            }
        }

        private CrewMember[] LoadCrew(string filename)
        {
            var folder = ".\\Crews";
            if (!Directory.Exists(folder))
            {
                return new List<CrewMember>().ToArray();
            }
            string file = Directory.GetFiles(folder, string.Format("{0}.json", filename.Replace(' ', '_'))).FirstOrDefault();
            if (file != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                return JsonConvert.DeserializeObject<CrewMember[]>(File.ReadAllText(file), settings);
            }
            else
            {
                return new List<CrewMember>().ToArray();
            }
        }

        private BaseResponse WriteAsset(Ship ship, string filename = null)
        {
            BaseResponse retval = new BaseResponse();
            if (filename == null)
                filename = ship.CrewName;
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                string contents = JsonConvert.SerializeObject(ship, Formatting.Indented, settings);
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

        private BaseResponse WriteAsset(Cargo cargo, string filename = null)
        {
            return WriteAsset(new Cargo[1] { cargo }, filename);
        }

        private BaseResponse WriteAsset(Cargo[] cargo, string filename = null)
        {
            BaseResponse retval = new BaseResponse();

            int filenumber = 1;

            if (filename == null)
                filename = string.Format("Cargo{0}.json", filenumber); 

            try
            {
                var folder = ".\\Cargo";
                DirectoryInfo dir = null;
                if (!Directory.Exists(folder))
                {
                    dir = Directory.CreateDirectory(folder);
                }
                else
                {
                    dir = new DirectoryInfo(folder);
                }

                FileInfo[] file;

                filename = string.Format("Cargo{0}.json", filenumber);

                file = dir.GetFiles(filename);
                while (file != null && file.Length > 0)
                {
                    filenumber++;
                    filename = string.Format("Cargo{0}.json", filenumber);
                    file = dir.GetFiles(filename);
                }

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.All;
                string contents = JsonConvert.SerializeObject(cargo, Formatting.Indented, settings);

                File.WriteAllText(string.Format("{0}\\{1}", folder, filename), contents);
                retval.Messages.Add(string.Format("Stored plunder as file '{0}'.", filename));
                retval.Success = true;
            }
            catch(Exception ex)
            {
                retval.Messages.Add(string.Format("ERROR: {0}", ex.Message));
            }

            return retval;
        }

        private BaseResponse WriteAsset(CrewMember crew, string filename = null)
        {
            return WriteAsset(new CrewMember[] { crew }, crew.Name.Replace(' ', '_'));
        }

        private BaseResponse WriteAsset(CrewMember[] crew, string filename)
        {
            BaseResponse retval = new BaseResponse();

            if (filename == null)
            {
                retval.Messages.Add("Must give filename when writing crew.");
                return retval;
            }

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                string contents = JsonConvert.SerializeObject(crew, Formatting.Indented, settings);
                var folder = ".\\Crews";
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
