using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class CargoFactory
    {
        private static CargoFactory _instance;

        public static CargoFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CargoFactory();

                return _instance;
            }
        }

        public Cargo ProduceCargo(CargoType cargoType, string hint = null)
        {
            switch (cargoType)
            {
                case CargoType.Ammunition:
                    return MakeAmmunition(hint);
                case CargoType.DryContainers:
                    return new Cargo()
                    {
                        Name = "Empty Crates",
                        CargoType = CargoType.DryContainers,
                        CargoPoints = 1,
                        DaysInHold = 0,
                        Fragile = false,
                        Perishable = false,
                        Value = 75
                    };
                case CargoType.Food:
                    return new Supplies()
                    {
                        Name = GetFoodName(),
                        CargoType = CargoType.Rum,
                        CargoPoints = 4,
                        DaysInHold = 0,
                        Fragile = false,
                        Perishable = false,
                        SupplyType = SupplyType.Water,
                        UnitsSupplyPerPoint = 900,
                        UnitsSupplyRemaining = 900,
                        Value = 1000
                    };
                case CargoType.Medicine:
                    return new Supplies()
                    {
                        Name = "Medicine",
                        CargoType = CargoType.Medicine,
                        CargoPoints = 1,
                        DaysInHold = 0,
                        Fragile = true,
                        Perishable = false,
                        SupplyType = SupplyType.Medicine,
                        UnitsSupplyPerPoint = 1000,
                        UnitsSupplyRemaining = 1000,
                        Value = 1000
                    };
                case CargoType.Passengers:
                    return new Cargo()
                    {
                        Name = "Passengers",
                        CargoType = CargoType.Passengers,
                        CargoPoints = 1,
                        DaysInHold = 0,
                        Fragile = false,
                        Perishable = false,
                        Value = 0
                    };
                case CargoType.Plunder:
                    return Loot(hint);
                case CargoType.Rum:
                    return new Supplies()
                    {
                        Name = "Rum",
                        CargoType = CargoType.Rum,
                        CargoPoints = 4,
                        DaysInHold = 0,
                        Fragile = false,
                        Perishable = false,
                        SupplyType = SupplyType.Water,
                        UnitsSupplyPerPoint = 900,
                        UnitsSupplyRemaining = 900,
                        Value = 1000
                    };
                case CargoType.SeigeWeapon:
                    return MakeSiegeEngine(hint);
                case CargoType.ShipsBoat:
                    return AddBoat(hint);
                case CargoType.ShipSupplies:
                    return new Supplies()
                    {
                        Name = "Ships Supplies",
                        CargoType = CargoType.ShipSupplies,
                        CargoPoints = 6,
                        DaysInHold = 0,
                        Fragile = false,
                        Perishable = false,
                        SupplyType = SupplyType.ShipSupplies,
                        UnitsSupplyPerPoint = 100,
                        UnitsSupplyRemaining = 100,
                        Value = 1000
                    };
                case CargoType.Water:
                    return new Supplies()
                    {
                        Name = "Water",
                        CargoType = CargoType.Water,
                        CargoPoints = 4,
                        DaysInHold = 0,
                        Fragile = false,
                        Perishable = false,
                        SupplyType = SupplyType.Water,
                        UnitsSupplyPerPoint = 900,
                        UnitsSupplyRemaining = 900,
                        Value = 300
                    };
                case CargoType.WetContainers:
                    return new Cargo()
                    {
                        Name = "Empty Barrels",
                        CargoType = CargoType.WetContainers,
                        CargoPoints = 1,
                        DaysInHold = 0,
                        Fragile = false,
                        Perishable = false,
                        Value = 75
                    };
                case CargoType.Fodder:
                    return new Supplies()
                    {
                        Name = "Fodder",
                        CargoPoints = 6,
                        DaysInHold = 0,
                        CargoType = CargoType.Fodder,
                        Fragile = false,
                        Perishable = false,
                        SupplyType = SupplyType.Fodder,
                        UnitsSupplyPerPoint = 7,
                        UnitsSupplyRemaining = 1,
                        Value = 300
                    };
                default:
                    return new Cargo();
            }
        }

        private string GetPlunderName(PlunderCategory pc, out bool perishable, out bool fragile, out bool livestock)
        {
            switch (pc)
            {
                case PlunderCategory.Bulk:
                    return GetBulkPlunderName(out perishable, out fragile, out livestock);
                case PlunderCategory.Commodity:
                    return GetCommodityPlunderName(out perishable, out fragile, out livestock);
                case PlunderCategory.Trade:
                    return GetTradePlunderName(out perishable, out fragile, out livestock);
                case PlunderCategory.Luxury:
                    return GetLuxuryPlunderName(out perishable, out fragile, out livestock);
                case PlunderCategory.Precious:
                    return GetPreciousPlunderName(out perishable, out fragile, out livestock);
                default:
                    return GetCommodityPlunderName(out perishable, out fragile, out livestock);
            }
        }

        private string GetPlunderName(PlunderCategory pc, int hint, out bool perishable, out bool fragile, out bool livestock)
        {
            switch (pc)
            {
                case PlunderCategory.Bulk:
                    return GetBulkPlunderName(hint, out perishable, out fragile, out livestock);
                case PlunderCategory.Commodity:
                    return GetCommodityPlunderName(out perishable, out fragile, out livestock);
                case PlunderCategory.Trade:
                    return GetTradePlunderName(out perishable, out fragile, out livestock);
                case PlunderCategory.Luxury:
                    return GetLuxuryPlunderName(out perishable, out fragile, out livestock);
                case PlunderCategory.Precious:
                    return GetPreciousPlunderName(out perishable, out fragile, out livestock);
                default:
                    return GetCommodityPlunderName(out perishable, out fragile, out livestock);
            }
        }

        private string GetBulkPlunderName(int hint, out bool perishable, out bool fragile, out bool livestock)
        {
            livestock = false;
            switch (hint)
            {
                case 1:
                case 2:
                case 3:
                    perishable = false;
                    fragile = false;
                    return "Lumber";
                case 4:
                case 5:
                case 6:
                    perishable = true;
                    fragile = false;
                    return ReturnGrainName();
                case 7:
                    perishable = false;
                    fragile = false;
                    return "Coal";
                case 8:
                case 9:
                    perishable = false;
                    fragile = false;
                    return "Stone";
                case 10:
                    perishable = false;
                    fragile = false;
                    return "Cork";
                case 11:
                    perishable = false;
                    fragile = false;
                    return "Bricks";
                case 12:
                case 13:
                    perishable = false;
                    fragile = false;
                    return "Cotton";
                case 14:
                case 15:
                    perishable = false;
                    fragile = false;
                    return "Flax";
                case 16:
                case 17:
                    perishable = false;
                    fragile = false;
                    return "Hemp";
                case 18:
                case 19:
                    perishable = true;
                    fragile = true;
                    livestock = true;
                    return GetLiveStockName();
                case 20:
                    perishable = true;
                    fragile = false;
                    return "Raisins";
                default:
                    perishable = false;
                    fragile = false;
                    return "Hardtack";
            }
        }

        private string GetBulkPlunderName(out bool perishable, out bool fragile, out bool livestock)
        {
            return GetBulkPlunderName(DiceRoller.D20(1), out perishable, out fragile, out livestock);
        }

        private string GetCommodityPlunderName(int hint, out bool perishable, out bool fragile, out bool livestock)
        {
            livestock = false;
            switch (hint)
            {
                case 1:
                    perishable = false;
                    fragile = false;
                    return "Vinegar";
                case 2:
                case 3:
                    perishable = false;
                    fragile = false;
                    return "Oil";
                case 4:
                    perishable = true;
                    fragile = false;
                    return "Beer";
                case 5:
                case 6:
                    perishable = false;
                    fragile = false;
                    return "Iron";
                case 7:
                    perishable = false;
                    fragile = false;
                    return "Lead";
                case 8:
                case 9:
                    perishable = false;
                    fragile = false;
                    return "Wool";
                case 10:
                    perishable = false;
                    fragile = false;
                    return "Hides";
                case 11:
                    perishable = false;
                    fragile = false;
                    return "Rum";
                case 12:
                    perishable = false;
                    fragile = false;
                    return "Salt";
                case 13:
                case 14:
                case 23:
                case 24:
                    perishable = true;
                    fragile = false;
                    return "Sugar";
                case 15:
                    perishable = false;
                    fragile = true;
                    return "Pottery";
                case 16:
                    perishable = false;
                    fragile = false;
                    return "Thread";
                case 17:
                    perishable = false;
                    fragile = false;
                    return "Dried Fish";
                case 18:
                    perishable = false;
                    fragile = false;
                    return "Salted Meat";
                case 19:
                    perishable = false;
                    fragile = false;
                    return "Pitch";
                case 20:
                    perishable = false;
                    fragile = false;
                    return "Marble";
                case 21:
                    perishable = false;
                    fragile = false;
                    return "Pickles";
                case 22:
                    perishable = false;
                    fragile = false;
                    return "Rope";
                default:
                    perishable = false;
                    fragile = false;
                    return "Sugar";
            }
        }

        private string GetCommodityPlunderName(out bool perishable, out bool fragile, out bool livestock)
        {
            return GetCommodityPlunderName(DiceRoller.Roll(24, 1), out perishable, out fragile, out livestock);
        }

        private string ReturnGrainName()
        {
            switch (DiceRoller.D8(1))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return "Wheat";
                case 5:
                    return "Rye";
                case 6:
                    return "Barley";
                case 7:
                    return "Oats";
                case 8:
                    return "Rice";
                default:
                    return "Quinoa";
            }
        }

        private string GetLiveStockName()
        {
            switch (DiceRoller.D8(1))
            {
                case 1:
                case 2:
                    return "Pigs";
                case 3:
                case 4:
                    return "Sheep";
                case 5:
                case 6:
                    return "Cattle";
                case 7:
                    return "Horses";
                case 8:
                    return "Camels";
                default:
                    return "Rabbits";
            }
        }

        private string GetTradePlunderName(int hint, out bool perishable, out bool fragile, out bool livestock)
        {
            livestock = false;
            switch (hint)
            {
                case 1:
                case 2:
                case 3:
                    perishable = false;
                    fragile = false;
                    return "Wine";
                case 4:
                    perishable = false;
                    fragile = false;
                    return "Copper";
                case 5:
                    perishable = false;
                    fragile = false;
                    return "Tin";
                case 6:
                    perishable = false;
                    fragile = false;
                    return "Glass";
                case 7:
                case 8:
                case 9:
                    perishable = false;
                    fragile = false;
                    return "Cloth";
                case 10:
                    perishable = false;
                    fragile = false;
                    return "Whale Oil";
                case 11:
                    perishable = false;
                    fragile = false;
                    return "Alabaster";
                case 12:
                case 13:
                    perishable = false;
                    fragile = false;
                    return "Tea";
                case 14:
                case 15:
                    perishable = false;
                    fragile = false;
                    return "Coffee";
                case 16:
                    perishable = false;
                    fragile = false;
                    return "Leather";
                case 17:
                    perishable = false;
                    fragile = false;
                    return "Wire";
                case 18:
                    perishable = false;
                    fragile = false;
                    return "Nails";
                case 19:
                    perishable = false;
                    fragile = false;
                    return "Tools";
                case 20:
                    perishable = true;
                    fragile = false;
                    return GetNutsName();
                case 21:
                    perishable = true;
                    fragile = false;
                    return GetFruitName();
                default:
                    perishable = false;
                    fragile = false;
                    return "Hardtack";
            }
        }

        private string GetTradePlunderName(out bool perishable, out bool fragile, out bool livestock)
        {
            return GetTradePlunderName(DiceRoller.Roll(21, 1), out perishable, out fragile, out livestock);
        }

        private string GetFruitName()
        {
            switch (DiceRoller.D8(1))
            {
                case 1:
                    return "Apples";
                case 2:
                    return "Citron";
                case 3:
                    return "Blood Oranges";
                case 4:
                    return "Grapefruit";
                case 5:
                case 6:
                    return "Lemons";
                case 7:
                case 8:
                    return "Limes";
                default:
                    return "Limes";
            }
        }

        private string GetNutsName()
        {
            switch (DiceRoller.D8(1))
            {
                case 1:
                    return "Walnuts";
                case 2:
                    return "Pecans";
                case 3:
                    return "Almonds";
                case 4:
                    return "Pistachios";
                case 5:
                case 6:
                case 7:
                case 8:
                    return "Coconuts";
                default:
                    return "Chestnuts";
            }
        }

        private string GetLuxuryPlunderName(int hint, out bool perishable, out bool fragile, out bool livestock)
        {
            livestock = false;
            switch (hint)
            {
                case 1:
                    perishable = false;
                    fragile = false;
                    return "Armor";
                case 2:
                case 3:
                    perishable = false;
                    fragile = false;
                    return "Fine Wine";
                case 4:
                case 5:
                    perishable = false;
                    fragile = false;
                    return "Dyes";
                case 6:
                case 7:
                    perishable = false;
                    fragile = true;
                    return "Porcelain";
                case 8:
                case 9:
                    perishable = false;
                    fragile = false;
                    return "Silk";
                case 10:
                case 11:
                    perishable = false;
                    fragile = false;
                    return GetRareWoodName(); 
                case 12:
                case 13:
                    perishable = false;
                    fragile = true;
                    return "Glassware";
                case 14:
                    perishable = false;
                    fragile = false;
                    return "Weapons";
                case 15:
                    perishable = false;
                    fragile = false;
                    return "Paper Parchment";
                case 16:
                case 17:
                    perishable = false;
                    fragile = false;
                    return "Whiskey";
                case 18:
                    perishable = false;
                    fragile = false;
                    return "Honey";
                case 19:
                    perishable = false;
                    fragile = false;
                    return "Herbs";
                case 20:
                    perishable = false;
                    fragile = false;
                    return "Wax";
                default:
                    perishable = false;
                    fragile = false;
                    return "Gizmos";
            }
        }

        private string GetLuxuryPlunderName(out bool perishable, out bool fragile, out bool livestock)
        {
            return GetLuxuryPlunderName(DiceRoller.D20(1), out perishable, out fragile, out livestock); 
        }

        private string GetRareWoodName()
        {
            switch(DiceRoller.D20(1))
            {
                case 1:
                case 2:
                case 3:
                    return "Ebony";
                case 4:
                case 5:
                    return "Rosewood";
                case 6:
                    return "Bocate Wood";
                case 7:
                    return "Agar Wood";
                case 8:
                case 9:
                case 10:
                    return "Sandalwood";
                case 11:
                    return "Bubinga Wood";
                case 12:
                    return "Lignum vitae";
                case 13:
                    return "Umnini Wood";
                case 14:
                case 15:
                    return "Blackwood";
                case 16:
                    return "Ironwood";
                case 17:
                    return "Bloodwood";
                case 18:
                    return "Cocobolo Wood";
                case 19:
                    return "Pernambuco wood";
                case 20:
                    return "Tulipwood";
                default:
                    return "Groot wood";
            }
        }

        private string GetPreciousPlunderName(int hint, out bool perishable, out bool fragile, out bool livestock)
        {
            livestock = false;
            switch (hint)
            {
                case 1:
                    perishable = false;
                    fragile = false;
                    return "Art Goods";
                case 2:
                    perishable = false;
                    fragile = false;
                    return "Alchemical Reagents";
                case 3:
                    perishable = false;
                    fragile = false;
                    return "Books";
                case 4:
                    perishable = false;
                    fragile = false;
                    return "Furs";
                case 5:
                    perishable = false;
                    fragile = false;
                    return "Jewelry";
                case 6:
                case 7:
                case 8:
                    perishable = false;
                    fragile = false;
                    return GetSpiceName(); //"Spices";
                case 9:
                case 10:
                    perishable = false;
                    fragile = false;
                    return "Perfume";
                case 11:
                case 12:
                    perishable = false;
                    fragile = false;
                    return "Incense";
                case 13:
                    perishable = false;
                    fragile = false;
                    return "Amber";
                case 14:
                case 15:
                    perishable = false;
                    fragile = false;
                    return "Ivory";
                case 16:
                    perishable = false;
                    fragile = false;
                    return "Jade";
                case 17:
                    perishable = false;
                    fragile = false;
                    return "Medicine";
                case 18:
                case 19:
                case 20:
                    perishable = false;
                    fragile = false;
                    return "Silver Bullion";
                case 21:
                    perishable = false;
                    fragile = false;
                    return "Vellum";
                case 22:
                case 23:
                    perishable = false;
                    fragile = false;
                    return "Tapestries";
                case 24:
                    perishable = false;
                    fragile = false;
                    return SemiPreciousStone();
                default:
                    perishable = false;
                    fragile = false;
                    return "Beanie Babies";
            }
        }

        private string GetPreciousPlunderName(out bool perishable, out bool fragile, out bool livestock)
        {
            return GetPreciousPlunderName(DiceRoller.Roll(24, 1), out perishable, out fragile, out livestock);
        }

        private string GetSpiceName()
        {
            switch (DiceRoller.D20(1))
            {
                case 1:
                    return "Cardamon";
                case 2:
                case 3:
                case 4:
                case 5:
                    return "Black Pepper";
                case 6:
                case 7:
                    return "Cloves";
                case 8:
                case 9:
                    return "Cinnamon";
                case 10:
                    return "Mace";
                case 11:
                    return "Nutmeg";
                case 12:
                    return "Chocolate";
                case 13:
                    return "Ginger";
                case 14:
                    return "Tumeric";
                case 15:
                    return "Cumin";
                case 16:
                    return "Anise";
                case 17:
                case 18:
                    return "Vanilla";
                case 19:
                    return "Allspice";
                case 20:
                    return "Galangal";
                default:
                    return "Cayenne";
            }
        }

        private string SemiPreciousStone()
        {
            switch(DiceRoller.D20(1))
            {
                case 1:
                    return "Chalcedony";
                case 2:
                case 3:
                case 4:
                case 5:
                    return "Agate";
                case 6:
                    return "Jasper";
                case 7:
                case 8:
                case 9:
                    return "Coral";
                case 10:
                    return "Mother of Pearl";
                case 11:
                    return "Obsidian";
                case 12:
                    return "Bloodstone";
                case 13:
                    return "Hematite";
                case 14:
                    return "Onyx";
                case 15:
                    return "Serpentine";
                case 16:
                case 17:
                case 18:
                    return "Malachite";
                case 19:
                    return "Verdite";
                case 20:
                    return "Azurite";
                default:
                    return "Plastic";
            }
        }

        private string GetFoodName()
        {
            Dictionary<string, bool> foodstuffs = new Dictionary<string, bool>();

            for (int i = 0; i < 4; i++)
            {
                switch (DiceRoller.D20(1))
                {
                    case 1:
                    case 2:
                    case 3:
                    case 15:
                        if (!foodstuffs.ContainsKey("Hardtack"))
                            foodstuffs.Add("Hardtack", true);
                        break;
                    case 4:
                        if (!foodstuffs.ContainsKey("Salt beef"))
                            foodstuffs.Add("Salt beef", true);
                        break;
                    case 5:
                    case 6:
                    case 16:
                        if (!foodstuffs.ContainsKey("Salt pork"))
                            foodstuffs.Add("Salt pork", true);
                        break;
                    case 7:
                        if (!foodstuffs.ContainsKey("Dried Peas"))
                            foodstuffs.Add("Dried Peas", true);
                        break;
                    case 8:
                        if (!foodstuffs.ContainsKey("Sourkraut"))
                            foodstuffs.Add("Sourkraut", true);
                        break;
                    case 9:
                        if (!foodstuffs.ContainsKey("Hard Cheese"))
                            foodstuffs.Add("Hard Cheese", true);
                        break;
                    case 10:
                        if (!foodstuffs.ContainsKey("Lime Juice"))
                            foodstuffs.Add("Lime Juice", true);
                        break;
                    case 11:
                    case 12:
                    case 17:
                        if (!foodstuffs.ContainsKey("Milled Oats"))
                            foodstuffs.Add("Milled Oats", true);
                        break;
                    case 13:
                        if (!foodstuffs.ContainsKey("Rice"))
                            foodstuffs.Add("Rice", true);
                        break;
                    case 14:
                        if (!foodstuffs.ContainsKey("Suet"))
                            foodstuffs.Add("Suet", true);
                        break;
                    case 18:
                        if (!foodstuffs.ContainsKey("Prunes"))
                            foodstuffs.Add("Prunes", true);
                        break;
                    case 19:
                        if (!foodstuffs.ContainsKey("Butter"))
                            foodstuffs.Add("Butter", true);
                        break;
                    case 20:
                        if (!foodstuffs.ContainsKey("Olive Oil"))
                            foodstuffs.Add("Olive Oil", true);
                        break;
                    default:
                        if (!foodstuffs.ContainsKey("Hardtack"))
                            foodstuffs.Add("Hardtack", true);
                        break;
                }
            }

            return string.Join(",", foodstuffs.Keys.ToList());
        }

        public Plunder ProducePlunder(PlunderCategory category, int? hint = null)
        {
            return Loot(category, hint);
        }

        public Ammunition ProduceAmmunition(SiegeEngineType engineType)
        {
            return MakeAmmunition(engineType);
        }

        public Supplies ProduceSupplies(SupplyType supplyType)
        {
            switch (supplyType)
            {
                case SupplyType.Ammunition:
                    return ProduceAmmunition(SiegeEngineType.LightBallista);
                case SupplyType.Food:
                    return ProduceCargo(CargoType.Food) as Supplies;
                case SupplyType.Medicine:
                    return ProduceCargo(CargoType.Medicine) as Supplies;
                case SupplyType.Rum:
                    return ProduceCargo(CargoType.Rum) as Supplies;
                case SupplyType.ShipSupplies:
                    return ProduceCargo(CargoType.ShipSupplies) as Supplies;
                case SupplyType.Water:
                    return ProduceCargo(CargoType.Water) as Supplies;
                default:
                    return new Supplies();
            }
        }

        public SiegeEngine ProduceSeigeEngine(SiegeEngineType engineType)
        {
            return MakeSiegeEngine(engineType);
        }

        private Ammunition MakeAmmunition(string hint)
        {
            SiegeEngineType at;
            if (hint == null)
            {
               at = (SiegeEngineType)DiceRoller.D4(1);
            }
            else if (Enum.TryParse<SiegeEngineType>(hint, out at))
            {
                // Do nothing.
            }
            else
            {
                at = (SiegeEngineType)DiceRoller.D4(1);
            }

            return MakeAmmunition(at);
        }

        private Ammunition MakeAmmunition(SiegeEngineType at)
        {
            var a = new Ammunition()
            {
                CargoType = CargoType.Ammunition,
                CargoPoints = 1,
                DaysInHold = 0,
                Fragile = false,
                Perishable = false,
                SupplyType = SupplyType.Ammunition,
                SeigeEngineType = at,
                Value = 1000
            };

            switch (a.SeigeEngineType)
            {
                case SiegeEngineType.HeavyBallista:
                    a.UnitsSupplyPerPoint = 50;
                    a.UnitsSupplyRemaining = 50;
                    a.Name = "Heavy Ballista Ammunition";
                    break;
                case SiegeEngineType.HeavyCatapolt:
                    a.UnitsSupplyPerPoint = 14;
                    a.UnitsSupplyRemaining = 14;
                    a.Name = "Heavy Catopolt Ammunition";
                    break;
                case SiegeEngineType.LightBallista:
                    a.UnitsSupplyPerPoint = 100;
                    a.UnitsSupplyRemaining = 100;
                    a.Name = "Light Ballista Ammunition";
                    break;
                case SiegeEngineType.LightCatapolt:
                    a.UnitsSupplyPerPoint = 20;
                    a.UnitsSupplyRemaining = 20;
                    a.Name = "Light Catapolt Ammunition";
                    break;
                case SiegeEngineType.Springal:
                    a.UnitsSupplyPerPoint = 100;
                    a.UnitsSupplyRemaining = 100;
                    a.Value = 2000;
                    a.Name = "Springal Ammunition";
                    break;
                case SiegeEngineType.Firedrake:
                    a.UnitsSupplyPerPoint = 50;
                    a.UnitsSupplyRemaining = 50;
                    a.Value = 10000;
                    a.Name = "Firedrake Ammunition";
                    break;
            }

            return a;
        }

        private SiegeEngine MakeSiegeEngine(string hint)
        {
            SiegeEngineType set;
            if (hint == null)
            {
                set = (SiegeEngineType)DiceRoller.Roll(4, 1);
            }
            else if (Enum.TryParse<SiegeEngineType>(hint, out set))
            {
                // Do nothing
            }
            else
            {
                set = (SiegeEngineType)DiceRoller.Roll(4, 1);
            }

            return MakeSiegeEngine(set);
        }

        private SiegeEngine MakeSiegeEngine(SiegeEngineType set)
        {
            var se = new SiegeEngine()
            {
                CargoType = CargoType.SeigeWeapon,
                DaysInHold = 0,
                Fragile = false,
                Perishable = false,
                SeigeEngineType = set
            };

            switch (se.SeigeEngineType)
            {
                case SiegeEngineType.HeavyBallista:
                    se.Name = "Heavy Ballista";
                    se.CargoPoints = 2;
                    se.Value = 800;
                    break;
                case SiegeEngineType.HeavyCatapolt:
                    se.Name = "Heavy Catapolt";
                    se.CargoPoints = 2;
                    se.Value = 800;
                    break;
                case SiegeEngineType.LightBallista:
                    se.Name = "Light Ballista";
                    se.CargoPoints = 1;
                    se.Value = 500;
                    break;
                case SiegeEngineType.LightCatapolt:
                    se.Name = "Light Catapolt";
                    se.CargoPoints = 1;
                    se.Value = 550;
                    break;
                case SiegeEngineType.Springal:
                    se.Name = "Springal";
                    se.CargoPoints = 2;
                    se.Value = 1000;
                    break;
                case SiegeEngineType.Firedrake:
                    se.Name = "Firedrake";
                    se.CargoPoints = 2;
                    se.Value = 4000;
                    break;
                case SiegeEngineType.Corvus:
                    se.Name = "Corvus";
                    se.CargoPoints = 1;
                    se.Value = 100;
                    break;
            }

            return se;
        }

        private Plunder Loot(string hint)
        {
            PlunderCategory pc;
            if (hint == null)
            {
                pc = (PlunderCategory)((DiceRoller.D6(2) - 1) % 5 + 1);
            }
            else if (Enum.TryParse<PlunderCategory>(hint, out pc))
            {
                // Do nothing.
            }
            else
            {
                pc = (PlunderCategory)((DiceRoller.D6(2) - 1) % 5 + 1);
            }

            return Loot(pc);
        }

        private Plunder Loot(PlunderCategory category, int? hint = null)
        {
            var p = new Plunder()
            {
                CargoType = CargoType.Plunder,
                PlunderCategory = category,
                DaysInHold = 0,
                Value = 1000
            };

            switch (p.PlunderCategory)
            {
                case PlunderCategory.Bulk:
                    p.CargoPoints = 6;
                    break;
                case PlunderCategory.Commodity:
                    p.CargoPoints = 4;
                    break;
                case PlunderCategory.Trade:
                    p.CargoPoints = 3;
                    break;
                case PlunderCategory.Luxury:
                    p.CargoPoints = 2;
                    break;
                case PlunderCategory.Precious:
                    p.CargoPoints = 1;
                    break;
                default:
                    p.CargoPoints = 4;
                    break;
            }

            bool perishable;
            bool fragile;
            bool livestock;
            if(hint.HasValue)
                p.Name = GetPlunderName(p.PlunderCategory, hint.Value, out perishable, out fragile, out livestock);
            else
                p.Name = GetPlunderName(p.PlunderCategory, out perishable, out fragile, out livestock);
            p.Perishable = perishable;
            p.Fragile = fragile;
            p.IsLivestock = livestock;

            return p;
        }

        private Cargo AddBoat(string hint)
        {
            BoatClasses boat;
            if (hint == null)
            {
                boat = (BoatClasses)((DiceRoller.D6(2) - 1) % 5 + 1);
            }
            else if (Enum.TryParse<BoatClasses>(hint, out boat))
            {
                // Do nothing.
            }
            else
            {
                boat = (BoatClasses)((DiceRoller.D6(2) - 1) % 5 + 1);
            }

            return AddBoat(boat);
        }

        private Cargo AddBoat(BoatClasses boatClass)
        {
            Cargo boat = new Cargo()
            {
                CargoType = CargoType.ShipsBoat,
                DaysInHold = 0,
                Fragile = false,
                Perishable = false
            };

            switch(boatClass)
            {
                case BoatClasses.Barge:
                    boat.CargoPoints = 5;
                    boat.Value = 800;
                    break;
                case BoatClasses.Cutter:
                    boat.Name = "Cutter";
                    boat.CargoPoints = 2;
                    boat.Value = 250;
                    break;
                case BoatClasses.Dingy:
                    boat.Name = "Dingy";
                    boat.CargoPoints = 0;
                    boat.Value = 50;
                    break;
                case BoatClasses.Jollyboat:
                    boat.Name = "Jollyboat";
                    boat.CargoPoints = 1;
                    boat.Value = 100;
                    break;
                case BoatClasses.Launch:
                    boat.Name = "Launch";
                    boat.CargoPoints = 6;
                    boat.Value = 1000;
                    break;
                case BoatClasses.Longboat:
                    boat.Name = "Longboat";
                    boat.CargoPoints = 4;
                    boat.Value = 500;
                    break;
                case BoatClasses.Pinnace:
                    boat.Name = "Pinnace";
                    boat.CargoPoints = 3;
                    boat.Value = 400;
                    break;
                case BoatClasses.Gig:
                    boat.Name = "Gig";
                    boat.Value = 200;
                    break;
            }

            return boat;
        }

        private CargoFactory()
        {

        }
    }
}
