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
        private int _cargoPointsRemaining { get; set; }
        public override int CargoPoints
        {
            get
            {
                if (_unitSuppliesRemaining == UnitsSupplyPerPoint)
                    return _cargoPointsRemaining;
                else
                    return _cargoPointsRemaining + ((_unitSuppliesRemaining > 0) ? 1 : 0);
            }
            set
            {
                _cargoPointsRemaining = value;
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
                return _cargoPointsRemaining < 0;
            }
        }
        [JsonIgnore]
        public override decimal SellableValue
        {
            get
            {
                if ((CargoPoints == 0) || (UnitsSupplyPerPoint == 0))
                    return 0;
                else
                    // TODO: Test this formula.
                    return Math.Floor(Value * (_cargoPointsRemaining / CargoPoints) + Value * (_unitSuppliesRemaining / (CargoPoints * UnitsSupplyPerPoint)));
            }
        }

        public int AdjustSupplies(int amount)
        {
            int missing = 0;  // TODO: Calculate how much we are short.

            bool subtraction = amount < 0;
            int units = Math.Abs(amount) % UnitsSupplyPerPoint;
            int cargo = Math.Abs(amount) / UnitsSupplyPerPoint;

            if (subtraction)
            {
                if (units < _unitSuppliesRemaining)
                {
                    _cargoPointsRemaining -= cargo;
                    _unitSuppliesRemaining -= units;
                }
                else
                {
                    _cargoPointsRemaining -= (cargo + 1);
                    _unitSuppliesRemaining = UnitsSupplyPerPoint + _unitSuppliesRemaining - units;
                }
            }
            else
            {
                if (units < (UnitsSupplyPerPoint - _unitSuppliesRemaining))
                {
                    _cargoPointsRemaining += cargo;
                    _unitSuppliesRemaining += units;
                }
                else
                {
                    _cargoPointsRemaining += (cargo + 1);
                    _unitSuppliesRemaining = _unitSuppliesRemaining + units - UnitsSupplyPerPoint;
                }
            }

            if (IsExausted)
            {
                if (_cargoPointsRemaining < 0)
                    missing = (UnitsSupplyPerPoint * _cargoPointsRemaining * -1);
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
