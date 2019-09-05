using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    [Serializable]
    public class CargoHold : List<Cargo>
    {
        [JsonIgnore]
        public List<Plunder> AllPlunder
        {
            get
            {
                return this.Where(a => a.GetType() == typeof(Plunder)).Cast<Plunder>().ToList();
            }
        }

        [JsonIgnore]
        public List<Supplies> AllSupplies
        {
            get
            {
                return this.Where(a => a.GetType() == typeof(Supplies)).Cast<Supplies>().ToList();
            }
        }

        [JsonIgnore]
        public List<Ammunition> AllAmmuntion
        {
            get
            {
                return AllSupplies.Where(a => a.SupplyType == SupplyType.Ammunition).Cast<Ammunition>().ToList();
            }
        }

        [JsonIgnore]
        public int TotalCargoPointsUsed
        {
            get
            {
                return this.Sum(a => a.CargoPoints);
            }
        }

        [JsonIgnore]
        public int AnimalUnitsAboard
        {
            get
            {
                return AllPlunder.Count(a => a.IsLivestock);
            }
        }

        public void ResetPassengers(int count)
        {
            this.RemoveAll(a => a.CargoType == CargoType.Passengers);

            int units = count / 10 + (count % 10 == 0 ? 0 : 1);

            for(int i = 0; i < units; i++)
            {
                this.Add(CargoFactory.Instance.ProduceCargo(CargoType.Passengers));
            }
        }

        public void ConsumeSupplies(int totalCrew)
        {
            ConsumeSupplies(totalCrew, 1);
        }

        public void ConsumeSupplies(int totalCrew, int days)
        {
            ConsumeSupply(SupplyType.Water, totalCrew * days * -1);
            ConsumeSupply(SupplyType.Food, totalCrew * days * -1);
            ConsumeSupply(SupplyType.Rum, totalCrew * days * -1);
            AgeCargo(days);
        }

        private void AgeCargo(int days)
        {
            foreach(Cargo c in this)
            {
                c.DaysInHold += days;
            }
        }

        public void ConsumeSupply(SupplyType supplyType, int count)
        {
            var supply = this.AllSupplies.FirstOrDefault(a => a.SupplyType == supplyType && a.IsOpen && !a.IsExausted);

            if (supply == null || string.IsNullOrEmpty(supply.Name))
                supply = this.AllSupplies.FirstOrDefault(a => a.SupplyType == supplyType && !a.IsExausted);

            if (supply == null && string.IsNullOrEmpty(supply.Name))
            {
                // TODO: All out of needed supply!
            }
            else
            {
                var deficit = supply.AdjustSupplies(count);

                if(supply.IsExausted)
                {
                    ConsumeSupply(supplyType, deficit);
                }
            }
        }

        public void ConsumeAmmuntion(SiegeEngineType siegeEngine, int count)
        {
            var supply = this.AllAmmuntion.FirstOrDefault(a => a.SeigeEngineType == siegeEngine && a.IsOpen && !a.IsExausted);

            if (supply == null || string.IsNullOrEmpty(supply.Name))
                supply = this.AllAmmuntion.FirstOrDefault(a => a.SeigeEngineType == siegeEngine && !a.IsExausted);

            if (supply == null && string.IsNullOrEmpty(supply.Name))
            {
                // TODO: All out of needed supply!
            }
            else
            {
                var deficit = supply.AdjustSupplies(count);

                if (supply.IsExausted && deficit > 0)
                {
                    ConsumeAmmuntion(siegeEngine, deficit);
                }
            }
        }

        public void ConsumeFodder(int animalUnits)
        {
            ConsumeSupply(SupplyType.Fodder, animalUnits * -1);
            ConsumeSupply(SupplyType.Water, animalUnits * -30);
        }
    }
}
