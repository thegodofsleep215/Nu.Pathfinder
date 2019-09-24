using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class Supplies : Cargo, ISupply
    {
        private int _unitSuppliesRemaining { get; set; }
        public int CargoPointsRemaining { get; set; }
        public override int CargoPoints
        {
            get
            {
                if (_unitSuppliesRemaining == UnitsSupplyPerPoint)
                    return CargoPointsRemaining;
                else
                    return CargoPointsRemaining + ((_unitSuppliesRemaining > 0) ? 1 : 0);
            }
            set
            {
                CargoPointsRemaining = value;
            }
        }
        public SupplyType SupplyType { get; set; }
        public int UnitsSupplyPerPoint { get; set; }
        public int UnitsSupplyRemaining
        {
            get
            {
                return _unitSuppliesRemaining;
            }
            set
            {
                _unitSuppliesRemaining = value;
            }
        }

        [JsonIgnore]
        public bool IsOpen
        {
            get
            {
                return UnitsSupplyRemaining < UnitsSupplyRemaining;
            }
        }

        [JsonIgnore]
        public bool IsExausted
        {
            get
            {
                return CargoPointsRemaining < 0;
            }
        }
        [JsonIgnore]
        public override decimal SellableValue
        {
            get
            {
                if ((CargoPoints == 0) || (UnitsSupplyPerPoint == 0))
                    return 0;
                else if (_unitSuppliesRemaining == UnitsSupplyPerPoint)
                    return Math.Floor(Value * (CargoPointsRemaining / CargoPoints));
                else
                    return Math.Floor(Value * (CargoPointsRemaining / CargoPoints) + Value * (_unitSuppliesRemaining / (CargoPoints * UnitsSupplyPerPoint)));
            }
        }

        public int AdjustSupplies(int amount)
        {
            int missing = 0;  

            bool subtraction = amount < 0;
            int units = Math.Abs(amount) % UnitsSupplyPerPoint;
            int cargo = Math.Abs(amount) / UnitsSupplyPerPoint;

            if (subtraction)
            {
                if (units < _unitSuppliesRemaining)
                {
                    CargoPointsRemaining -= cargo;
                    _unitSuppliesRemaining -= units;
                }
                else
                {
                    CargoPointsRemaining -= (cargo + 1);
                    _unitSuppliesRemaining = UnitsSupplyPerPoint + _unitSuppliesRemaining - units;
                }
            }
            else
            {
                if (units < (UnitsSupplyPerPoint - _unitSuppliesRemaining))
                {
                    CargoPointsRemaining += cargo;
                    _unitSuppliesRemaining += units;
                }
                else
                {
                    CargoPointsRemaining += (cargo + 1);
                    _unitSuppliesRemaining = _unitSuppliesRemaining + units - UnitsSupplyPerPoint;
                }
            }

            if (IsExausted)
            {
                if (CargoPointsRemaining < 0)
                    missing = (UnitsSupplyPerPoint * CargoPointsRemaining * -1);
                missing += (_unitSuppliesRemaining * -1);
            }

            if (IsExausted || cargo > 1)
            {
                OnExhuastion();   
            }

            return missing;
        }
   
        public void OnExhuastion()
        {
           
        }
    }
}
